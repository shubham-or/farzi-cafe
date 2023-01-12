using FishNet;
using FishNet.Transporting.Bayou;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField] private Server server = Server.LocalHost;
    [SerializeField] private Connection connection = Connection.None;
    [SerializeField] private Bayou bayou;
    private string ip_aws = "43.204.232.118";
    private string ip_digitalocean_live = "142.93.211.48";
    private string ip_digitalocean_testing = "64.227.156.68";

    [Header("-----DEBUG-----")]
    [SerializeField] private string IP;
    [SerializeField] private ushort PORT;
    [SerializeField] private string CONNECTION;


    public enum Server { LocalHost, DigitalOcean_Dev, DigitalOcean_Live, AWS };
    public enum Connection { Client, Server, Both, None };


    public static ConnectionManager Instance;
    private void Awake()
    {
        Instance = this;
        bayou = InstanceFinder.NetworkManager.gameObject.GetComponent<Bayou>();

    }
    void Start()
    {
        if (connection == Connection.Server) InstanceFinder.ServerManager.StartConnection();
        else if (connection == Connection.Client) InstanceFinder.ClientManager.StartConnection();
        //Init();
    }

    [ContextMenu("Init")]
    public void Init()
    {
        // Server Setup
        switch (server)
        {
            case Server.LocalHost:
                PORT = 8080;
                IP = "localhost";
                break;

            case Server.DigitalOcean_Dev:
                PORT = 8008;
                IP = ip_digitalocean_testing;
                break;

            case Server.DigitalOcean_Live:
                PORT = 8800;
                IP = ip_digitalocean_live;
                break;

            case Server.AWS:
                PORT = 8000;
                IP = ip_aws;
                break;

            default:
                PORT = 8080;
                IP = "localhost";
                break;
        }
        bayou.SetPort(PORT);
        bayou.SetClientAddress(IP);



        // Mode Setup
        switch (connection)
        {
            case Connection.Client:
                CONNECTION = "Client";
                InstanceFinder.ClientManager.StartConnection();
                break;

            case Connection.Server:
                CONNECTION = "Server";
                InstanceFinder.ServerManager.StartConnection();
                break;

            case Connection.Both:
                CONNECTION = "Both";
                InstanceFinder.ServerManager.StartConnection();
                InstanceFinder.ClientManager.StartConnection();
                break;

            case Connection.None:
                CONNECTION = "NONE";
                break;
            default:
                break;
        }

        print($"PORT -> {PORT}  |  IP -> {IP}  |  SERVER -> {CONNECTION}");
    }


    public void StartLocalHost()
    {
        print("Starting LOCAlHOST Connections");
        //bayou.SetPort(8080);
        //bayou.SetClientAddress("localhost");
        InstanceFinder.ServerManager.StartConnection();
        InstanceFinder.ClientManager.StartConnection();
    }
}
