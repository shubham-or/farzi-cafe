using FishNet.Connection;
using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public List<GameObject> players;


    [SerializeField] private GameObject playerNetworkPrefab;
    [SerializeField] private GameObject playerLocalPrefab;

    public List<string> _debug = new List<string>();


    public static PlayerSpawner Instance;
    private void Awake()
    {
        _debug = new List<string>();
        Instance = this;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        print("OnStart Client");
        _debug.Add("OnStart Client");
    }



    public GameObject SpawnLocalPlayer()
    {
        GameObject go = Instantiate(playerLocalPrefab);
        players.Add(go);
        go.SetActive(true);

        return go;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        print("OnStop Server");
        _debug.Add("OnStop Server");
    }



    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        print("OnStart Network");
        _debug.Add("OnStart Network");
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        print("OnStart Server");
        _debug.Add("OnStart Server");
    }

    public override void OnOwnershipServer(NetworkConnection prevOwner)
    {
        base.OnOwnershipServer(prevOwner);
        print("OnOwnership Server");
        _debug.Add("OnOwnership Server");
    }

    public override void OnOwnershipClient(NetworkConnection prevOwner)
    {
        base.OnOwnershipClient(prevOwner);
        print("OnOwnership Client");
        _debug.Add("OnOwnership Client");
    }

    public override void OnSpawnServer(NetworkConnection connection)
    {
        base.OnSpawnServer(connection);
        print("OnSpawn Server");
        _debug.Add("OnSpawn Server");
    }

    public override void OnDespawnServer(NetworkConnection connection)
    {
        base.OnDespawnServer(connection);
        print("OnDespawn Server");
        _debug.Add("OnDespawn Server");
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        print("OnStop Client");
        _debug.Add("OnStop Client");
    }



    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        print("OnStop Network");
        _debug.Add("OnStop Network");
    }
}
