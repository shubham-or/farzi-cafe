using FishNet;
using FishNet.Managing.Timing;
using NaughtyCharacter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string gamePlaySceneName = "GamePlay";
    public bool useFirebase = false;
    public bool useWeb3 = false;
    public bool isGameplayPaused = false;
    public bool hasGameStarted = false;
    public bool isRunningLocal = false;

    [Header("-----Prefabs-----")]
    public GameObject playerLocalPrefab;
    public GameObject botPrefab;

    [Header("-----References-----")]
    public TimeManager timeManager;
    public UI_Handler uIHandler;
    public Timer timer;
    public CluesManager cluesManager;
    public Animator leftdoor, rightdoor;
    public Player player;
    public Character character;
    public Player_Interaction playerInteraction;
    public PlayerDataHandler playerDataHandler;

    [Header("-----User Data-----")]
    [SerializeField] private UserData userData;
    [Space(10)] [SerializeField] private List<UserData> botsData = new List<UserData>();
    [Space(10)] public UserData savedData;


    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
        playerDataHandler = new PlayerDataHandler();
    }
    void Start()
    {
        isRunningLocal = false;

        OnGamePause += Callback_OnGamePause;
        OnGameResume += Callback_OnGameResume;
    }

    private void OnDestroy()
    {
        OnGamePause -= Callback_OnGamePause;
        OnGameResume -= Callback_OnGameResume;
    }

    #region Getters and Setters
    public void SetUserData(UserData _userData) => userData = _userData;
    public UserData GetUserData() => userData;
    public string GetUserUID() => userData.userDataServer.uid;

    public void SetDishData(DishData.Dish _dishData) => userData.dishData = _dishData;
    public void SetLeaderboardData(LeaderBoardItem _leaderboard) => userData.leaderBoard = _leaderboard;

    public void SetGameTime(float _time)
    {
        userData.leaderBoard.time = _time;
        userData.userDataServer.time = _time.ToString();
    }

    public void SetRoomDetails(RoomDetails roomDetails)
    {
        userData.userDataServer.roomId = roomDetails.ID;
        userData.userDataServer.roomName = roomDetails.Name;
    }


    public void ResetUserData()
    {
        userData.hasCompleted = false;
        userData.userDataServer.roomId = "";
        userData.userDataServer.roomName = "";
        userData.leaderBoard = null;
        userData.dishData = null;
        hasGameStarted = false;
        botsData.Clear();
        DeletePlayerData();
    }

    [ContextMenu("SavePlayerData")]
    public void SavePlayerData()
    {
        playerDataHandler.SaveData(userData);
    }

    [ContextMenu("GetPlayerData")]
    public string GetPlayerData()
    {
        string _data = playerDataHandler.GetData();
        savedData = string.IsNullOrWhiteSpace(_data) ? new UserData() : Newtonsoft.Json.JsonConvert.DeserializeObject<UserData>(_data);
        return _data;
    }

    [ContextMenu("DeletePlayerData")]
    public void DeletePlayerData() => playerDataHandler.DeleteData();

    public void AddBot(UserData _data) => botsData.Add(_data);
    #endregion


    public void UpdateRoomDetailsOnFirebase(bool _setEmpty = false)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

#elif UNITY_WEBGL
        if(useFirebase)
        {
            print("UpdateRoomDetailsOnFIREBASE " + _setEmpty);
            FirebaseDBLibrary.UpdateRoomID(GetUserData().userDataServer.uid, "roomID", _setEmpty ? "" : GetUserData().userDataServer.roomId.Trim(), gameObject.name, "OnSuccess_UpdateRoomID", "OnFailed_UpdateRoomID");
            FirebaseDBLibrary.UpdateRoomName(GetUserData().userDataServer.uid, "roomName", _setEmpty ? "" : GetUserData().userDataServer.roomName.Trim(), gameObject.name, "OnSuccess_UpdateRoomName", "OnFailed_UpdateRoomName");
        }
#endif
    }


    #region Local Game Stuff
    public IEnumerator StartLocalGame()
    {
        if (InstanceFinder.IsServer) yield break;

        isRunningLocal = true;
        userData.isLocal = true;


        // Load Scene
        SceneManager.LoadSceneAsync(gamePlaySceneName, LoadSceneMode.Additive);


        // Create Room
        RoomDetails _room;
        bool restoreGame = !string.IsNullOrWhiteSpace(savedData.userDataServer.uid);


        if (restoreGame)
        {
            savedData.isLocal = true;
            print("Restoring OLD GAME-----");
            SetUserData(savedData);
            _room = new RoomDetails()
            {
                ID = savedData.userDataServer.roomId,
                Name = savedData.userDataServer.roomName,
                sceneHandle = 1,
                MaxPlayers = 5,
                dishData = savedData.dishData
            };
        }
        else
        {
            print("Creating NEW GAME-----");
            _room = new RoomDetails()
            {
                ID = GetUnixTimeCode(),
                Name = "FarziCafe_LocalHost" + UnityEngine.Random.Range(1000, 10000).ToString(),
                sceneHandle = 1,
                MaxPlayers = 5,
                dishData = cluesManager.SelectRandomDish()
            };
        }


        // Player Setup
        PlayerSetup_Local(_room, restoreGame);


        // Bots Setup
        yield return StartCoroutine(BotsSetup_Local(_room, botPrefab));


        // Start Gameplay
        StartGamePlay_Local(_room, restoreGame);
    }



    private void PlayerSetup_Local(RoomDetails _room, bool _restoreGame)
    {
        SetRoomDetails(_room);
        SetDishData(_room.dishData);
        SetLeaderboardData(new LeaderBoardItem()
        {
            id = GetUserUID(),
            userName = userData.userDataServer.userName,
            rank = "1",
            dishName = userData.dishData.Dish_Name,
            isBot = false
        });
        cluesManager.Setup(_room.dishData, _restoreGame ? savedData.userDataServer.currentIngredientIndex : 0);

        SpawnPlayer_Local(playerLocalPrefab, ServerInstancing.Instance.playerPosition);
    }
    private GameObject SpawnPlayer_Local(GameObject _playerPrefab, Vector3 _defaultPosition)
    {
        //Vector3 _position = _defaultPosition;
        //float _multiplier = (connection.ClientId + 1) * 0.04f;
        //_position.x = _position.x + playerPosition.x * _multiplier;
        //_position.z = _position.z + playerPosition.z * _multiplier;
        //print("Player Spawned at Position - " + _position);
        Player _player = Instantiate(_playerPrefab, _defaultPosition, Quaternion.identity).GetComponent<Player>();
        _player.InitialisePlayer_Local();
        return _player.gameObject;

        //return _player.GetComponent<NetworkObject>();
    }

    private IEnumerator BotsSetup_Local(RoomDetails _room, GameObject _botPrefab)
    {
        for (int i = 0; i < 4; i++)
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

            SpawnBot_Local(_botPrefab, _botData);
            print(_botData.userDataServer.userName + "-Bot User Data added in Room-" + _room.Name);
            yield return new WaitForSeconds(0.25f);
        }
    }
    private GameObject SpawnBot_Local(GameObject _botPrefab, UserData _botData)
    {
        AI aI = Instantiate(_botPrefab).GetComponent<AI>();
        aI.GetComponent<Bot>().Init(_botData);
        AddBot(_botData);
        return aI.gameObject;
    }

    private void StartGamePlay_Local(RoomDetails _room, bool _restoreGame = false)
    {
        print("GamePlay Started LOCAL");
        PopUpManager.Instance.HideWaitingForOtherPlayers();
        ScreenManager.Instance.SwitchScreen(ScreenManager.Instance.menuScreen.gameObject, ScreenManager.Instance.gameplayScreen.gameObject);
        ScreenManager.Instance.gameplayScreen.StartGamePlay();
        hasGameStarted = true;
        int _startTime = string.IsNullOrWhiteSpace(savedData.userDataServer.time) ? 0 : int.Parse(savedData.userDataServer.time);
        timer.StartTimer(_restoreGame ? _startTime : 0, ServerInstancing.MAX_TIME, 1, _onUpdate: UpdateGamePlayTime, _onComplete: () => { print("Timer on LOCAL FINISHED for room -> " + _room.Name); });
    }

    public void UpdateGamePlayTime(float _timeGone, string _roomID)
    {
        ScreenManager.Instance.gameplayScreen.UpdateGamePlayTime(_timeGone);
        if (!GetUserData().hasCompleted)
            SetGameTime(_timeGone);
    }

    #endregion

    public void StopGamePlay()
    {
        if (useFirebase)
            UpdateRoomDetailsOnFirebase(true);
        ServerInstancing.Instance.UpdateUserTimeOnServerLeaderboard(GetUserUID(), GetUserData().userDataServer.roomId);
        ServerInstancing.Instance.GetLeaderboard(GetUserUID(), GetUserData().userDataServer.roomId);
        ServerInstancing.Instance.StopGame(GetUserData(), GetUserData().userDataServer.roomId, gamePlaySceneName);
        GetUserData().hasCompleted = true;
        ResetUserData();

        //InstanceFinder.ClientManager.StopConnection();
    }





    public static void SetCursorLockState(CursorLockMode _mode)
    {
        Cursor.lockState = _mode;
        switch (_mode)
        {
            case CursorLockMode.None:
                Cursor.visible = true;
                break;
            case CursorLockMode.Locked:
                Cursor.visible = false;
                break;
            default:
                break;
        }

    }

    private void Callback_OnGamePause()
    {
        print("OnGamePaused");
        isGameplayPaused = true;
        SetCursorLockState(CursorLockMode.None);

        if (character)
            character.enabled = false;
    }

    private void Callback_OnGameResume()
    {
        print("OnGameResume");
        isGameplayPaused = false;
        SetCursorLockState(CursorLockMode.Locked);

        if (character)
            character.enabled = true;
    }


    #region Events
    public static event Action OnGameStarts;
    public static void Event_OnGameStarts() => OnGameStarts?.Invoke();

    public static event Action OnGamePause;
    public static void Event_OnGamePause() => OnGamePause?.Invoke();

    public static event Action OnGameResume;
    public static void Event_OnGameResume() => OnGameResume?.Invoke();
    #endregion


    #region React Callbacks
    public void OnSuccess_UpdateRoomID(string _json)
    {
        print("OnSuccess_UpdateRoomID" + _json);
    }

    public void OnFailed_UpdateRoomID(string _json)
    {
        print("OnFailed_UpdateRoomID" + _json);
    }


    public void OnSuccess_UpdateRoomName(string _json)
    {
        print("OnSuccess_UpdateRoomName" + _json);
    }

    public void OnFailed_UpdateRoomName(string _json)
    {
        print("OnFailed_UpdateRoomName" + _json);
    }
    #endregion


    public static string GetUnixTimeCode() => DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
}
