using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public NavMeshAgent agent;
    Animator anim;
    //public Transform player;
    State currentState;

    public List<CluePoint> checkpoints = new List<CluePoint>();
    public List<CluePoint> clueOrder = new List<CluePoint>();

    public CluePoint currentClue;
    public CluePoint chasingClue;
    public int ing_Found = 0;
    public int visited_Ing = 0;

    public string myState;

    public GameObject currentDish;

    public bool isOnMesh = false;

    public void SetRandomPoints(List<CluePoint> points, GameObject _currentDish)
    {
        currentDish = _currentDish;
        clueOrder = new List<CluePoint>(points);
        GetNextIngredient();
    }

    public void GetNextIngredient()
    {
        visited_Ing = 0;
        this.currentClue = clueOrder[ing_Found];
        List<CluePoint> _points = new List<CluePoint>(clueOrder);
        checkpoints = new List<CluePoint>();

        while (_points.Count > 0)
        {
            int i = Random.Range(0, _points.Count);
            checkpoints.Add(_points[i]);
            _points.RemoveAt(i);
        }

        GetNextPoint();
        currentState = new Pursue(agent, anim, this);
    }

    public void GetNextPoint()
    {
        if (checkpoints.Count > 0)
        {
            chasingClue = checkpoints[0];
            checkpoints.RemoveAt(0);
        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //if (!GameManager.Instance.IsOwner) return;

        if (currentState != null)
        {
            myState = currentState.name.ToString();
            currentState = currentState.Process();
            isOnMesh = agent.isOnNavMesh;
        }

        anim.SetFloat("moveAmount", agent.velocity.normalized.magnitude);
    }

    public void DestroyBot()
    {
        //GetComponent<Bot>().StopTimer();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("Enter");
        if (other.gameObject.name == "RightDoor_Collider")
            GameManager.Instance.rightdoor.GetComponentInChildren<Animator>().SetBool("IsPlay", true);
        else if (other.gameObject.name == "LeftDoor_Collider")
            GameManager.Instance.leftdoor.GetComponentInChildren<Animator>().SetBool("IsPlay", true);
    }


    private void OnTriggerExit(Collider other)
    {
        //print("Exit");
        if (other.gameObject.name == "RightDoor_Collider")
            GameManager.Instance.rightdoor.GetComponentInChildren<Animator>().SetBool("IsPlay", false);
        else if (other.gameObject.name == "LeftDoor_Collider")
            GameManager.Instance.leftdoor.GetComponentInChildren<Animator>().SetBool("IsPlay", false);
    }
}
