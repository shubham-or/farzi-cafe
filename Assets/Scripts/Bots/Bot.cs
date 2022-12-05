using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private bool isBot = true;
    [SerializeField] private string uid;
    [SerializeField] private string roomID;

    public float timer = 0;

    private Coroutine timerCo;

    [ContextMenu("InitialisePlayer")]
    private void InitialisePlayer()
    {
        //if (!GameManager.Instance.IsOwner) return;

        if (isBot)
        {
            AI ai = GetComponent<AI>();
            ai.enabled = true;
            ai.SetRandomPoints(GameManager.Instance.cluesManager.chosenCluePoints, GameManager.Instance.cluesManager.GetCurrentDish());
        }
        else
        {
            GetComponent<Player_Controller>().Init(Camera.main);
            GetComponent<Player_Interaction>().Init();
        }

        //timerCo = GameManager.Instance.timer.StartTimer(0, int.MaxValue, 1, roomID, (_time, _roomid) =>
        //   {
        //       timer = _time;
        //   });


        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, GameplayScene.Instance.gameObject.scene);
    }


    public void StopTimer()
    {
        GameManager.Instance.timer.StopTimer(timerCo);
    }

    public void Start()
    {
        Invoke("InitialisePlayer", 5);
    }
}
