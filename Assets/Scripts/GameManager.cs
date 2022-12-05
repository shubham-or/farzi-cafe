using FirstGearGames.LobbyAndWorld.Lobbies.JoinCreateRoomCanvases;
using FishNet;
using FishNet.Managing.Timing;
using FishNet.Object.Synchronizing;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string gamePlaySceneName = "GamePlay";

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
        print("UpdateRoomDetailsOnFIREBASE " + _setEmpty);
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

#elif UNITY_WEBGL
        FirebaseDBLibrary.UpdateRoomID(GetUserData().userDataServer.uid, "roomID", _setEmpty ? "" : GetUserData().userDataServer.roomId.Trim(), gameObject.name, "OnSuccess_UpdateRoomID", "OnFailed_UpdateRoomID");
        FirebaseDBLibrary.UpdateRoomName(GetUserData().userDataServer.uid, "roomName", _setEmpty ? "" : GetUserData().userDataServer.roomName.Trim(), gameObject.name, "OnSuccess_UpdateRoomName", "OnFailed_UpdateRoomName");
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


}
