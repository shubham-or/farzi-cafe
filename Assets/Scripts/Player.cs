using FishNet;
using FishNet.Connection;
using FishNet.Object;
using NaughtyCharacter;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public bool isOwner = false;

    private void Start()
    {

    }

    [ContextMenu("InitialisePlayer")]
    private void InitialisePlayer()
    {
        gameObject.name = InstanceFinder.ClientManager.Connection.ClientId.ToString();

        GetComponent<CharacterController>().enabled = true;
        GetComponent<Character>().Init();
        GetComponent<Character>().enabled = true;
        GetComponent<CharacterAnimator>().enabled = true;

        GetComponent<Player_Interaction>().Init();
        GetComponent<Player_Interaction>().enabled = true;

        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, GameplayScene.Instance.gameObject.scene);
        GameManager.Event_OnGameStarts();
        GameManager.Event_OnGameResume();
    }

    public override void OnOwnershipClient(NetworkConnection prevOwner)
    {
        base.OnOwnershipClient(prevOwner);
        if (!IsOwner)
        {
            isOwner = false;
            GameManager.Instance.player = null;
            GetComponent<Player_Interaction>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
            GetComponent<Character>().enabled = false;
            GetComponent<CharacterAnimator>().enabled = false;
        }
        else
        {
            isOwner = true;
            GameManager.Instance.player = this;
            GameManager.Instance.playerInteraction = GetComponent<Player_Interaction>();
            Invoke("InitialisePlayer", 5);
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        if (!IsOwner) return;


        GameManager.Instance.hasGameStarted = false;
        print("Player Despawn");
        Despawn();

    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();

        GameManager.Instance.hasGameStarted = false;
        print("Player Despawn");
        InstanceFinder.ServerManager.Despawn(gameObject);
    }

}
