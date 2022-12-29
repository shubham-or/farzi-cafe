using FishNet.Managing.Timing;
using System;
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


    [Header("-----References-----")]
    public TimeManager timeManager;
    public UI_Handler uIHandler;
    public Timer timer;
    public CluesManager cluesManager;
    public GameObject leftdoor, rightdoor;
    public Player player;
    public Player_Interaction playerInteraction;

    [Header("-----User Data-----")]
    [SerializeField] private UserData userData;
    [Space(10)]
    [SerializeField] private List<UserData> botsData = new List<UserData>();

    private PlayerDataHandler playerDataHandler;

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
        playerDataHandler = GetComponent<PlayerDataHandler>();
    }
    void Start()
    {
        OnGamePause += Callback_OnGamePause;
        OnGameResume += Callback_OnGameResume;
    }


    private void Update()
    {

    }


    [ContextMenu("SaveData")]
    public void SaveData()
    {
        playerDataHandler.SaveData(userData);
    }

    private void OnDestroy()
    {
        OnGamePause -= Callback_OnGamePause;
        OnGameResume -= Callback_OnGameResume;
    }


    public void SetUserData(UserData _userData) => userData = _userData;
    public UserData GetUserData() => userData;
    public string GetUID() => userData.userDataServer.uid;

    public void SetDishData(DishData.Dish _dishData) => userData.dishData = _dishData;
    public void SetLeaderboardData(LeaderBoardItem _leaderboard) => userData.leaderBoard = _leaderboard;

    public void SetRoomDetails(RoomDetails roomDetails)
    {
        userData.userDataServer.roomId = roomDetails.ID;
        userData.userDataServer.roomName = roomDetails.Name;
    }


    public void AddBot(UserData _data)
    {
        botsData.Add(_data);
    }

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
    public void StartLocalGame()
    {
        // Load Scene
        SceneManager.LoadSceneAsync(gamePlaySceneName);


        // Set Data
        RoomDetails _room = new RoomDetails()
        {
            ID = GetUnitueID(),
            Name = "FarziCafe_LocalHost" + UnityEngine.Random.Range(1000, 10000).ToString(),
            sceneHandle = 1,
            MaxPlayers = 5,
            dishData = cluesManager.SelectRandomDish()
        };
        SetRoomDetails(_room);
        SetDishData(_room.dishData);
        SetLeaderboardData(new LeaderBoardItem()
        {
            id = userData.userDataServer.uid,
            userName = userData.userDataServer.userName,
            rank = "1",
            dishName = userData.dishData.Dish_Name,
            isBot = false
        });
        cluesManager.Setup(_room.dishData);


        //Instantiate Player
        SpawnPlayer(ServerInstancing.Instance.playerNetworkPrefab, ServerInstancing.Instance.playerPosition);


        // Instantiate Bots
        //int botsCount = _room.MaxPlayers - _room.userData.Count;
        //for (int i = 0; i < botsCount; i++)
        //{
        //    UserData _botData = new UserData().CreateBotData();
        //    LeaderBoardItem _leaderBoardBot = new LeaderBoardItem()
        //    {
        //        id = _botData.userDataServer.uid,
        //        userName = _botData.userDataServer.userName,
        //        rank = "5",
        //        dishName = _room.dishData.Dish_Name,
        //        isBot = true
        //    };

        //    // Set Details For User
        //    _botData.SetRoomDetails(_room);
        //    _botData.SetDishData(_room.dishData);
        //    _botData.SetLeaderboardData(_leaderBoardBot);


        //    // Add Details to Room
        //    _room.userData.Add(_botData.userDataServer.uid, _botData);
        //    _room.userData[_botData.userDataServer.uid].leaderBoard = _leaderBoardBot;
        //    print(_botData.userDataServer.userName + "-Bot User Data added in Room-" + _room.Name);
        //}

        //ClientServerManager.Instance.GenerateBot(_conn.Value, _user.Value);


        // Start Gameplay
        //ClientServerManager.Instance.StartGamePlay(item.Value);

    }

    private GameObject SpawnPlayer(GameObject _playerPrefab, Vector3 _defaultPosition)
    {
        //Vector3 _position = _defaultPosition;
        //float _multiplier = (connection.ClientId + 1) * 0.04f;
        //_position.x = _position.x + playerPosition.x * _multiplier;
        //_position.z = _position.z + playerPosition.z * _multiplier;
        //print("Player Spawned at Position - " + _position);

        return Instantiate(_playerPrefab, _defaultPosition, Quaternion.identity);

        //return _player.GetComponent<NetworkObject>();
    }
    #endregion

    public void StopGamePlay()
    {
        if (GameManager.Instance.useFirebase)
            UpdateRoomDetailsOnFirebase(true);
        ServerInstancing.Instance.UpdateUserTimeOnServerLeaderboard(GetUserData().userDataServer.uid, GetUserData().userDataServer.roomId);
        ServerInstancing.Instance.GetLeaderboard(GetUserData().userDataServer.uid, GetUserData().userDataServer.roomId);
        ServerInstancing.Instance.StopGame(GetUserData(), GetUserData().userDataServer.roomId, gamePlaySceneName);
        GetUserData().hasCompleted = true;

        //InstanceFinder.ClientManager.StopConnection();
    }


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
    }

    private void Callback_OnGameResume()
    {
        print("OnGameResume");
        isGameplayPaused = false;
        SetCursorLockState(CursorLockMode.Locked);
    }


    public static event Action OnGameStarts;
    public static void Event_OnGameStarts() => OnGameStarts?.Invoke();

    public static event Action OnGamePause;
    public static void Event_OnGamePause() => OnGamePause?.Invoke();

    public static event Action OnGameResume;
    public static void Event_OnGameResume() => OnGameResume?.Invoke();

    public static string GetUnitueID() => DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
}
