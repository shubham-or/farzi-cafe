using FishNet;
using FishNet.Object;
using UnityEngine;

public class Player : NetworkBehaviour
{

    private void Start()
    {

    }

    [ContextMenu("InitialisePlayer")]
    private void InitialisePlayer()
    {
        gameObject.name = InstanceFinder.ClientManager.Connection.ClientId.ToString();
        //GetComponent<Player_Controller>().Init(GameplayScene.Instance.mainCamera);
        //GetComponent<Player_Controller>().enabled = true;

        GetComponent<CharacterController>().enabled = true;
        GetComponent<NaughtyCharacter.Character>().enabled = true;
        GetComponent<NaughtyCharacter.CharacterAnimator>().enabled = true;

        GetComponent<Player_Interaction>().Init();
        GetComponent<Player_Interaction>().enabled = true;
        //GameManager.Instance.UpdateRoomDetailsOnFirebase();


        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, GameplayScene.Instance.gameObject.scene);
        GameManager.Event_OnGameStarts();
        GameManager.Event_OnGameResume();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
        {
            GetComponent<Player_Interaction>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
            GetComponent<NaughtyCharacter.Character>().enabled = false;
            GetComponent<NaughtyCharacter.CharacterAnimator>().enabled = false;
            enabled = false;
        }
        else
        {
            //GameManager.Instance.playerController = GetComponent<Player_Controller>();
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
        InstanceFinder.ServerManager.Despawn(gameObject);

    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();

        GameManager.Instance.hasGameStarted = false;
        print("Player Despawn");
        InstanceFinder.ServerManager.Despawn(gameObject);
    }

}
