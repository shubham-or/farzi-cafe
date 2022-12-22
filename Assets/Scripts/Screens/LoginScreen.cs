using UnityEngine;

public class LoginScreen : MonoBehaviour
{
    public string data = "";

    [Header("-----Login-----")]
    [SerializeField] private TMPro.TextMeshProUGUI loginTitle;
    [SerializeField] private Color loginErrorColor;
    [SerializeField] private GameObject socialLogin;

    [Header("-----Welcome-----")]
    [SerializeField] private GameObject welcome;
    [SerializeField] private TMPro.TMP_InputField userName;
    [SerializeField] private TMPro.TextMeshProUGUI error;

    void Start()
    {
        userName.onEndEdit.AddListener(OnInputField_Username);

        Init();
    }

    public void Init()
    {
        loginTitle.text = "Login with";
        loginTitle.color = Color.black;

        welcome.SetActive(false);
        socialLogin.SetActive(true);
        userName.text = "";

        error.gameObject.SetActive(false);
    }



    #region Web3Auth
    public void Web3Login_Callback(string _jsonData)
    {
        if (string.IsNullOrWhiteSpace(_jsonData))
        {
            print("Web3Login_Callback FAILED - " + _jsonData);
            loginTitle.text = "Login unsuccessful. try again!";
            loginTitle.color = Color.black;
        }
        else
        {
            Web3AuthResponse.LoginResponse _response = Newtonsoft.Json.JsonConvert.DeserializeObject<Web3AuthResponse.LoginResponse>(_jsonData);
            print("Web3Login_Callback - " + _jsonData);
            GameManager.Instance.SetUserData(new UserData
            {
                userDataServer = new UserDataServer
                {
                    uid = _response.address,
                    actualName = _response.data.name,
                    userName = _response.data.name,
                    email = _response.data.email,
                    picture = _response.data.profileImage,
                    signInMethod = _response.data.typeOfLogin,
                    score = "0"
                }
            });
            OnSuccess_PostData(_response.address);
        }
    }

    public void Web3Logout_Callback(string _jsonData)
    {
        if (string.IsNullOrWhiteSpace(_jsonData))
        {
            print("Web3Logout_Callback FAILED - " + _jsonData);
        }
        else
        {
            print("Web3Logout_Callback - " + _jsonData);
        }
    }
    #endregion



    #region Gmail
    public void OnClick_Gmail()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        socialLogin.SetActive(false);
        welcome.SetActive(true);
        userName.text = "Gmail";
        GameManager.Instance.SetUserData(new UserData
        {
            userDataServer = new UserDataServer
            {
                uid = System.Guid.NewGuid().ToString(),
                actualName = "Gmail",
                userName = "Gmail",
                email = "",
                picture = "",
                signInMethod = "",
                score = "0",
            }
        });
        print("Gameobject - " + gameObject.name);
#elif UNITY_WEBGL && !UNITY_EDITOR
        if (GameManager.Instance.useWeb3)
            ReactHandler.Web3LoginRequest(gameObject.name, "Web3Login_Callback");
        else if (GameManager.Instance.useFirebase)
            FirebaseAuthLibrary.SignInWithGoogle(gameObject.name, "OnGmail_Success", "OnGmail_Failed");
#endif
    }

    UserData _tempData = null;

    public void OnGmail_Success(string _json)
    {
        print("OnGmail_Success-" + _json);
        FirebaseLoginResponse loginResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FirebaseLoginResponse>(_json);

        _tempData = new UserData
        {
            userDataServer = new UserDataServer
            {
                uid = loginResponse.user.uid,
                actualName = loginResponse.user.displayName,
                userName = loginResponse.user.displayName,
                email = loginResponse.user.email,
                picture = loginResponse.user.photoURL,
                signInMethod = loginResponse.credential.signInMethod,
                score = "0"
            }
        };

        if (GameManager.Instance.useFirebase)
            FirebaseDBLibrary.GetJSON(loginResponse.user.uid, gameObject.name, "OnGmail_GetDataSuccess", "OnGmail_GetDataFailed");
        else
            OnSuccess_PostData("");
    }

    public void OnGmail_Failed(string _json)
    {
        print("OnGmail_Failed-" + _json);
        loginTitle.text = "Login unsuccessful. try again!";
        loginTitle.color = Color.black;
    }

    public void OnGmail_GetDataSuccess(string _json)
    {
        if (string.IsNullOrWhiteSpace(_json) || _json.Trim().ToLower().Equals("null"))
        {
            print("OnGmail_GetDataSuccess IF - " + _json);
            GameManager.Instance.SetUserData(new UserData
            {
                userDataServer = new UserDataServer
                {
                    uid = _tempData.userDataServer.uid,
                    actualName = _tempData.userDataServer.actualName,
                    userName = _tempData.userDataServer.userName,
                    email = _tempData.userDataServer.email,
                    picture = _tempData.userDataServer.picture,
                    signInMethod = _tempData.userDataServer.signInMethod,
                    score = _tempData.userDataServer.score,
                    roomId = "",
                    roomName = "",
                    clientId = "",
                    lastUpdated = System.DateTime.Now.ToString()
                }
            });
            string _data = Newtonsoft.Json.JsonConvert.SerializeObject(GameManager.Instance.GetUserData().userDataServer);
            FirebaseDBLibrary.PostJSON(GameManager.Instance.GetUserData().userDataServer.uid, _data, gameObject.name, "OnSuccess_PostData", "OnFailed_PostData");
        }
        else
        {
            print("OnGmail_GetDataSuccess ELSE - " + _json);
            _tempData.userDataServer = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDataServer>(_json);
            GameManager.Instance.SetUserData(_tempData);
        }
        OnSuccess_PostData(_json);
    }

    public void OnGmail_GetDataFailed(string _json)
    {
        print("OnGmail_GetDataFailed- " + _json);

        GameManager.Instance.SetUserData(new UserData
        {
            userDataServer = new UserDataServer
            {
                uid = _tempData.userDataServer.uid,
                actualName = _tempData.userDataServer.actualName,
                userName = _tempData.userDataServer.userName,
                email = _tempData.userDataServer.email,
                picture = _tempData.userDataServer.picture,
                signInMethod = _tempData.userDataServer.signInMethod,
                score = _tempData.userDataServer.score
            }
        });
        string _data = Newtonsoft.Json.JsonConvert.SerializeObject(GameManager.Instance.GetUserData());
        FirebaseDBLibrary.PostJSON(GameManager.Instance.GetUserData().userDataServer.uid, _data, gameObject.name, "OnSuccess_PostData", "OnFailed_PostData");
    }
    #endregion



    #region Facebook
    public void OnClick_Facebook()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        socialLogin.SetActive(false);
        welcome.SetActive(true);
        userName.text = "Facebook";
        GameManager.Instance.SetUserData(new UserData
        {
            userDataServer = new UserDataServer
            {
                uid = System.Guid.NewGuid().ToString(),
                actualName = "Facebook",
                userName = "Facebook",
                email = "",
                picture = "",
                signInMethod = "",
                score = "0",
            }
        });
#elif UNITY_WEBGL && !UNITY_EDITOR
         if (GameManager.Instance.useWeb3)
            ReactHandler.Web3LoginRequest(gameObject.name, "Web3Login_Callback");
        else if (GameManager.Instance.useFirebase)
            FirebaseAuthLibrary.SignInWithGoogle(gameObject.name, "OnGmail_Success", "OnGmail_Failed");
#endif
    }

    public void OnFacebook_Success(string _json)
    {
        FirebaseLoginResponse loginResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FirebaseLoginResponse>(_json);
        GameManager.Instance.SetUserData(new UserData
        {
            userDataServer = new UserDataServer
            {
                uid = loginResponse.user.uid,
                actualName = loginResponse.user.displayName,
                userName = loginResponse.user.displayName,
                email = loginResponse.user.email,
                picture = loginResponse.user.photoURL,
                signInMethod = loginResponse.credential.signInMethod,
                score = "0"
            }
        });

        string _data = Newtonsoft.Json.JsonConvert.SerializeObject(GameManager.Instance.GetUserData());
        FirebaseDBLibrary.PostJSON(GameManager.Instance.GetUserData().userDataServer.uid, _data, gameObject.name, "OnSuccess_PostData", "OnFailed_PostData");

    }

    public void OnFacebook_Failed(string _json)
    {
        loginTitle.text = "Login unsuccessful. try again!";
        loginTitle.color = Color.black;
    }
    #endregion



    public void OnSuccess_PostData(string _json)
    {
        print("OnSuccess_PostData- " + _json);
        socialLogin.SetActive(false);
        welcome.SetActive(true);

        userName.text = GameManager.Instance.GetUserData().userDataServer.userName;
    }

    public void OnFailed_PostData(string _json)
    {
        print("OnFailed_PostData - " + _json);
        loginTitle.text = "Login unsuccessful. try again!";
        loginTitle.color = Color.black;
        Debug.Log("Gmail -> Post data failed");
    }

    public void OnInputField_Username(string _value)
    {
        error.gameObject.SetActive(false);
    }

    public void OnClick_Enter()
    {
        userName.text = GameManager.Instance.GetUserData().userDataServer.userName;
        error.gameObject.SetActive(false);
#if UNITY_EDITOR  || UNITY_STANDALONE_WIN
        OnSuccess_UpdateUsername("");
#elif UNITY_WEBGL
        if (GameManager.Instance.useFirebase)
            FirebaseDBLibrary.UpdateUserName(GameManager.Instance.GetUserData().userDataServer.uid, "userName", userName.text.Trim().ToUpper(), gameObject.name, "OnSuccess_UpdateUsername", "OnFailed_UpdateUsername");
        else  
            OnSuccess_UpdateUsername("");
#endif
    }

    public void OnSuccess_UpdateUsername(string _json)
    {
        print("OnSuccess_UpdateUsername- " + _json);
        GameManager.Instance.GetUserData().userDataServer.userName = userName.text;


        ScreenManager.Instance.SwitchScreen(ScreenManager.Instance.loginScreen.gameObject, ScreenManager.Instance.menuScreen.gameObject);
    }

    public void OnFailed_UpdateUsername(string _json)
    {
        print("OnFailed_UpdateUsername- " + _json);
        error.gameObject.SetActive(true);
    }


}
