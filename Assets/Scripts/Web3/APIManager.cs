using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WinnerData
{
    public string nftId;
    public string winnerAddress;
    public string winnerEmail;
    public float time;
}

public class APIManager : MonoBehaviour
{
    public static APIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void AssignWinner(WinnerData winnerData)
    {
        StartCoroutine(Post_AssignWinner(winnerData, data => {
            Debug.Log(data);
        }));
    }

    IEnumerator Post_AssignWinner(WinnerData winnerData, Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("nftId", winnerData.nftId);
        form.AddField("winnerAddress", winnerData.winnerAddress);
        form.AddField("winnerEmail", winnerData.winnerEmail);
        form.AddField("time", winnerData.time.ToString());


        using (UnityWebRequest www = UnityWebRequest.Post("https://restaurant-backend.onerare.io/assignWinner", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                callback(www.downloadHandler.text);
            }
        }
    }
}
