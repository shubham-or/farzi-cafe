using UnityEngine;
using FishNet.Object;

public class Player_Controller : NetworkBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float rotattionSpeed = 500f;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] Quaternion targetRotation;

    CameraController cameraController;

    Animator animator;

    public bool isGrounded;

    public float jumpForce;

    public static bool isIdle = true;

    private void Awake()
    {

    }

    //public override void OnStartClient()
    //{
    //    base.OnStartClient();
    //}

    public void Init(Camera _main = null)
    {
        print("Init PlayerController");
        animator = GetComponent<Animator>();
        cameraController = _main == null ? Camera.main.GetComponent<CameraController>() : _main.GetComponent<CameraController>();
    }

    private void Update()
    {
        // add conditon for connection
        if (!IsOwner) return; if (cameraController == null) return;

        transform.LookAt(transform.position + cameraController.transform.forward, Vector3.up);

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        var moveInput = (new Vector3(h, 0, v)).normalized;

        //transform.position += moveInput * moveSpeed * Time.deltaTime;

        var moveDir = cameraController.PlanarRotation * moveInput;

        GroundCheck();

        //var velocity = moveDir * moveSpeed;
        //velocity.y = ySpeed;


        if (moveAmount > 0.2f)
        {
            isIdle = false;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            targetRotation = Quaternion.LookRotation(moveDir);
        }
        else isIdle = true;

        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            targetRotation, rotattionSpeed + Time.deltaTime);

        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);

        //print("moveAmount : " + moveAmount);

        if ((Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }


    public override void OnStopClient()
    {
        base.OnStopClient();
        if (IsOwner) return;

        Destroy(gameObject);

    }
}


