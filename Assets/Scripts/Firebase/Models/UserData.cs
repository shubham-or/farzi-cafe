using FirstGearGames.LobbyAndWorld.Lobbies.JoinCreateRoomCanvases;
using System;

[System.Serializable]
public class UserData
{
    public bool isBot = false;
    public bool hasCompleted = false;
    public UserDataServer userDataServer = new UserDataServer();
    public DishData.Dish dishData = new DishData.Dish();
    public LeaderBoardItem leaderBoard = new LeaderBoardItem();

    public UserData CreateBotData()
    {
        userDataServer.uid = Guid.NewGuid().ToString();
        userDataServer.actualName = "Player_" + userDataServer.uid;
        userDataServer.userName = " Player_" + UnityEngine.Random.Range(99, 999999);
        userDataServer.signInMethod = "";
        userDataServer.email = "";
        userDataServer.picture = "";
        isBot = true;
        return this;
    }

    public void SetDishData(DishData.Dish _dishData) => dishData = _dishData;
    public void SetLeaderboardData(LeaderBoardItem _leaderboard) => leaderBoard = _leaderboard;
    public void SetClientId(string _clientId) => userDataServer.clientId = _clientId;

    public void SetRoomDetails(RoomDetails roomDetails)
    {
        userDataServer.roomId = roomDetails.ID;
        userDataServer.roomName = roomDetails.Name;
    }



}

[System.Serializable]
public class UserDataServer
{
    public string uid;
    public string actualName;
    public string userName;
    public string email;
    public string signInMethod;
    public string picture;
    public string score = ""; // Rank
    public string time = "0";
    public string lastUpdated = DateTime.Now.ToString();

    public string roomId;
    public string roomName;
    public string clientId;
}

