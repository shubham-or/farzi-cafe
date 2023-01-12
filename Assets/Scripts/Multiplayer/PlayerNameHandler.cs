using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameHandler : MonoBehaviour
{
    [SerializeField] private PlayerNames playerNames;
    private PlayerNames playerNamesTaken;

    public const string SaveDirectory = "/Resources/";
    public static string FileName = "PlayerNames";

    public static PlayerNameHandler Instance;
    private void Awake() => Instance = this;

    void Start()
    {
        ReadPlayerNames();
    }

    public void ReadPlayerNames()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(FileName);
        playerNames = JsonConvert.DeserializeObject<PlayerNames>(textAsset.text);
    }


    public string GetRandomName() =>
        playerNames.Player_Names[Random.Range(0, playerNames.Player_Names.Count)];

}

[System.Serializable]
public class PlayerNames
{
    public List<string> Player_Names;
}

