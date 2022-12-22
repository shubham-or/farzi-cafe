using UnityEngine;
using Newtonsoft.Json;



public class JsonUtility_DishData : MonoBehaviour
{
    public const string SaveDirectory = "/Resources/";
    public static string FileName = "Dish_Data";


    //public static bool Save(DishData CurrentDishData)
    //{
    //    string dir = Application.persistentDataPath + SaveDirectory;

    //    if (!Directory.Exists(dir))
    //        Directory.CreateDirectory(dir);

    //    string json = JsonConvert.SerializeObject(CurrentDishData);
    //    File.WriteAllText(dir + FileName, json);


    //    return true;
    //}


    public static DishData Load()
    {
        DishData CurrentRoomData = new DishData();
        TextAsset textAsset = Resources.Load<TextAsset>(FileName);
        //print("textAsset : " + textAsset.text);
        //string json = File.ReadAllText(textAsset.text);
        CurrentRoomData = JsonConvert.DeserializeObject<DishData>(textAsset.text);
        return CurrentRoomData;
    }
}
