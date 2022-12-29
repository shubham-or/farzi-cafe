using FishNet;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    [Header("-----Pop Ups-----")]
    [SerializeField] private GameObject bgs;
    [SerializeField] private GameObject facts;
    [SerializeField] private GameObject farzified;
    [SerializeField] private GameObject foundItem;
    [SerializeField] private GameObject dishOfTheDay;
    [SerializeField] private GameObject readyToServe;
    [SerializeField] private GameObject oopsTimeout;
    [SerializeField] private GameObject oopsSessionExpired;
    [SerializeField] private GameObject oopsNetworkFailed;
    [SerializeField] private GameObject waitingForOtherPlayers;

    [Header("-----FarziFied-----")]
    [SerializeField] private TextMeshProUGUI hh;
    [SerializeField] private TextMeshProUGUI mm;
    [SerializeField] private TextMeshProUGUI ss;
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private GameObject newBest;

    [Header("-----Found Item-----")]
    [SerializeField] private RawImage item;
    [SerializeField] private TextMeshProUGUI itemTitle;

    [Header("-----Dish Of The Day-----")]
    [SerializeField] private TextMeshProUGUI dishName;
    [SerializeField] private RawImage dish;

    [Header("-----ReadyToServe-----")]
    [SerializeField] private TextMeshProUGUI dishNameReadyToServe;
    [SerializeField] private RawImage dishReadyToServe;

    [Header("-----Other-----")]
    [SerializeField] private TextMeshProUGUI waitingForPlayersTimer;
    [SerializeField] private CluesManager cluesManager;
    [SerializeField] private TimerDisplay timerDisplay;
    [SerializeField] private GameObject crossWaitingPopup;

    [Space(20)]
    [SerializeField] private Image bgImage;

    public static PopUpManager Instance;
    private void Awake()
    {
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Init();
    }

    void OnEnable()
    {
        Event_OnIngredientFound += ShowPopup_OnItemFound;
        Event_OnDishFound += OnDishFoundCallback;
    }

    private void OnDisable()
    {
        //Init();
        Event_OnIngredientFound -= ShowPopup_OnItemFound;
        Event_OnDishFound -= OnDishFoundCallback;
    }

    private void Init()
    {
        facts.SetActive(false);
        farzified.SetActive(false);
        foundItem.SetActive(false);
        dishOfTheDay.SetActive(false);
        readyToServe.SetActive(false);
        oopsTimeout.gameObject.SetActive(false);
        oopsSessionExpired.gameObject.SetActive(false);
        oopsNetworkFailed.gameObject.SetActive(false);
        waitingForOtherPlayers.gameObject.SetActive(false);
        newBest.SetActive(false);
    }

    public void OnClick_PlayAgain()
    {
        bgImage.enabled = false;
        bgs.SetActive(false);
        hh.text = "00";
        mm.text = "00";
        ss.text = "00";
        farzified.SetActive(false);
        ScreenManager.Instance.SwitchScreen(null, ScreenManager.Instance.menuScreen.gameObject);
    }

    public void ShowPopup_Farzified(string _dishName, Texture2D _dishTexture, float _timer)
    {
        GameManager.Event_OnGamePause();
        bgImage.enabled = true;
        bgs.SetActive(true);
        hh.text = "00";
        mm.text = Timer.GetMinutes(_timer).ToString();
        ss.text = Timer.GetSeconds(_timer).ToString();
        GameManager.Instance.GetUserData().leaderBoard.time = _timer;
        GameManager.Instance.StopGamePlay();
        farzified.SetActive(true);
    }


    #region Item Found
    bool isReadyToServe = false;
    private void ShowPopup_OnItemFound(Clue _clue)
    {
        print("Clue Found ID - " + _clue.id);
        GameManager.Event_OnGamePause();
        isReadyToServe = _clue.id == 9 ? true : false;
        itemTitle.text = $"You Found\n{_clue.ingredient}";
        item.texture = _clue.enabledTexture;
        bgImage.enabled = false;
        bgs.SetActive(true);
        foundItem.SetActive(true);
        //ScreenManager.Instance.settingsIcon.SetActive(true);

        Invoke("HideItemFound", 3);
    }

    private void HideItemFound()
    {
        foundItem.SetActive(false);
        bgImage.enabled = false;
        bgs.gameObject.SetActive(false);
        //ScreenManager.Instance.settingsIcon.SetActive(false);

        GameManager.Event_OnGameResume();

        if (isReadyToServe)
            ShowPopup_ReadyToServe(GameManager.Instance.GetUserData().dishData.Dish_Name, Resources.Load<Texture2D>($"DishImages/{GameManager.Instance.GetUserData().dishData.Dish_Name}"));
    }
    #endregion


    #region Dish Found
    private void OnDishFoundCallback(string _dishName, Texture2D _dishTexture, float _timer)
    {
        StartCoroutine(Co_OnDishFound(_dishName, _dishTexture, _timer));
    }

    private IEnumerator Co_OnDishFound(string _dishName, Texture2D _dishTexture, float _timer)
    {
        //ScreenManager.Instance.settingsIcon.SetActive(false);
        ShowPopup_ReadyToServe(_dishName, _dishTexture);
        //GameManager.Instance.playerController.enabled = false;
        //GameManager.Instance.playerInteraction.enabled = false;
        yield return new WaitForSeconds(3);
        HideReadyToServe();
        yield return new WaitForEndOfFrame();
        ShowPopup_Farzified(_dishName, _dishTexture, _timer);
    }
    #endregion


    #region Ready To Server
    public void ShowPopup_ReadyToServe(string _dishName, Texture2D _dishTexture)
    {
        GameManager.Event_OnGamePause();
        bgs.SetActive(true);
        bgImage.enabled = false;
        dishReadyToServe.texture = _dishTexture;
        dishNameReadyToServe.text = _dishName;
        readyToServe.SetActive(true);

        Invoke("HideReadyToServe", 4);
    }

    public void HideReadyToServe()
    {
        bgs.SetActive(false);
        readyToServe.SetActive(false);
        GameManager.Event_OnGameResume();
    }
    #endregion


    #region Dish of the Day
    public void ShowPopup_DishOfTheDay(DishData.Dish _dish)
    {
        GameManager.Event_OnGamePause();
        //ScreenManager.Instance.settingsIcon.SetActive(false);
        bgs.SetActive(true);
        bgImage.enabled = false;
        dishName.text = _dish.Dish_Name;
        dishOfTheDay.SetActive(true);
        Invoke("HideDishOfTheDay", 4);
    }

    public void HideDishOfTheDay()
    {
        bgs.SetActive(false);
        dishOfTheDay.SetActive(false);
        //ScreenManager.Instance.settingsIcon.SetActive(true);
        GameManager.Event_OnGameResume();
    }
    #endregion


    #region Waiting for players
    public void ShowPopup_WaitingForOtherPlayers()
    {
        bgImage.enabled = true;
        bgs.SetActive(true);
        waitingForOtherPlayers.SetActive(true);
        //ScreenManager.Instance.settingsIcon.SetActive(false);

        if (InstanceFinder.ClientManager.Started) //&& InstanceFinder.ServerManager.Started
        {
            crossWaitingPopup.SetActive(false);
            ServerInstancing.Instance.QuickRaceConnect(GameManager.Instance.GetUserData(), InstanceFinder.ClientManager.Connection);
        }
        else
        {

            ConnectionManager.Instance.StartLocalHost();
            StartCoroutine(StartLocalGame());
            //crossWaitingPopup.SetActive(true);
            //waitingForPlayersTimer.text = "Couldn't establish a connection!\nPlease try again.";
            //InstanceFinder.ClientManager.StartConnection();
        }
    }

    int timeout = 10;

    private IEnumerator StartLocalGame()
    {
        timeout--;
        if (timeout < 0)
            yield break;

        yield return new WaitForSeconds(2);
        if (InstanceFinder.ClientManager.Started && InstanceFinder.ServerManager.Started)
            ServerInstancing.Instance.QuickRaceConnect(GameManager.Instance.GetUserData(), InstanceFinder.ClientManager.Connection);
        else
            StartCoroutine(StartLocalGame());
    }


    public void UpdateWaitingTime(float _timeLeft)
    {
        waitingForPlayersTimer.fontSize = 20;
        waitingForPlayersTimer.text = _timeLeft > 0 ? _timeLeft.ToString() : "Get Ready";
    }

    public void HideWaitingForOtherPlayers()
    {
        bgs.SetActive(false);
        bgImage.enabled = false;
        waitingForOtherPlayers.SetActive(false);
        //ScreenManager.Instance.settingsIcon.SetActive(true);
    }

    public void Close_HideWaitingForOtherPlayers()
    {
        bgs.SetActive(false);
        bgImage.enabled = false;
        waitingForOtherPlayers.SetActive(false);
        ScreenManager.Instance.SwitchScreen(null, ScreenManager.Instance.menuScreen.gameObject);
    }

    #endregion





    #region Events
    public static event Action<Clue> Event_OnIngredientFound;
    public static void OnIngredientFound(Clue _clue) => Event_OnIngredientFound?.Invoke(_clue);

    public static event Action<string, Texture2D, float> Event_OnDishFound;
    public static void OnDishFound(string _itemName, Texture2D _itemTexture, float _time) => Event_OnDishFound?.Invoke(_itemName, _itemTexture, _time);
    #endregion

}
