using UnityEngine;
using UnityEngine.EventSystems;

public class OnHoverMenuItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject selector;

    void Awake()
    {
        selector = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        selector.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selector.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selector.SetActive(false);
    }
}
