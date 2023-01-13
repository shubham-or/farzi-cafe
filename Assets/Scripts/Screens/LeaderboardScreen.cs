using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardScreen : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;

    [SerializeField] private Transform records;
    [SerializeField] private Sprite yellow;
    [SerializeField] private Sprite white;
    [SerializeField] private Sprite purple;


    private void Start()
    {
        StartCoroutine(GetTopWinners());
    }

    void OnEnable()
    {
        //Init();
    }


    private IEnumerator GetTopWinners()
    {
        yield return UnityWebRequestHandler.GetTopWinnersRequest(GameManager.restaurantName, _response =>
        {
            APIDataClasses.WinnersResponse _data = Newtonsoft.Json.JsonConvert.DeserializeObject<APIDataClasses.WinnersResponse>(_response);
            if (_data.data.winners.Count > 0 && UnityWebRequestHandler.IsSuccess(_data.status))
            {
                bool _isOwnerInTop5 = false;
                int _rank = 1;
                for (int i = 0; i < 5 && i < _data.data.winners.Count; i++)
                {
                    if (_data.data.winners[i].userAddress == GameManager.walletAddress)
                        _isOwnerInTop5 = true;
                    Instantiate(itemPrefab, records).GetComponent<LeaderboardItem>().SetDetails(_data.data.winners[i], _rank++, _data.data.winners[i].userAddress == GameManager.walletAddress);
                }

                if (!_isOwnerInTop5)
                {
                    APIDataClasses.WinnersResponse.Winner owner = _data.data.winners.Find(x => x.userAddress == GameManager.walletAddress);
                    if (owner != null)
                        Instantiate(itemPrefab, records).GetComponent<LeaderboardItem>().SetDetails(owner, _data.data.winners.IndexOf(owner), true);
                }

            }
        });
    }

    public void Init()
    {
        foreach (Transform item in records.transform)
            item.gameObject.SetActive(false);

        ServerInstancing.Instance.GetLeaderboard(GameManager.Instance.GetUserData().userDataServer.uid, GameManager.Instance.GetUserData().userDataServer.roomId);
    }

    public void SetLeaderboard(Dictionary<string, LeaderBoardRecord> _leaderBoard)
    {
        print("Set leaderBoard Target Local COUNT - " + _leaderBoard.Count);

        int _rank = 0;
        foreach (var item in _leaderBoard)
        {
            print(Newtonsoft.Json.JsonConvert.SerializeObject(item.Value));
            item.Value.rank = $"{_rank + 1}";
            records.transform.GetChild(_rank).name = item.Key == GameManager.Instance.GetUserUID() ? "You" : item.Value.userName;

            Image[] images = records.transform.GetChild(_rank).GetComponentsInChildren<Image>();
            for (int j = 0; j < images.Length; j++)
                images[j].sprite = _rank == 0 ? purple : j == 0 ? yellow : white;

            // Rank
            records.transform.GetChild(_rank).GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = item.Value.rank;
            // Player Name
            records.transform.GetChild(_rank).GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = item.Key == GameManager.Instance.GetUserUID() ? "You" : item.Value.userName;
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
