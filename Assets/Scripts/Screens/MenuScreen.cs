using FishNet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour
{
    [Header("-----Menu Items-----")]
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject howToPlay;
    [SerializeField] private GameObject exit;
    [SerializeField] private GameObject settingsIcon;


    [Header("-----Settings Items-----")]
    [SerializeField] private TMP_InputField userName;
    [SerializeField] private Slider volume;
    [SerializeField] private Slider music;
    [SerializeField] private Slider soundEffects;
    [SerializeField] private TextMeshProUGUI error;


    [SerializeField] private TextMeshProUGUI playButtonText;



    private void Awake()
    {
        userName.onEndEdit.AddListener(OnInputField_Username);
        volume.onValueChanged.AddListener(OnSlider_Volume);
        music.onValueChanged.AddListener(OnSlider_Music);
        soundEffects.onValueChanged.AddListener(OnSlider_SoundEffects);
    }

    private void Start() => Init();

    private void OnEnable() => playButtonText.text = GameManager.Instance.hasGameStarted ? "Resume" : "Play";

    private void OnDisable() => Init();

    private void Init()
    {
        buttons.SetActive(true);
        settings.SetActive(false);
        howToPlay.SetActive(false);
        exit.SetActive(false);
        error.gameObject.SetActive(false);
    }



    #region Menu Buttons
    public void OnClick_Play()
    {
        if (GameManager.Instance.hasGameStarted)
        {
            ScreenManager.isMenuOrPopupOpen = !ScreenManager.isMenuOrPopupOpen;
            gameObject.SetActive(false);
            settingsIcon.SetActive(true);
        }
        else
        {
            ServerInstancing.Instance.QuickRaceConnect(GameManager.Instance.GetUserData(), InstanceFinder.ClientManager.Connection);
            PopUpManager.Instance.ShowPopup_WaitingForOtherPlayers(20);
        }

    }

    public void OnClick_Settings()
    {
        if (string.IsNullOrWhiteSpace(userName.text))
            userName.text = GameManager.Instance.GetUserData().userDataServer.userName;

        settings.SetActive(true);
        buttons.SetActive(false);
    }

    public void OnClick_HowToPlay()
    {
        howToPlay.SetActive(true);
        buttons.SetActive(false);
    }

    public void OnClick_Leaderboard()
    {
        ScreenManager.Instance.SwitchScreen(ScreenManager.Instance.menuScreen.gameObject, ScreenManager.Instance.leaderboardScreen.gameObject);
    }

    public void OnClick_Exit()
    {
        exit.SetActive(true);
        buttons.SetActive(false);
    }
    #endregion


    #region Settings
    public void OnInputField_Username(string _value)
    {
        error.gameObject.SetActive(false);
#if UNITY_EDITOR
        OnSuccess_UpdateUsername("");
#elif UNITY_WEBGL && !UNITY_EDITOR
        FirebaseDBLibrary.UpdateUserName(GameManager.Instance.GetUserData().userDataServer.uid, "userName", userName.text.Trim().ToUpper(), gameObject.name, "OnSuccess_UpdateUsername", "OnFailed_UpdateUsername");
#endif
    }

    public void OnSuccess_UpdateUsername(string _json)
    {
        error.text = "Successfully updated!";
        error.color = Color.green;
    }

    public void OnFailed_UpdateUsername(string _json)
    {
        error.text = "Something went wrong. Try again!";
        error.gameObject.SetActive(true);
        error.color = Color.red;
    }

    public void OnSlider_Volume(float _value)
    {

    }

    public void OnSlider_Music(float _value)
    {

    }

    public void OnSlider_SoundEffects(float _value)
    {

    }
    #endregion



    public void OnClick_Close(GameObject _go)
    {
        buttons.SetActive(true);
        _go.SetActive(false);
    }

    public void OnClick_ExitYes()
    {
        Application.Quit();
    }

}
