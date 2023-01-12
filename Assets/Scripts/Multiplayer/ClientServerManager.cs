using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using System.Linq;

public class ClientServerManager : NetworkBehaviour
{
    public GameObject botPrefab;
    public Vector3 playerPosition;

    public double serverTime = 0;

    public static ClientServerManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        print("CLIENT STARTED----------");
    }


    [TargetRpc]
    public void SetDataForUser(NetworkConnection connection, RoomDetails _room, LeaderBoardItem _leaderBoard, bool _restoreGame)
    {
        print("SetRoomAndDishDetailsForUser -> " + _room.dishData.Dish_Name);

        //RoomData roomData = new RoomData()
        //{
        //    Name = _room.Name,
        //    IsSimulationStarted = _room.IsSimulationStarted,
        //    IsEnded = _room.IsEnded,
        //};

        // SaveSystem.Save(roomData);

        //PlayerPrefs.SetString(UserMetaData.myAddress, JsonConvert.SerializeObject(roomData));
        //PlayerPrefs.Save();

        //Debug.Log("Room Details Saved :   " + roomData.Name);

        //UserMetaData.currentRoom = _room;

        GameManager.Instance.SetRoomDetails(_room);
        GameManager.Instance.SetDishData(_room.dishData);
        GameManager.Instance.SetLeaderboardData(_leaderBoard);
        GameManager.Instance.cluesManager.Setup(_room.dishData, _restoreGame ? GameManager.Instance.savedData.userDataServer.currentIngredientIndex : 0);

        //Debug.Log("DIsh COunt For USer:   " + _room.userData[UserMetaData.myAddress].userDishes.selectedDish.Count);
    }



    [TargetRpc]
    public void SetLeaderboard(NetworkConnection networkConnection = null, Dictionary<string, LeaderBoardItem> _leaderBoard = null)
    {
        print("Set leaderBoard Target RPC");
        ScreenManager.Instance.leaderboardScreen.SetLeaderboard(_leaderBoard.OrderBy(x => x.Value.time).ToDictionary(x => x.Key, x => x.Value));
    }

    [TargetRpc]
    public void StartGamePlay(NetworkConnection networkConnection)
    {
        print("Gameplay Started " + gameObject.name);
        PopUpManager.Instance.HideWaitingForOtherPlayers();
        ScreenManager.Instance.SwitchScreen(ScreenManager.Instance.menuScreen.gameObject, ScreenManager.Instance.gameplayScreen.gameObject);
        ScreenManager.Instance.gameplayScreen.StartGamePlay();
        GameManager.Instance.hasGameStarted = true;
    }

    [TargetRpc]
    public void UpdateWaitTime(NetworkConnection networkConnection, float _timeLeft, string _onfinishMsg = null)
    {
        PopUpManager.Instance.UpdateWaitingTime(_timeLeft);
    }

    [TargetRpc]
    public void UpdateGamePlayTime(NetworkConnection networkConnection, float _timeGone)
    {
        ScreenManager.Instance.gameplayScreen.UpdateGamePlayTime(_timeGone);
        if (!GameManager.Instance.GetUserData().hasCompleted)
            GameManager.Instance.SetGameTime(_timeGone);
    }


    int counter;
    [TargetRpc]
    public void GenerateBot(NetworkConnection networkConnection, UserData _botData)
    {
        if (counter >= 4) counter = 0;

        print("Bot Created on Client - " + _botData.userDataServer.userName);
        Vector3 _position = playerPosition;
        float _multiplier = (++counter) * 0.06f;
        _position.x += playerPosition.x * _multiplier;
        _position.z += playerPosition.z * _multiplier;
        AI aI = Instantiate(botPrefab, _position, Quaternion.identity).GetComponent<AI>();
        aI.GetComponent<Bot>().Init(_botData);
        GameManager.Instance.AddBot(_botData);
        //UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(aI.gameObject, GameplayScene.Instance.gameObject.scene);
    }



    //public void Update()
    //{
    //    //serverTime = TimeManager.TicksToTime(TickType.Tick);
    //}




    private bool AllUserAreReady(string roomName)
    {
        if (ServerInstancing.Instance.currentRoomsRunning[roomName].userData.Count != ServerInstancing.Instance.currentRoomsRunning[roomName].MaxPlayers)
            return false;


        foreach (var userData in ServerInstancing.Instance.currentRoomsRunning[roomName].userData.Values)
        {
            //if (userData.userDishes.selectedDish.Count != 2)
            //    return false;
        }

        return true;
    }

    [TargetRpc]
    public void OnMemberReady(NetworkConnection connection, RoomDetails roomDetails)
    {
        //if (_lobbyManager)
        //    _lobbyManager.OnMembeGetReady(roomDetails);
    }

    //IEnumerator StartGameAfterDelay(string roomName)
    //{
    //    yield return new WaitForSeconds(3f);
    //    ServerInstancing.instance.StartGame(roomName, "GamePlay");
    //}

    [TargetRpc]
    //public void GetCustomersData(NetworkConnection connection, Dictionary<string, UserData> userData, RoomDetails room, SimulationData simulationDatas)
    public void GetCustomersData(NetworkConnection connection, Dictionary<string, UserData> userData, RoomDetails room)
    {
        //UserMetaData.currentRoom = room;
        //UserMetaData.userData = new Dictionary<string, UserData>(userData);
        //UserMetaData.customersSimulation = simulationDatas;
    }

    public class Startplay
    {
        public List<string> addresses = new List<string>();
        public int fee;
    }

    IEnumerator StartPlay(RoomDetails room, Action callback)
    {
        Startplay startplay = new Startplay();


        startplay.fee = 10;
        foreach (var data in room.userData.Values)
        {
            //startplay.addresses.Add(data.address);
        }


        JObject obj = JObject.FromObject(new
        {
            startplay = startplay
        });


        string json = JsonConvert.SerializeObject(obj);


        //using (UnityWebRequest www = UnityWebRequest.Put(Constants.BASE_URL + "startPlay", json))
        using (UnityWebRequest www = UnityWebRequest.Put("startPlay", json))
        {
            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", "application/json");

            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Result awardObject = StartRewardDistributing(room);

                Debug.Log("Start Game Data:   " + json);
                Debug.Log("Game Started!    -     " + www.downloadHandler.text);

                //StartCoroutine(DistributeRewards(awardObject, room));
                callback();
            }
        }
    }

    //public Result StartRewardDistributing(RoomDetails room)
    //{
    //    Result result = new Result();

    //    int _score = 0;
    //    int index = 0;

    //    foreach (var data in room.userData.Values)
    //    {
    //        _score = 0;
    //        foreach (var dish in data.userDishes.selectedDish)
    //        {
    //            _score += dish.price / 2;
    //        }

    //        Address temp = new Address()
    //        {
    //            address = data.address,
    //            score = _score
    //        };
    //        result.addresses.Add(temp);
    //        index++;
    //    }
    //    return result;
    //}


    //IEnumerator DistributeRewards(Result result, RoomDetails room)

    IEnumerator DistributeRewards(RoomDetails room)
    {
        yield return new WaitForSeconds(90f);


        JObject obj = JObject.FromObject(new
        {
            //result = result
        });


        string json = JsonConvert.SerializeObject(obj);


        //using (UnityWebRequest www = UnityWebRequest.Put(Constants.BASE_URL + "distributeRewards", json))
        using (UnityWebRequest www = UnityWebRequest.Put("distributeRewards", json))
        {
            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (ServerInstancing.Instance.currentRoomsRunning.ContainsKey(room.Name))
                    ServerInstancing.Instance.currentRoomsRunning.Remove(room.Name);

                Debug.Log("Reward post Data!    -     " + json);
                Debug.Log("Reward Distributed!    -     " + www.downloadHandler.text);
            }
        }
    }

}