using System.Runtime.InteropServices;

public static class FirebaseDBLibrary
{
    [DllImport("__Internal")]
    public static extern void GetJSON(string path, string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void PostJSON(string path, string value, string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void PushJSON(string path, string value, string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void UpdateJSON(string path, string key, string value, string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void UpdateUserName(string path, string key, string value, string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void UpdateScore(string path, string key, string value, string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void UpdateRoomID(string path, string key, string value, string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void UpdateRoomName(string path, string key, string value, string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void DeleteJSON(string path, string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void ListenForValueChanged(string path, string objectName, string onValueChanged, string onFailed);

    [DllImport("__Internal")]
    public static extern void StopListeningForValueChanged(string path, string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void ListenForChildAdded(string path, string objectName, string onChildAdded, string onFailed);

    [DllImport("__Internal")]
    public static extern void StopListeningForChildAdded(string path, string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void ListenForChildChanged(string path, string objectName, string onChildChanged, string onFailed);

    [DllImport("__Internal")]
    public static extern void StopListeningForChildChanged(string path, string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void ListenForChildRemoved(string path, string objectName, string onChildRemoved, string onFailed);

    [DllImport("__Internal")]
    public static extern void StopListeningForChildRemoved(string path, string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void ModifyNumberWithTransaction(string path, float amount, string objectName, string onSuccess, string onFailed);

    [DllImport("__Internal")]
    public static extern void ToggleBooleanWithTransaction(string path, string objectName, string onSuccess, string onFailed);
}
