using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataHandler : NetworkBehaviour
{
    public string progressKey = "progress_data";
    public string data;

    void Start()
    {

    }


    public void SaveData(UserData _data, string _key = null)
    {
        data = Newtonsoft.Json.JsonConvert.SerializeObject(_data);
        PlayerPrefs.SetString(string.IsNullOrWhiteSpace(_key) ? progressKey : _key, data);
        PlayerPrefs.Save();
    }


    [ContextMenu("GetData")]
    public void GetData(string _key = null)
    {
        data = PlayerPrefs.GetString(string.IsNullOrWhiteSpace(_key) ? progressKey : _key);
    }


    [ContextMenu("DeleteData")]
    public void DeleteData() => PlayerPrefs.DeleteAll();


}
