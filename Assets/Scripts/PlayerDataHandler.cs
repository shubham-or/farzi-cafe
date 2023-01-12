using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerDataHandler
{
    public string progressKey = "progress_data";
    public string filePath = "";
    public string extension = ".json";


    public void SetFilePath(string _fileName)
    {
        filePath = Path.Combine(Application.persistentDataPath, _fileName) + extension;
        Debug.Log("SAVED PATH -> " + filePath);
    }

    public void SaveData<T>(T _data)
    {
        string _dataString = Newtonsoft.Json.JsonConvert.SerializeObject(_data);
        File.WriteAllText(filePath, _dataString);

        //PlayerPrefs.SetString(_keyString, _dataString);
        //PlayerPrefs.Save();

        Debug.Log("Player Data Saved @-> " + filePath);
    }



    public string GetData()
    {
        string _dataString = "";

        if (File.Exists(filePath))
        {
            _dataString = File.ReadAllText(filePath);
            Debug.Log($"Player Data Found @-> {filePath} - {_dataString}");
        }

        //if (PlayerPrefs.HasKey(_keyString))
        //    _dataString = PlayerPrefs.GetString(_keyString);

        return _dataString;
    }

    public void DeleteData()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"Player Data DELETED @-> {filePath}");
        }
    }


}
