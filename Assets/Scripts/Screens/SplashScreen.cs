using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private float hold = 3f;


    void Start()
    {
        Init(hold);
    }

    public void Init(float _hold)
    {
        hold = _hold;
        ScreenManager.isMenuOrPopupOpen = true;
        //StopCoroutine("Co_Init");
        //StartCoroutine("Co_Init");
    }

    //private IEnumerator Co_Init()
    //{
    //    yield return new WaitForSeconds(hold);
    //    SwitchToLoginScreen();
    //}

    public void SwitchToLoginScreen()
    {
        ScreenManager.Instance.SwitchScreen(ScreenManager.Instance.splashScreen.gameObject, ScreenManager.Instance.loginScreen.gameObject);
    }

}
