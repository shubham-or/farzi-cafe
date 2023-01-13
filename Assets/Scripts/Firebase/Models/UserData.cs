using System;
using UnityEngine;

[Serializable]
public class UserData
{
    [Header("---BasicDetails---")]
    public bool isBot = false;
    public bool hasCompleted = false;
    public bool isLocal = false;

    [Space(10)]
    [Header("---UserData---")]
    public UserDataServer userDataServer = new UserDataServer();

    [Space(10)]
    [Header("---DishData---")]
    public DishData.Dish dishData = new DishData.Dish();

    [Space(10)]
    [Header("---LeaderboardData---")]
    public LeaderBoardRecord leaderBoard = new LeaderBoardRecord();


    public UserData CreateBotData()
    {
        userDataServer.uid = GameManager.GetUnixTimeCode();
        userDataServer.actualName = "Bot_" + userDataServer.uid;
        userDataServer.userName = PlayerNameHandler.Instance.GetRandomName();
        userDataServer.signInMethod = "";
        userDataServer.email = "";
        userDataServer.picture = "";
        isBot = true;
        return this;
    }

    public void SetDishData(DishData.Dish _dishData) => dishData = _dishData;
    public void SetLeaderboardData(LeaderBoardRecord _leaderboard) => leaderBoard = _leaderboard;

    public void SetRoomDetails(RoomDetails roomDetails)
    {
        userDataServer.roomId = roomDetails.ID;
        userDataServer.roomName = roomDetails.Name;
    }

}


[Serializable]
public class LeaderBoardRecord
{
    public string id;
    public string rank;
    public string userName;
    public string dishName;
    public float time;
    public bool isBot = false;
}


[Serializable]
public class UserDataServer
{
    public string uid;
    public string actualName;
    public string userName;
    public string email;
    public string signInMethod;
    public string picture;
    public string score; // Rank
    public string time;
    public int currentIngredientIndex;
    public string lastUpdated = DateTime.Now.ToString();

    public string roomId;
    public string roomName;
}

