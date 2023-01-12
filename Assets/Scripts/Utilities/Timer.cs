using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Coroutine StartCountdown(int _time, float _gapBetweenUpdateCallback, string _roomID = null, Action<float, string> _onUpdate = null, Action _onComplete = null)
    {
        return StartCoroutine(Co_StartCoundown(_time, _gapBetweenUpdateCallback, _roomID, _onUpdate, _onComplete));
    }
    private IEnumerator Co_StartCoundown(int _time, float _gapBetweenUpdateCallback, string _roomID = null, Action<float, string> _onUpdate = null, Action _onComplete = null)
    {
        WaitForSeconds wait = new WaitForSeconds(_gapBetweenUpdateCallback);

        float counter = _time;
        _onUpdate?.Invoke(counter, _roomID);
        yield return null;

        while (counter > 1)
        {
            //print(counter);
            yield return wait;
            counter -= _gapBetweenUpdateCallback;
            _onUpdate?.Invoke(counter, _roomID);
        }

        yield return wait;
        //_onUpdate?.Invoke(0, _roomID);
        _onComplete?.Invoke();
    }


    public Coroutine StartTimer(int _startTime, int _endTime, float _gapBetweenUpdateCallback, string _roomID = null, Action<float, string> _onUpdate = null, Action _onComplete = null)
    {
        return StartCoroutine(Co_StartTimer(_startTime, _endTime, _gapBetweenUpdateCallback, _roomID, _onUpdate, _onComplete));
    }
    private IEnumerator Co_StartTimer(int _startTime, int _endTime, float _gapBetweenUpdateCallback, string _roomID = null, Action<float, string> _onUpdate = null, Action _onComplete = null)
    {
        WaitForSeconds wait = new WaitForSeconds(_gapBetweenUpdateCallback);

        float counter = _startTime;
        _onUpdate?.Invoke(counter, _roomID);
        yield return null;

        while (counter < _endTime)
        {
            //print(counter);
            yield return wait;
            counter += _gapBetweenUpdateCallback;
            _onUpdate?.Invoke(counter, _roomID);
        }

        yield return wait;
        //_onUpdate?.Invoke(_endTime, _roomID);
        _onComplete?.Invoke();
    }


    public void StopTimer(Coroutine _timer) => StopCoroutine(_timer);

    public static string GetTimeInMinAndSec(float _time)
    {
        float minutes = Mathf.FloorToInt(_time / 60);
        float seconds = Mathf.FloorToInt(_time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public static int GetMinutes(float _time) => Mathf.FloorToInt(_time / 60);

    public static int GetSeconds(float _time) => Mathf.FloorToInt(_time % 60);

    public static float GetTimeInSeconds(Tuple<string, string, string> _time) => float.Parse(_time.Item3) + float.Parse(_time.Item2) * 60;
}
