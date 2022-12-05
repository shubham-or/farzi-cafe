using UnityEngine;

public class Selector : MonoBehaviour
{
    void Start()
    {

    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
