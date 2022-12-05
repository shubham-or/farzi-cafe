using FishNet;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField] private string awsIp = "43.204.232.118";
    [SerializeField] private string port = "8000";
    [SerializeField] private string doceanIp = "142.93.211.48";

    public bool isServer;

    void Start()
    {
        if (isServer) InstanceFinder.ServerManager.StartConnection();
        else InstanceFinder.ClientManager.StartConnection();
    }
}
