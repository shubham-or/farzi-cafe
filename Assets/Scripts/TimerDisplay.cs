using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class TimerDisplay : MonoBehaviour
{
    public float timeValue = 0;
    public TMP_Text timeText;

    //public bool gameStarted;

    private float minutes;
    private float seconds;

    void Update()
    {
        timeValue += Time.deltaTime;
        //else
        //{
        //    timeValue = 0;
        //}

        DisplayTime(timeValue);
    }

    public void DisplayTime(float timeToDisplay)
    {
        //if (timeToDisplay < 0)
        //{
        //    timeToDisplay = 0;
        //}

        //else if(timeToDisplay>0)
        //{
        //    timeToDisplay += 1;
        //}

        minutes = Mathf.FloorToInt(timeToDisplay / 60);
        seconds = Mathf.FloorToInt(timeToDisplay % 60);

        //float miliseconds = timeToDisplay % 1 * 1000;
        //timeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, miliseconds);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    public Tuple<string, string, string> GetTimer()
    {
        return new Tuple<string, string, string>("00", minutes.ToString(), seconds.ToString());
    }

    private void OnDisable()
    {
        GameManager.Instance.hasGameStarted = false;
        timeValue = minutes = seconds = 0;
    }

}
