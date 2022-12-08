using UnityEngine;
using UnityEngine.UI;

public class GameplayScreen : MonoBehaviour
{
    [SerializeField] private GameObject ingredientsGo;
    [SerializeField] private GameObject clueGo;
    [SerializeField] private TMPro.TextMeshProUGUI time;

    [Space(20)]
    [SerializeField] private TMPro.TextMeshProUGUI hint;
    [SerializeField] private TMPro.TextMeshProUGUI clue;
    [SerializeField] private RawImage[] ingredients;
    [SerializeField] private int freeRoamTime = 10;

    [Header("-----Debug-----")]
    [SerializeField] private int currentClueIndex;
    [SerializeField] private string currentHintInfo;


    #region Getters-Setters
    public void SetCurrentClueIndex(int _value) => currentClueIndex = _value;
    public int GetCurrentClueIndex() => currentClueIndex;

    public void SetCurrentClueHint(string _value) => currentHintInfo = _value;
    public string GetCurrentClueHint() => currentHintInfo;
    #endregion


    void Start()
    {
        //Init();
    }

    [ContextMenu("Init")]
    public void Init()
    {
        ingredientsGo.SetActive(false);
        clueGo.SetActive(false);

        currentClueIndex = 0;
        clue.text = $"Clue {currentClueIndex + 1}";
        hint.text = $"Clue {currentClueIndex + 1}: Hint goes here";
    }


    
    public void StartGamePlay()
    {
        print("Start main GamePlay");
        ingredientsGo.SetActive(true);
        clueGo.SetActive(true);

        PopUpManager.Instance.ShowPopup_DishOfTheDay(GameManager.Instance.cluesManager.selectedDish);
    }



    public void SetIngredient(Clue _clue)
    {
        //print("SetIngredient - " + _clue.ingredient);
        if (_clue != null)
        {
            ingredients[_clue.id].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = _clue.ingredient;
            ingredients[_clue.id].GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = false;
            ingredients[_clue.id].texture = ingredients[_clue.id].transform.GetSiblingIndex() == 0 ? _clue.enabledTexture : _clue.disabledTexture;
            ingredients[_clue.id].gameObject.SetActive(true);
        }
        else print("CLUE IS NULL");
    }

    public void ActivateNextItem(Clue _next, Clue _prev)
    {
        //print("ActivateNextItem - " + _clue.id);1
        //if (_prev != null)
        //    ingredients[currentClueIndex].texture = _prev.disabledTexture;

        currentClueIndex = _next.id;
        if (currentClueIndex < ingredients.Length)
        {
            clue.text = $"Clue {currentClueIndex + 1}";
            hint.text = _next.clue;
            print($"Clue {currentClueIndex + 1} is {_next.clue}");
            ingredients[currentClueIndex].texture = _next.enabledTexture;
        }
    }

    public void UpdateGamePlayTime(float _time)
    {
        time.text = Timer.GetTimeInMinAndSec(_time);
    }

}
