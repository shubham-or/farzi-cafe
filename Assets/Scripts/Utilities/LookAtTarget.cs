using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;

    void Start()
    {

    }

    void Update()
    {
        if (target)
            transform.LookAt(target);
    }
}
