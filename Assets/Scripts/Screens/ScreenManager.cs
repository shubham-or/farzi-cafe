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
    //public GameObject settingsIcon;

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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnClick_SettingsIcon();
    }

    public void Init()
    {
        splashScreen.gameObject.SetActive(true);
        loginScreen.gameObject.SetActive(false);
        menuScreen.gameObject.SetActive(false);
        leaderboardScreen.gameObject.SetActive(false);
        gameplayScreen.gameObject.SetActive(false);

        //settingsIcon.SetActive(false);
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
        if (GameManager.Instance.isGameplayPaused)
        {
            menuScreen.gameObject.SetActive(false);
            splashScreen.gameObject.SetActive(false);
            loginScreen.gameObject.SetActive(false);
            //settingsIcon.SetActive(true);
            GameManager.Event_OnGameResume();
        }
        else
        {
            menuScreen.gameObject.SetActive(true);
            splashScreen.gameObject.SetActive(false);
            loginScreen.gameObject.SetActive(false);
            //settingsIcon.SetActive(false);
            GameManager.Event_OnGamePause();
        }


    }
}
