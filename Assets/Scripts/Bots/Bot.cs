using UnityEngine;

public class Bot : MonoBehaviour
{
    //[SerializeField] private bool isBot = true;
    [SerializeField] private string uid;
    [SerializeField] private string actualName;
    [SerializeField] private string userName;
    [SerializeField] private string roomId;
    [SerializeField] private string roomName;

    [SerializeField] private TMPro.TextMeshPro playerName;
    public float timer = 0;

    private Coroutine timerCo = null;


    public void Start()
    {
        foreach (var tmp in GetComponentsInChildren<TMPro.TextMeshPro>())
        {
            tmp.text = userName;
        }

        Invoke("InitialisePlayer", 5);
    }

    public void Init(UserData _data)
    {
        uid = _data.userDataServer.uid;
        actualName = _data.userDataServer.actualName;
        userName = _data.userDataServer.userName;
        roomId = _data.userDataServer.roomId;
        roomName = _data.userDataServer.roomName;
    }


    [ContextMenu("InitialisePlayer")]
    private void InitialisePlayer()
    {
        //if (!GameManager.Instance.IsOwner) return;

        //if (isBot)
        //{
        AI ai = GetComponent<AI>();
        ai.enabled = true;
        ai.SetRandomPoints(GameManager.Instance.cluesManager.chosenCluePoints, GameManager.Instance.cluesManager.GetCurrentDish());

        GetComponentInChildren<LookAtTarget>().target = Camera.main.transform;
        GetComponentInChildren<LookAtTarget>().enabled = true;
        GetComponentInChildren<SpriteRenderer>().enabled = true;

        //}
        //else
        //{
        //    GetComponent<Player_Controller>().Init(Camera.main);
        //    GetComponent<Player_Interaction>().Init();
        //}

        //timerCo = GameManager.Instance.timer.StartTimer(0, int.MaxValue, 1, roomID, (_time, _roomid) =>
        //   {
        //       timer = _time;
        //   });


        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, GameplayScene.Instance.gameObject.scene);
    }


    public void StopTimer()
    {
        if (timerCo != null)
        {
            GameManager.Instance.timer.StopTimer(timerCo);
            timerCo = null;
        }
    }


    private void OnDestroy()
    {
        StopTimer();
    }

}
