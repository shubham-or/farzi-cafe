using TMPro;
using UnityEngine;

public class FirebaseAuthHandler : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;

    public TextMeshProUGUI outputText;





    private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            DisplayError("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
            return;
        }

        FirebaseAuthLibrary.OnAuthStateChanged(gameObject.name, "DisplayUserInfo", "DisplayInfo");
    }

    public void CreateUserWithEmailAndPassword() =>
        FirebaseAuthLibrary.CreateUserWithEmailAndPassword(emailInputField.text, passwordInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void SignInWithEmailAndPassword() =>
        FirebaseAuthLibrary.SignInWithEmailAndPassword(emailInputField.text, passwordInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void SignInWithGoogle() =>
        FirebaseAuthLibrary.SignInWithGoogle(gameObject.name, "OnGmailLoginSuccess", "OnGmailLoginFailed");

    public void SignInWithFacebook() =>
        FirebaseAuthLibrary.SignInWithFacebook(gameObject.name, "OnFacebookLoginSuccess", "OnFacebookLoginFailed");

    public void SignOut() =>
        FirebaseAuthLibrary.SignOut(gameObject.name, "OnSignOutSuccess", "OnSignOutFailed");

    public void GetUser() =>
     FirebaseAuthLibrary.GetUser(gameObject.name, "DisplayUserInfo");


    public void DisplayUserInfo(string user)
    {
        var parsedUser = JsonUtility.FromJson<FirebaseUser>(user);
        DisplayData($"Email: {parsedUser.email}, UserId: {parsedUser.uid}, EmailVerified: {parsedUser.isEmailVerified}");
    }

    public void DisplayData(string data)
    {
        outputText.color = outputText.color == Color.green ? Color.blue : Color.green;
        outputText.text = data;
        Debug.Log(data);
    }

    private void OnGmailLoginSuccess(string json)
    {
        DisplayInfo("Sign in Success!");

        FirebaseLoginResponse loginResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FirebaseLoginResponse>(json);

        //UIHandler.Instance.SwitchToHomePanel();
        //FirebaseHandler.Instance.firebaseDBHandlerInstance.uidText.text = loginResponse.user.uid;

        string _data = Newtonsoft.Json.JsonConvert.SerializeObject(new UserData
        {
            userDataServer = new UserDataServer
            {
                uid = loginResponse.user.uid,
                actualName = loginResponse.user.displayName,
                email = loginResponse.user.email,
                picture = loginResponse.user.photoURL,
                signInMethod = loginResponse.credential.signInMethod,
                score = "99"
            }
        });
        print("*********** " + _data);
        FirebaseDBLibrary.PostJSON(loginResponse.user.uid, _data, gameObject.name, "DisplayInfo", "DisplayErrorObject");
    }
    private void OnSuccesPost()
    {

    }


    private void OnGmailLoginFailed(string json)
    {
        DisplayErrorObject(json);
        print(json);
    }

    private void OnFacebookLoginSuccess(string json)
    {
        DisplayInfo(json);
        print(json);
    }
    private void OnFacebookLoginFailed(string json)
    {
        DisplayErrorObject(json);
        print(json);
    }

    private void OnSignOutSuccess()
    {
        DisplayInfo("Sign out Successful!");
        //UIHandler.Instance.SwitchToLoginPanel();

    }
    private void OnSignOutFailed()
    {
        DisplayInfo("Sign out Failed!");
    }

    public void DisplayInfo(string info)
    {
        print("DisplayInfo");
        outputText.color = Color.white;
        outputText.text = info;
        Debug.Log(info);
    }

    public void DisplayErrorObject(string error)
    {
        print("DisplayErrorObject");
        var parsedError = JsonUtility.FromJson<FirebaseError>(error);
        DisplayError(parsedError.message);
    }

    public void DisplayError(string error)
    {
        outputText.color = Color.red;
        outputText.text = error;
        Debug.LogError(error);
    }
}