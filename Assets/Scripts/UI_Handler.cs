using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_Handler : MonoBehaviour
{
    [SerializeField] private CluesManager m_CluesManager;

    public GameObject interactionPanel_Container;
    public TextMeshProUGUI interactionText;
    public Image interactionHoldProgress;

    // Start is called before the first frame update
    void Start()
    {
        interactionPanel_Container.SetActive(false);
    }


    public void Key_E_Popup_On(string text)
    {
        interactionPanel_Container.SetActive(true);
        interactionText.text = text;
    }

    public void Key_E_Popup_Off()
    {
        interactionText.text = "";
        interactionPanel_Container.SetActive(false);
    }

    public void OnStartButton()
    {
        m_CluesManager.gameObject.SetActive(true);
    }
}
