using UnityEngine;
using UnityEngine.UI;
using System;

public class Loading : MonoBehaviour
{
    [SerializeField] private int count = 36;
    [SerializeField] private float angle = 10;
    [SerializeField] private float animSpeed = 0.2f;


    [Header("-----Debug-----")]
    [SerializeField] private bool isLoaded = false;
    [SerializeField] private float timer;
    [SerializeField] private int counter = 0;

    private bool isInit = false;

    void Start()
    {
        timer = animSpeed;
        Init(count, angle, isLoaded);
    }

    void Update()
    {
        if (isInit && !isLoaded && counter <= count)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                transform.transform.GetChild(counter++).GetComponentInChildren<Image>().color = Color.black;
                timer = animSpeed;
                if (counter == count)
                {
                    isLoaded = true;
                    Event_OnLoadingComplete?.Invoke(ScreenManager.Instance.currentScreen);
                    if (ScreenManager.Instance.currentScreen == ScreenManager.Instance.splashScreen.gameObject)
                        ScreenManager.Instance.splashScreen.SwitchToLoginScreen();
                }
            }
        }
    }


    private void Init(int _count, float _angle, bool _isLoaded = false)
    {
        Transform _spawn = transform.GetChild(0);
        _spawn.GetComponentInChildren<Image>().color = _isLoaded ? Color.black : Color.white;
        float _rotValue = _angle;
        for (int i = 0; i < _count - 1; i++)
        {
            RectTransform _dot = Instantiate(_spawn, Vector3.zero, Quaternion.Euler(new Vector3(0, 0, -_rotValue)), _spawn.parent).GetComponent<RectTransform>();
            _dot.anchoredPosition = Vector3.zero;
            _dot.name = $"{i + 1}_{_rotValue}";
            _dot.GetComponentInChildren<Image>().color = _isLoaded ? Color.black : Color.white;
            _rotValue += _angle;
        }
        isInit = true;
    }

    private void OnValidate()
    {
        if (count == 0 || string.IsNullOrWhiteSpace(count.ToString()))
        {
            angle = 0;
            return;
        }

        angle = 360f / count;
    }


    public static event Action<GameObject> Event_OnLoadingComplete;

}
