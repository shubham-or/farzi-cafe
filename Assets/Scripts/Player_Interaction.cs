using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;


public class Player_Interaction : NetworkBehaviour
{
    public string treasureHuntPointTags;
    public LayerMask mask = 3;
    public float interactionDistance;
    public string Hit_Object;

    private UI_Handler m_UIHandler;
    private CluesManager m_CluesManager;
    [SerializeField] private Camera MainCamera;

    bool isHit;
    bool isHitDish;
    bool isObjPicking;

    private void Awake()
    {

    }

    void Start()
    {

    }

    public override void OnOwnershipClient(NetworkConnection prevOwner)
    {
        base.OnOwnershipClient(prevOwner);
        if (!IsOwner) return;

        if (GameManager.Instance)
        {
            m_UIHandler = GameManager.Instance.uIHandler;
            m_CluesManager = GameManager.Instance.cluesManager;
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        if (!IsOwner) return;

        //if (MainCamera)
        //{
        //    MainCamera.GetComponent<CameraController>().SetFollowTarget(null);
        //    MainCamera.GetComponent<CameraController>().enabled = false;
        //}
    }

    public void Init(Camera _main = null)
    {
        if (MainCamera == null) MainCamera = _main == null ? Camera.main : _main;

        if (GameManager.Instance)
        {
            m_UIHandler = GameManager.Instance.uIHandler;
            m_CluesManager = GameManager.Instance.cluesManager;
            Popup_Disable();
        }

        //if (MainCamera)
        //{
        //    MainCamera.GetComponent<CameraController>().SetFollowTarget(transform);
        //    MainCamera.GetComponent<CameraController>().enabled = true;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner && !GameManager.Instance.player.isRunningLocal) return;
        if (GameManager.Instance.isGameplayPaused) return;
        if (m_UIHandler == null || m_CluesManager == null || GameManager.Instance.leftdoor == null || GameManager.Instance.rightdoor == null || MainCamera == null) return;

        /*
        Ray ray = new Ray(MainCamera.transform.position, (MainCamera.transform.forward) * interactionDistance);
        Debug.DrawRay(MainCamera.transform.position, (MainCamera.transform.forward) * interactionDistance, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactionDistance, mask))
        {
            Hit_Object = hit.collider.gameObject.name;

            if (((hit.collider.gameObject.tag) == "WayPoint"))
            {
                m_UIHandler.Key_E_Popup_On("  Press 'E' ", "E");
                isHit = true;
            }
            else if (((hit.collider.gameObject.tag) == "Dish"))
            {
                m_UIHandler.Key_E_Popup_On("  Press 'D' ", "D");
                isHitDish = true;
            }
            else
            {
                isHit = false;
                isHitDish = false;
            }

        }
        else
        {
            if (!isObjPicking)
                m_UIHandler.Key_E_Popup_Off();
            isHit = false;
            isHitDish = false;
        }
        */

        if (Input.GetKeyDown(KeyCode.E) && isHit)
        {
            CluePoint triggeredClue = hitCollider.gameObject.GetComponent<CluePoint>();
            if (triggeredClue != null)
            {
                print("Different Object Found...");

                if (m_CluesManager.currentClue.ingredient == triggeredClue._clue.ingredient) //remove if we want to allow user to pick up any clue
                {
                    isObjPicking = true;

                    m_UIHandler.Key_E_Popup_On(triggeredClue.gameObject.name + " Found ");

                    print("Object Found");
                    AudioManager.Instance.PlayIngredientSound();
                    PopUpManager.OnIngredientFound(triggeredClue._clue);

                    m_CluesManager.AssignNextClue(); //Assign function also to be removed for above statement
                    triggeredClue.gameObject.SetActive(false);

                    Invoke("Popup_Disable", 3f);

                }
            }

        }

        if (Input.GetKeyDown(KeyCode.D) && isHitDish)
        {
            hitCollider.gameObject.SetActive(false);

            //dish claimed + Timer off and show leaderboard
            print("dish claimed");
            AudioManager.Instance.PlayWinningSound();
            m_CluesManager.OnDishClaimed();
        }
    }

    void Popup_Disable()
    {
        m_UIHandler.Key_E_Popup_Off();
        isObjPicking = false;
        hitCollider = null;
    }


    public Collider hitCollider;
    private void OnTriggerEnter(Collider other)
    {
        //print("Enter");
        if (other.gameObject.name == "RightDoor_Collider")
            GameManager.Instance.rightdoor.SetBool("IsPlay", true);
        else if (other.gameObject.name == "LeftDoor_Collider")
            GameManager.Instance.leftdoor.SetBool("IsPlay", true);

        if (!IsOwner && !GameManager.Instance.player.isRunningLocal) return;
        else
        {
            if (other.gameObject.tag == "WayPoint")
            {
                m_UIHandler.Key_E_Popup_On("  Press 'E' ", "E");
                isHit = true;
                hitCollider = other;
            }
            else if (other.gameObject.tag == "Dish")
            {
                m_UIHandler.Key_E_Popup_On("  Press 'D' ", "D");
                isHitDish = true;
                hitCollider = other;
            }
            else
            {
                isHit = false;
                isHitDish = false;
                hitCollider = null;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "RightDoor_Collider")
        {
            if (!GameManager.Instance.rightdoor.GetBool("IsPlay"))
                GameManager.Instance.rightdoor.SetBool("IsPlay", true);

        }
        else if (other.gameObject.name == "LeftDoor_Collider")
        {
            if (!GameManager.Instance.rightdoor.GetBool("IsPlay"))
                GameManager.Instance.leftdoor.SetBool("IsPlay", true);
        }

    }


    private void OnTriggerExit(Collider other)
    {
        //print("Exit");
        if (other.gameObject.name == "RightDoor_Collider")
            GameManager.Instance.rightdoor.SetBool("IsPlay", false);
        else if (other.gameObject.name == "LeftDoor_Collider")
            GameManager.Instance.leftdoor.SetBool("IsPlay", false);

        isHit = false;
        isHitDish = false;
        hitCollider = null;
        m_UIHandler.Key_E_Popup_Off();
    }
}
