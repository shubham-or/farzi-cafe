using FirstGearGames.LobbyAndWorld.Lobbies.JoinCreateRoomCanvases;
using FishNet.Managing.Timing;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string gamePlaySceneName = "GamePlay";
    public bool useFirebase = false;
    public bool isGameplayPaused = false;

    [Header("-----References-----")]
    public TimeManager timeManager;
    public UI_Handler uIHandler;
    public Timer timer;
    public CluesManager cluesManager;
    public GameObject leftdoor, rightdoor;

    [Header("-----User Data-----")]
    [SerializeField] private UserData userData;
    [SerializeField] private List<UserData> botsData = new List<UserData>();

    public bool hasGameStarted = false;

    public Player_Controller playerController;
    public Player_Interaction playerInteraction;

    public static GameManager Instance;
    private void Awake() => Instance = this;

    void Start()
    {
        OnGamePause += Callback_OnGamePause;
        OnGameResume += Callback_OnGameResume;
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

    public void SetClientId(string _clientId) => userData.userDataServer.clientId = _clientId;
    public string GetClientId() => userData.userDataServer.clientId;

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


    public void StopGamePlay()
    {
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


}
