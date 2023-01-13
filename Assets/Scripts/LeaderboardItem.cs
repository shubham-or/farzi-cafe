using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardItem : MonoBehaviour
{
    APIDataClasses.WinnersResponse.Winner leaderboard;
    public Color ownerColor;

    [Header("---Image---")]
    [SerializeField] private Image rankImage;
    [SerializeField] private Image playerNameImage;
    [SerializeField] private Image dishNameImage;
    [SerializeField] private Image timeImage;

    [Header("---Text---")]
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI dishNameText;
    [SerializeField] private TextMeshProUGUI timeText;




    private static int rank;
    void Start()
    {

    }


    public void SetDetails(APIDataClasses.WinnersResponse.Winner _data, int _rank, bool isOwner = false)
    {
        leaderboard = _data;
        rankText.text = (_rank).ToString();
        playerNameText.text = isOwner ? "You" : _data.userAddress;
        dishNameText.text = GameManager.ReplaceUnderscoreWithSpace(_data.dishName);
        timeText.text = Timer.GetTimeInMinAndSec(_data.bestTime);

        if (isOwner) rankImage.color = ownerColor;
        playerNameImage.color = dishNameImage.color = timeImage.color = isOwner ? ownerColor : Color.white;
    }
}
