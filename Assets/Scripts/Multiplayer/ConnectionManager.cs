using FishNet;
using FishNet.Transporting.Bayou;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField] private Server server = Server.LocalHost;
    [SerializeField] private Connection connection = Connection.None;
    [SerializeField] private string awsIp = "43.204.232.118";
    [SerializeField] private string doceanIp = "142.93.211.48";
    [SerializeField] private Bayou bayou;


    public enum Server { LocalHost, DigitalOcean, AWS };
    public enum Connection { Client, Server, Both, None };

    private void Awake() => bayou = InstanceFinder.NetworkManager.gameObject.GetComponent<Bayou>();

    void Start() => Init();

    [ContextMenu("Init")]
    public void Init()
    {
        // Server Setup
        switch (server)
        {
            case Server.LocalHost:
                bayou.SetPort(8080);
                bayou.SetClientAddress("localhost");
                break;
            case Server.DigitalOcean:
                bayou.SetPort(8000);
                bayou.SetClientAddress(doceanIp);
                break;
            case Server.AWS:
                bayou.SetPort(8000);
                bayou.SetClientAddress(awsIp);
                break;
            default:
                bayou.SetPort(8080);
                bayou.SetClientAddress("localhost");
                break;
        }

        // Mode Setup
        switch (connection)
        {
            case Connection.Client:
                InstanceFinder.ClientManager.StartConnection();
                break;
            case Connection.Server:
                InstanceFinder.ServerManager.StartConnection();
                break;
            case Connection.Both:
                InstanceFinder.ServerManager.StartConnection();
                InstanceFinder.ClientManager.StartConnection();
                break;
            case Connection.None:
                InstanceFinder.ServerManager.StartConnection();
                InstanceFinder.ClientManager.StartConnection();
                break;
            default:
                break;
        }
    }
}
