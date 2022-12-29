using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardScreen : MonoBehaviour
{
    [SerializeField] private GameObject records;
    [SerializeField] private Sprite yellow;
    [SerializeField] private Sprite white;
    [SerializeField] private Sprite purple;

    void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        foreach (Transform item in records.transform)
            item.gameObject.SetActive(false);

        ServerInstancing.Instance.GetLeaderboard(GameManager.Instance.GetUserData().userDataServer.uid, GameManager.Instance.GetUserData().userDataServer.roomId);
    }

    public void SetLeaderboard(Dictionary<string, LeaderBoardItem> _leaderBoard)
    {
        print("Set leaderBoard Target Local COUNT - " + _leaderBoard.Count);
        //Dictionary<string, LeaderBoardItem> _leaderBoardSorted = _leaderBoard.OrderBy(x => x.Value.time).ToDictionary(x => x.Key, x => x.Value);

        int _rank = 0;
        foreach (var item in _leaderBoard)
        {
            print(Newtonsoft.Json.JsonConvert.SerializeObject(item.Value));
            item.Value.rank = $"{_rank + 1}";
            records.transform.GetChild(_rank).name = item.Key == GameManager.Instance.GetUserData().userDataServer.uid ? "You" : item.Value.userName;

            Image[] images = records.transform.GetChild(_rank).GetComponentsInChildren<Image>();
            for (int j = 0; j < images.Length; j++)
                images[j].sprite = _rank == 0 ? purple : j == 0 ? yellow : white;

            // Rank
            records.transform.GetChild(_rank).GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = item.Value.rank;
            // Player Name
            records.transform.GetChild(_rank).GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = item.Key == GameManager.Instance.GetUserData().userDataServer.uid ? "You" : item.Value.userName;
            // Dish Name
            records.transform.GetChild(_rank).GetChild(2).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = item.Value.dishName;
            // Time
            records.transform.GetChild(_rank).GetChild(3).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = Timer.GetTimeInMinAndSec(item.Value.time);

            records.transform.GetChild(_rank).gameObject.SetActive(true);
            _rank++;
        }
    }




    public void OnClick_Close(GameObject _go)
    {
        _go.SetActive(true);
        gameObject.SetActive(false);
    }




}
