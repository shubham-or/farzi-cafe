using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;

    [SerializeField] float distance;
    [SerializeField] float minVerticalAngle, maxVerticalAngle;
    [SerializeField] float minHorizontalAngle, maxHorizontalAngle;
    [SerializeField] float minHorizontalAngleIdle, maxHorizontalAngleIdle;
    [SerializeField] Vector3 idlePosition;
    [SerializeField] private Quaternion idleRotation;
    [SerializeField] float rotationSpeed;

    [SerializeField] Vector2 framingOffset;

    [SerializeField] bool invertX, invertY;
    float invertXVal, invertYVal;

    float rotationY;
    float rotationX;
    //private Player_Controller player_Controller;

    private void Awake()
    {

    }

    public void SetFollowTarget(Transform _target)
    {
        if (_target == null)
        {
            followTarget = null;
            //player_Controller = null;
        }
        else
        {
            followTarget = _target.transform;
            //player_Controller = followTarget.GetComponent<Player_Controller>();
        }
    }



    private void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        //idleRotation = Quaternion.Euler(idlePosition.x, idlePosition.y, idlePosition.z);
    }

    private void Update()
    {
        if (followTarget == null) return;

        invertXVal = (invertX) ? -1 : 1;
        invertYVal = (invertY) ? -1 : 1;

        rotationX += Input.GetAxis("Mouse Y") * invertYVal * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        rotationY += Input.GetAxis("Mouse X") * invertXVal * rotationSpeed;
        //rotationY = Player_Controller.isIdle ? Mathf.Clamp(rotationY, minHorizontalAngleIdle, maxHorizontalAngleIdle) : Mathf.Clamp(rotationY, minHorizontalAngle, maxHorizontalAngle);


        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
        transform.rotation = targetRotation;

        var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);
        transform.position = focusPosition - targetRotation * new Vector3(0, 0, distance);

        //transform.position = new Vector3(transform.position.x, transform.position.y, (followTarget.position.z - distance));
        //if (Player_Controller.isIdle)
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, idleRotation, rotationSpeed);
        //}
        //else
        //{
        //    transform.position = Vector3.Lerp(transform.position, idlePosition, rotationSpeed);
        //}

    }

    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);

}
