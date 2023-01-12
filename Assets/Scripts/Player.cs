using FishNet;
using FishNet.Connection;
using FishNet.Object;
using NaughtyCharacter;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public bool isOwner = false;
    public bool isRunningLocal = false;

    private void Start()
    {
        UpdateDisplayName(GameManager.Instance.GetUserData().userDataServer.userName);
    }


    [ContextMenu("InitialisePlayer")]
    private void InitialisePlayer()
    {
        gameObject.name = GameManager.Instance.GetUserData().userDataServer.userName;

        GetComponent<CharacterController>().enabled = true;

        GetComponent<Character>().Init();
        GetComponent<Character>().enabled = true;

        GetComponent<CharacterAnimator>().enabled = true;

        GetComponent<Player_Interaction>().Init();
        GetComponent<Player_Interaction>().enabled = true;

        GetComponentInChildren<LookAtTarget>().target = Camera.main.transform;
        GetComponentInChildren<LookAtTarget>().enabled = true;
        GetComponentInChildren<SpriteRenderer>().enabled = true;


        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, GameplayScene.Instance.gameObject.scene);
        GameManager.Event_OnGameStarts();
        GameManager.Event_OnGameResume();
    }

    public override void OnOwnershipClient(NetworkConnection prevOwner)
    {
        base.OnOwnershipClient(prevOwner);
        isRunningLocal = false;
        if (!IsOwner)
        {
            isOwner = false;
            GameManager.Instance.player = null;
            GetComponent<Player_Interaction>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
            GetComponent<Character>().enabled = false;
            GetComponent<CharacterAnimator>().enabled = false;
            GetComponentInChildren<LookAtTarget>().target = null;
            GetComponentInChildren<LookAtTarget>().enabled = false;
        }
        else
        {
            isOwner = true;
            GameManager.Instance.player = this;
            GameManager.Instance.character = GetComponent<Character>();
            GameManager.Instance.playerInteraction = GetComponent<Player_Interaction>();
            Invoke("InitialisePlayer", 5);
        }
    }


    public void InitialisePlayer_Local()
    {
        isOwner = true;
        isRunningLocal = true;
        GameManager.Instance.player = this;
        GameManager.Instance.character = GetComponent<Character>();
        GameManager.Instance.playerInteraction = GetComponent<Player_Interaction>();
        Invoke("InitialisePlayer", 5);
    }


    public override void OnStopClient()
    {
        base.OnStopClient();
        if (!IsOwner) return;

        //Despawn();
        if (InstanceFinder.ServerManager.isActiveAndEnabled)
        {
            print("Depawn Player Client");
            InstanceFinder.ServerManager.Despawn(gameObject);
        }
        //ServerInstancing.Instance.DepawnPlayer(Owner, gameObject);
    }


    public void UpdateDisplayName(string _userName)
    {
        foreach (var tmp in GetComponentsInChildren<TMPro.TextMeshPro>())
            tmp.text = _userName;
    }

}
