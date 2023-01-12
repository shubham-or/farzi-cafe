using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class CluesManager : MonoBehaviour
{
    [SerializeField] public TimerDisplay m_Timer;


    public List<Clue> ingredients = new List<Clue>();
    public List<CluePoint> chosenCluePoints = new List<CluePoint>();
    public List<CluePoint> cluePoints = new List<CluePoint>();



    public GameObject[] dishes;


    public TMP_Text testTxt;
    public TMP_Text textCluePosName;

    public DishData dish_Data;

    public DishData.Dish selectedDish;
    public Clue currentClue;
    [SerializeField] private int currentClueIndex = -1;

    private void Start()
    {
        dish_Data = JsonUtility_DishData.Load();
        print(dish_Data);
    }


    [ContextMenu("Init")]
    private void Init()
    {
        ResetData();
        print("SelectRandomDish");
        SelectRandomDish();
        Generate_Clues();
        Set_RandomClues();
        AssignNextClue();
    }

    public void Setup(DishData.Dish _dish, int initialClueIndex = 0)
    {
        print("ClueManager Setup for - " + GameManager.Instance.GetUserData().userDataServer.userName + "| Initial Index - " + initialClueIndex);

        ResetData();
        selectedDish = _dish;
        Generate_Clues();
        Set_RandomClues();

        // restore old game
        for (int i = initialClueIndex; i >= 0; i--)
            AssignNextClue();

        //if (initialClueIndex > 0)
        //    currentClueIndex = GameManager.Instance.GetUserData().userDataServer.currentIngredientIndex = currentClueIndex + 1;
    }

    private void ResetData()
    {
        print("ResetData");
        currentClueIndex = -1;
        currentClue = null;
        selectedDish = null;
        ingredients.Clear();
        chosenCluePoints.Clear();
    }


    #region Dish
    //public DishData.Dish SelectRandomDish() => selectedDish = dish_Data.Dish_Data[Random.Range(0, dish_Data.Dish_Data.Count)];
    public DishData.Dish SelectRandomDish() => selectedDish = dish_Data.Dish_Data.First(x => x.Dish_Name == "Sweet Potato Chaat");

    public GameObject GetCurrentDish()
    {
        if (dishes.Length == 0)
            dishes = GameplayScene.Instance.dishes;

        return dishes.First(x => x.name == selectedDish.Dish_Name);
    }

    public void OnDishClaimed()
    {
        PopUpManager.OnDishFound(selectedDish.Dish_Name, Resources.Load<Texture2D>($"DishImages/{selectedDish.Dish_Name}"), GameManager.Instance.GetUserData().leaderBoard.time);
        gameObject.SetActive(false);
    }
    #endregion


    #region Clues
    public void Generate_Clues()
    {
        print("Generate_Clues");
        int ingredientCount = selectedDish.Ingredients.Count;
        for (int i = 0; i < ingredientCount; i++)
        {
            Clue c = new Clue();
            c.id = i;
            c.ingredient = selectedDish.Ingredients[i].name;
            //c.clue = cluePoints.Where(x => x.gameObject.name == )
            //c.hidingSpot = selectedDish.Ingredients[i].hint;

            string _enabledPath = $"{selectedDish.Dish_Name }/{selectedDish.Ingredients[i].name}";
            //print(_enabledPath);
            c.enabledTexture = Resources.Load<Texture2D>(_enabledPath);

            string _disabledPath = $"{selectedDish.Dish_Name }/B_W/{selectedDish.Ingredients[i].name}";
            c.disabledTexture = Resources.Load<Texture2D>(_disabledPath);

            ingredients.Add(c);
            ScreenManager.Instance.gameplayScreen.SetIngredient(c);
        }
    }


    public void Set_RandomClues()
    {
        print("Set_RandomClues");
        List<CluePoint> tempPoints = new List<CluePoint>(cluePoints);

        for (int i = 0; i < ingredients.Count; i++)
        {
            int randomPoint = Random.Range(0, tempPoints.Count);

            //Instantiate(ing, tempPoints[randomPoint].transform); //to instantiate prafabfs (ing is prefab)

            CluePoint cp = (tempPoints[randomPoint]);
            tempPoints.RemoveAt(randomPoint);

            cp.Set_Clue(ingredients[i]);
            ingredients[i].clue = dish_Data.Clues.First(x => x.object_name.Trim().ToLower() == cp.name.Trim().ToLower()).clue;
            ingredients[i].hidingSpot = dish_Data.HidingSpots.First(x => x.object_name.Trim().ToLower() == cp.name.Trim().ToLower()).hidingspot;
            chosenCluePoints.Add(cp);
        }
    }


    [ContextMenu("AssignNextClue")]
    public void AssignNextClue()
    {
        //if (ingredients.Count == 0) return;
        print("AssignNextClue");

        if (currentClueIndex >= 0)
            chosenCluePoints[currentClueIndex].gameObject.SetActive(false);

        currentClueIndex++;

        if (currentClueIndex == ingredients.Count)
        {
            print("Found all Ingredients... And claim dish");
            GetCurrentDish().SetActive(true);
            ScreenManager.Instance.gameplayScreen.ShowDishClaim();
            return;
        }

        currentClue = ingredients[currentClueIndex];
        //testTxt.text = chosenCluePoints[currentClueIndex].hint.ToString(); //to show hint for a particular clue     
        chosenCluePoints[currentClueIndex].gameObject.SetActive(true);

        //Clue position name
        textCluePosName.text = chosenCluePoints[currentClueIndex].gameObject.name;
        ScreenManager.Instance.gameplayScreen.ActivateNextItem(currentClue, currentClueIndex - 1 >= 0 ? ingredients[currentClueIndex - 1] : null);

        GameManager.Instance.GetUserData().userDataServer.currentIngredientIndex = currentClueIndex;
        GameManager.Instance.SavePlayerData();
    }
    #endregion




}

[System.Serializable]
public class Clue
{
    public string ingredient;
    public string clue;
    public string hidingSpot;
    public int id;
    public Texture2D enabledTexture;
    public Texture2D disabledTexture;
}
