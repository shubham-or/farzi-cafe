using System;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public SplashScreen splashScreen;
    public LoginScreen loginScreen;
    public MenuScreen menuScreen;
    public GameplayScreen gameplayScreen;
    public LeaderboardScreen leaderboardScreen;
    public NFTScreen nftScreen;

    public GameObject currentScreen;


    public static ScreenManager Instance;
    private void Awake() => Instance = this;


    void Start() => Init();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.hasGameStarted)
            OnClick_SettingsIcon();
    }

    public void Init()
    {
        splashScreen.gameObject.SetActive(false);
        loginScreen.gameObject.SetActive(true);
        menuScreen.gameObject.SetActive(false);
        leaderboardScreen.gameObject.SetActive(false);
        gameplayScreen.gameObject.SetActive(false);
        nftScreen.gameObject.SetActive(false);

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
        if (Application.isEditor)
        {
            if (GameManager.Instance.isGameplayPaused)
                GameManager.Event_OnGameResume();
            else
                GameManager.Event_OnGamePause();
        }
        else
        {
            if (GameManager.Instance.isGameplayPaused)
            {
                menuScreen.gameObject.SetActive(false);
                splashScreen.gameObject.SetActive(false);
                loginScreen.gameObject.SetActive(false);
                GameManager.Event_OnGameResume();
            }
            else
            {
                menuScreen.gameObject.SetActive(true);
                splashScreen.gameObject.SetActive(false);
                loginScreen.gameObject.SetActive(false);
                GameManager.Event_OnGamePause();
            }
        }


    }
}
