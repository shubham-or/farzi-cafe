using FishNet;
using FishNet.Managing;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Managing.Object;
using FishNet.Object;
using FishNet.Transporting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FirstGearGames.LobbyAndWorld.Lobbies;
using FirstGearGames.LobbyAndWorld.Lobbies.JoinCreateRoomCanvases;
using FishNet.Managing.Timing;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using FishNet.Object.Synchronizing;

public class ServerInstancing : NetworkBehaviour
{
    [SerializeField] private GameObject playerNetworkPrefab;
    [SerializeField] private Vector3 playerPosition;

    public Dictionary<string, RoomDetails> currentRoomsRunning = new Dictionary<string, RoomDetails>();
    RoomDetails currentRoom;

    public int localSpawnIndex = 0;

    private const int MAX_PLAYERS = 5;
    private int WAITING_FOR_PLAYERS_DURATION = 3;


    private enum ParamsTypes
    {
        ServerLoad,
        MemberLeft
    }

    public static ServerInstancing Instance;
    private NetworkManager _networkManager;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _networkManager = FindObjectOfType<NetworkManager>();

        if (_networkManager == null)
        {
            Debug.LogError("NetworkManager not found, HUD will not function.");
            return;
        }
    }


    private void Start()
    {
#if !UNITY_EDITOR
        if(IsServer)
            WAITING_FOR_PLAYERS_DURATION = 30;
#endif
        InstanceFinder.SceneManager.OnLoadEnd += OnSceneLoadEnd;
    }



    [ServerRpc(RequireOwnership = false)]
    public void QuickRaceConnect(UserData userData, NetworkConnection connection = null, bool isConnectingToLastGame = false)
    {
        if (currentRoom != null)
        {
            Debug.Log("current room is not null");
            JoinRoom(connection, currentRoom, userData);
        }
        else
        {
            Debug.Log("current room is null");
            CreateRoom(() => { JoinRoom(connection, currentRoom, userData); });
        }
    }

    private RoomDetails FindRoomInUserAlreadyExists(UserData _userData)
    {
        return null;
    }


    private void CreateRoom(Action callback = null)
    {
        RoomDetails room = new RoomDetails();
        room.ID = Guid.NewGuid().ToString();
        room.Name = currentRoomsRunning.Count.ToString() + "FarziCafe" + Random.Range(1000, 10000).ToString();
        room.sceneHandle = currentRoomsRunning.Count + 1;
        room.MaxPlayers = MAX_PLAYERS;
        room.dishData = GameManager.Instance.cluesManager.SelectRandomDish();

        if (currentRoomsRunning.ContainsKey(room.ID))
            currentRoomsRunning[room.ID] = room;
        else
            currentRoomsRunning.Add(room.ID, room);

        currentRoom = room;
        callback?.Invoke();
    }

    private void JoinRoom(NetworkConnection connection, RoomDetails room, UserData userData)
    {
        bool isFirstPlayer = room.userData.Count == 0 ? true : false;

        // New User
        if (string.IsNullOrWhiteSpace(userData.userDataServer.roomId) && string.IsNullOrWhiteSpace(userData.userDataServer.roomName))
        {
            Debug.Log("New User is added");
            room.userData.Add(userData.userDataServer.uid, userData);
            room.currentConnections.Add(userData.userDataServer.uid, connection);
        }
        else
        {
            // Returning user
            Debug.Log("Returning User");
            bool roomFound = false;
            if (currentRoomsRunning.ContainsKey(userData.userDataServer.roomId))
            {
                room = currentRoomsRunning[userData.userDataServer.roomId];
                if (room.userData.ContainsKey(userData.userDataServer.uid))
                    roomFound = !room.userData[userData.userDataServer.uid].hasCompleted;

                Debug.Log("Room found for Returning User " + room.Name);
            }

            if (roomFound)
            {
                if (room.userData.ContainsKey(userData.userDataServer.uid))
                {
                    Debug.Log("User is replaced");
                    room.userData[userData.userDataServer.uid] = userData;

                    // Current Connections
                    if (room.currentConnections.ContainsKey(userData.userDataServer.uid))
                        room.currentConnections[userData.userDataServer.uid] = connection;
                    else
                        room.currentConnections.Add(userData.userDataServer.uid, connection);
                }
                else
                {
                    print(userData.userDataServer.uid + " - User not for Found in the room - " + room.ID);
                }
            }
            else
            {
                Debug.Log("Room NOT found for Returning User. Returning User is added to the CURRENT ROOM");
                room.userData.Add(userData.userDataServer.uid, userData);
                room.currentConnections.Add(userData.userDataServer.uid, connection);
            }
        }


        // Add User to Leaderboard on Server
        UserData _userDataRoom = room.userData[userData.userDataServer.uid];
        _userDataRoom.dishData = room.dishData;
        LeaderBoardItem _leaderBoardUser = new LeaderBoardItem() { id = _userDataRoom.userDataServer.uid, userName = _userDataRoom.userDataServer.userName, rank = "5", dishName = _userDataRoom.dishData.Dish_Name, isBot = false };

        if (room.userData.ContainsKey(userData.userDataServer.uid))
            room.userData[userData.userDataServer.uid].leaderBoard = _leaderBoardUser;

        // Set Room and Dish For Joined User
        ClientServerManager.Instance.SetDataForUser(connection, room, _leaderBoardUser);


        // Start countdown when first user joins the room to wait for other players to join
        Coroutine waitForOtherplayers = null;
        if (isFirstPlayer)
        {
            print("First user Joined Setup");
            waitForOtherplayers = GameManager.Instance.timer.StartCountdown(WAITING_FOR_PLAYERS_DURATION, 1, room.ID, UpdateWaitingTimeForAllPlayers, () =>
            {
                // Start Game
                UpdateWaitingTimeForAllPlayers(0, room.ID);
                currentRoom = null;
                StartGame(room.ID, GameManager.Instance.gamePlaySceneName, connection);
            });
        }

        currentRoomsRunning[room.ID] = room;

        // Start Game for All users
        if (room.userData.Count == room.MaxPlayers && !room.hasGameStarted)
        {
            Debug.Log("Room is Full - Start Game");
            currentRoom = null;
            foreach (var conn in room.currentConnections)
            {
                currentRoom = null;
                if (waitForOtherplayers != null)
                    GameManager.Instance.timer.StopTimer(waitForOtherplayers);

                StartGame(room.ID, GameManager.Instance.gamePlaySceneName, connection);
            }
        }
        else if (room.hasGameStarted) // Start game for returning user
        {
            StartGame(room.ID, GameManager.Instance.gamePlaySceneName, connection);
        }
    }

    public void StartGame(string roomID, string sceneName, NetworkConnection conn, bool isSingleConnection = false, NetworkObject networkObject = null, int _botsCount = 0)
    {

        RoomDetails _room = currentRoomsRunning[roomID];
        print($"Game Started for room - {_room.Name}");
        //List<NetworkObject> networkObjects = new List<NetworkObject>();

        // Bots Setup
        int botsCount = _room.MaxPlayers - _room.userData.Count;
        for (int i = 0; i < botsCount; i++)
        {
            UserData _botData = new UserData().CreateBotData();
            LeaderBoardItem _leaderBoardBot = new LeaderBoardItem()
            {
                id = _botData.userDataServer.uid,
                userName = _botData.userDataServer.userName,
                rank = "5",
                dishName = _room.dishData.Dish_Name,
                isBot = true
            };

            // Set Details For User
            _botData.SetRoomDetails(_room);
            _botData.SetDishData(_room.dishData);
            _botData.SetLeaderboardData(_leaderBoardBot);


            // Add Details to Room
            _room.userData.Add(_botData.userDataServer.uid, _botData);
            _room.userData[_botData.userDataServer.uid].leaderBoard = _leaderBoardBot;
            print(_botData.userDataServer.userName + "-Bot User Data added in Room-" + _room.Name);

            //foreach (var item in _room.currentConnections)
            //{
            //    ClientServerManager.Instance.GenerateBot(item.Value, _botData);
            //}
        }

        // Spawn
        //foreach (var item in _room.currentConnections)
        //{
        //    networkObjects.Add(SpawnNetworkPlayer(playerNetworkPrefab, _room, item.Value));
        //}

        SceneLookupData lookup = new SceneLookupData(_room.sceneHandle, sceneName);
        SceneLoadData sld = new SceneLoadData(lookup);
        //sld.MovedNetworkObjects = networkObjects.ToArray();

        LoadParams loadParams = new LoadParams
        {
            ServerParams = new object[]
            {
                ParamsTypes.ServerLoad,
                _room,
                sld
            }
        };

        sld.Options.AllowStacking = true;
        sld.Options.AutomaticallyUnload = true;
        sld.ReplaceScenes = ReplaceOption.None;
        sld.Params = loadParams;

        if (!isSingleConnection)
            InstanceFinder.SceneManager.LoadConnectionScenes(_room.currentConnections.Values.ToArray(), sld);
        else
            InstanceFinder.SceneManager.LoadConnectionScenes(sld);
    }

    private void SpawnNetworkPlayer(GameObject _playerPrefab, RoomDetails _room = null, NetworkConnection connection = null)
    {
        Vector3 _position = playerPosition;
        float _multiplier = (connection.ClientId + 1) * 0.04f;
        _position.x = _position.x + playerPosition.x * _multiplier;
        _position.z = _position.z + playerPosition.z * _multiplier;
        print("Player Spawned at Position - " + _position);

        GameObject _player = Instantiate(_playerPrefab, _position, Quaternion.identity);

        if (connection != null)
            Spawn(_player, connection);

        //return _player.GetComponent<NetworkObject>();
    }



    [ServerRpc(RequireOwnership = false)]
    public void StopGame(UserData _userData, string _roomId, string _sceneName, bool _unloadForAll = false, NetworkConnection connection = null)
    {
        if (currentRoomsRunning.ContainsKey(_roomId))
        {
            print(_userData.userDataServer.userName + " - Unloading Scene For Room - " + currentRoomsRunning[_roomId].Name);
            currentRoomsRunning[_roomId].userData[_userData.userDataServer.uid].userDataServer.score = _userData.userDataServer.score;
            currentRoomsRunning[_roomId].userData[_userData.userDataServer.uid].hasCompleted = true;

            SceneUnloadData sud = new SceneUnloadData(currentRoomsRunning[_roomId].sceneHandle);
            NetworkConnection[] conns = _unloadForAll ? currentRoomsRunning[_roomId].currentConnections.Values.ToArray() : new NetworkConnection[] { connection };
            NetworkManager.SceneManager.UnloadConnectionScenes(conns, sud);

            if (currentRoomsRunning[_roomId].IsGameCompleted())
                currentRoomsRunning.Remove(_roomId);
        }
    }

    [Server]
    public float GetRoomTime(string _roomID)
    {
        if (currentRoomsRunning.ContainsKey(_roomID)) return currentRoomsRunning[_roomID].roomTime;
        else return 0;
    }

    #region Update Time
    private void UpdateWaitingTimeForAllPlayers(float _timeLeft, string _roomID)
    {
        foreach (var item in currentRoom.currentConnections)
        {
            ClientServerManager.Instance.UpdateWaitTime(item.Value, _timeLeft);
        }
    }

    private void UpdateGamePlayTimeForAllPlayers(float _timeGone, string _roomId)
    {
        if (currentRoomsRunning.ContainsKey(_roomId))
        {
            foreach (var item in currentRoomsRunning[_roomId].currentConnections)
            {
                ClientServerManager.Instance.UpdateGamePlayTime(item.Value, _timeGone);
            }
        }
    }

    private void UpdateRoomTime(float _time, string _roomID)
    {
        if (currentRoomsRunning.ContainsKey(_roomID))
        {
            currentRoomsRunning[_roomID].roomTime = _time;
            UpdateGamePlayTimeForAllPlayers(_time, _roomID);
        }
    }
    #endregion



    #region Scene
    [ServerRpc(RequireOwnership = false)]
    private void Server_UnLoadScene(NetworkConnection connection, string sceneName, RoomDetails room)
    {
        SceneLookupData lookup = new SceneLookupData(room.sceneHandle, sceneName);
        SceneUnloadData sld = new SceneUnloadData(lookup.Handle);

        UnloadParams unloadParams = new UnloadParams
        {
            ServerParams = new object[]
            {
                ParamsTypes.ServerLoad,
                room,
                sld
            }
        };

        sld.SceneLookupDatas = new[] { lookup };
        sld.Params = unloadParams;
        sld.Options.Mode = UnloadOptions.ServerUnloadMode.UnloadUnused;
        InstanceFinder.SceneManager.UnloadConnectionScenes(connection, sld);
    }

    public void UnLoadSceneViaServer(NetworkConnection connection, string sceneName, RoomDetails room)
    {
        Server_UnLoadScene(connection, sceneName, room);
    }

    //[ServerRpc(RequireOwnership =false)]
    public void OnSceneLoadEnd(SceneLoadEndEventArgs obj)
    {
        if (base.IsClient)
        {
            //RoomData roomData = SaveSystem.Load();
            //roomData.IsEnded = true;
            //roomData.IsSimulationStarted = true;
            //UserMetaData.currentRoom.IsSimulationStarted = true;
            //SaveSystem.Save(roomData);
        }

        if (!base.IsServer)
            return;

        object[] temp_ServerParams = obj.QueueData.SceneLoadData.Params.ServerParams;

        if (temp_ServerParams.Length == 0)
            return;


        RoomDetails _room = temp_ServerParams[1] as RoomDetails;

        foreach (var _conn in _room.currentConnections)
        {
            SpawnNetworkPlayer(playerNetworkPrefab, _room, _conn.Value);

            foreach (var _user in _room.userData)
                if (_user.Value.isBot)
                    ClientServerManager.Instance.GenerateBot(_conn.Value, _user.Value);
        }


        if (!_room.hasGameStarted)
        {
            _room.hasGameStarted = true;
            _room.startTime = (float)GameManager.Instance.timeManager.TicksToTime(TickType.Tick);
            GameManager.Instance.timer.StartTimer(0, 3600, 1, _room.ID, UpdateRoomTime, () => { print("Timer on Server FINISHED for room -> " + _room.Name); });
            print("Timer On Server Started For room -> " + _room.Name);
        }


        foreach (var item in _room.currentConnections)
            ClientServerManager.Instance.StartGamePlay(item.Value);


        //ClientServerManager.Instance.ActivatePlayerScripts(item.Value, item.Key);


        Debug.Log("Sceneloaded and even argument Room name " + _room.Name);

        //Debug.Log("Loaded scenes count" + " " + obj.LoadedScenes.Length);

        int handle = obj.QueueData.SceneLoadData.SceneLookupDatas[0].Handle;
        foreach (Scene scene in obj.LoadedScenes)
        {
            //Debug.Log("loaded scene handle" + " " + scene.handle);
        }

        //Debug.Log("Handle data on scene load end" + " " + handle);


        foreach (NetworkConnection connection in obj.QueueData.Connections)
        {

        }
    }
    #endregion



    [ServerRpc(RequireOwnership = false)]
    public void GetLeaderboard(string _uid, string _roomId)
    {
        if (currentRoomsRunning.ContainsKey(_roomId))
        {
            if (currentRoomsRunning[_roomId].userData.ContainsKey(_uid))
            {
                Dictionary<string, LeaderBoardItem> tempLeaderBoard = new Dictionary<string, LeaderBoardItem>();
                foreach (var item in currentRoomsRunning[_roomId].userData)
                {
                    if (!tempLeaderBoard.ContainsKey(item.Value.userDataServer.uid))
                        tempLeaderBoard.Add(item.Value.userDataServer.uid, item.Value.leaderBoard);

                    if (!item.Value.hasCompleted)
                        item.Value.leaderBoard.time = currentRoomsRunning[_roomId].roomTime;
                }

                print($"RoomTime | GetLeaderboard: UID Room - {currentRoomsRunning[_roomId].Name} - {currentRoomsRunning[_roomId].roomTime}");

                ClientServerManager.Instance.SetLeaderboard(currentRoomsRunning[_roomId].currentConnections[_uid], tempLeaderBoard);
            }
        }
    }

    [ServerRpc]
    public void UpdateUserTimeOnServerLeaderboard(string _uid, string _roomId)
    {
        if (currentRoomsRunning.ContainsKey(_roomId))
        {
            if (currentRoomsRunning[_roomId].userData.ContainsKey(_uid))
            {
                print($"UpdateUserTimeOnServerLeaderboard {currentRoomsRunning[_roomId].Name} | {currentRoomsRunning[_roomId].userData[_uid].userDataServer.userName}");
                currentRoomsRunning[_roomId].userData[_uid].leaderBoard.time = currentRoomsRunning[_roomId].roomTime;
            }
        }
    }


    private void ClienDisConnected(NetworkConnection connection, RoomDetails room)
    {
        // if client is in a temp room waiting
        if (room.MemberIds.Count < room.MaxPlayers)
            DisconnectedInWait(connection);
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnClientDisConnectedRpc(NetworkConnection connection = null)
    {
        Debug.Log("Client disconnected with id" + "  " + connection.ClientId);
        Debug.Log("Client disconnected with address" + "  " + connection.GetAddress());
    }

    private void DisconnectedInWait(NetworkConnection connection)
    {
    }

    private void DisconnectedGameRunning(NetworkConnection connection)
    {
    }

    public void OnRemoteConnectionStateChanged(NetworkConnection connection)
    {

    }

    private void OnDestroy()
    {
        if (InstanceFinder.SceneManager)
            InstanceFinder.SceneManager.OnLoadEnd -= OnSceneLoadEnd;
    }

}