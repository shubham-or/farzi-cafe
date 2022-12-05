using System.Runtime.InteropServices;

public static class FirebaseAuthLibrary
{
    [DllImport("__Internal")]
    public static extern void CreateUserWithEmailAndPassword(string email, string password, string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void SignInWithEmailAndPassword(string email, string password, string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void SignInWithGoogle(string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void SignInWithFacebook(string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void SignOut(string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void OnAuthStateChanged(string objectName, string onUserSignedIn, string onUserSignedOut);

    [DllImport("__Internal")]
    public static extern void GetUser(string objectName, string data);


}