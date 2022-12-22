using System.Runtime.InteropServices;

public static class ReactHandler
{
    [DllImport("__Internal")]
    public static extern void Web3LoginRequest(string _objectName, string _methodName);

    [DllImport("__Internal")]
    public static extern void Web3LogoutRequest(string _objectName, string _methodName);

    [DllImport("__Internal")]
    public static extern void QuitGame();
}
