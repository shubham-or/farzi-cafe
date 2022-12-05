using System;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public SplashScreen splashScreen;
    public LoginScreen loginScreen;
    public MenuScreen menuScreen;
    public GameplayScreen gameplayScreen;
    public LeaderboardScreen leaderboardScreen;


    [Space(20)]
    public GameObject settingsIcon;
    public static bool isMenuOrPopupOpen = false;

    public static ScreenManager Instance;
    private void Awake()
    {
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }
    public GameObject currentScreen;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        splashScreen.gameObject.SetActive(true);
        loginScreen.gameObject.SetActive(false);
        menuScreen.gameObject.SetActive(false);
        leaderboardScreen.gameObject.SetActive(false);
        gameplayScreen.gameObject.SetActive(false);

        settingsIcon.SetActive(false);
        currentScreen = splashScreen.gameObject;
    }


    public void SwitchScreen(GameObject _from, GameObject _to)
    {
        if (_to != null)
            _to.SetActive(true);
        if (_from != null)
            _from.SetActive(false);
    }

    public void OnClick_SettingsIcon()
    {
        isMenuOrPopupOpen = !isMenuOrPopupOpen;
        menuScreen.gameObject.SetActive(isMenuOrPopupOpen);
        splashScreen.gameObject.SetActive(false);
        loginScreen.gameObject.SetActive(false);
        settingsIcon.SetActive(false);
    }

    #region Events
    public static event Action Event_OnPlay;
    public static void OnPlay() => Event_OnPlay?.Invoke();
    #endregion
}
