using FishNet.Object;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [ContextMenu("InitialisePlayer")]
    private void InitialisePlayer()
    {
        GetComponent<Player_Controller>().Init(GameplayScene.Instance.mainCamera);
        GetComponent<Player_Controller>().enabled = true;
        GetComponent<Player_Interaction>().Init();
        GetComponent<Player_Interaction>().enabled = true;

        //GameManager.Instance.UpdateRoomDetailsOnFirebase();


        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, GameplayScene.Instance.gameObject.scene);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
        {
            GetComponent<Player_Controller>().enabled = false;
            GetComponent<Player_Interaction>().enabled = false;
            enabled = false;
        }
        else
        {
            GameManager.Instance.playerController = GetComponent<Player_Controller>();
            GameManager.Instance.playerInteraction = GetComponent<Player_Interaction>();

            Invoke("InitialisePlayer", 5);
        }
    }


    public override void OnStopClient()
    {
        base.OnStopClient();
        if (!IsOwner) return;

        GameManager.Instance.hasGameStarted = false;

    }

}
