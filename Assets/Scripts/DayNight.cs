using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DayNight : MonoBehaviour
{
    public State state;

    public float dayTime;
    public float nightTime;

    [SerializeField]
    private Light2D globalLight;

    private float nowTime = 0f;
    private float nowNightTime = 0f;

    private void Start()
    {
        nowTime = dayTime / 2;
    }

    void Update()
    {
        if (nowNightTime > 0f)
        {
            nowNightTime -= Time.deltaTime;
        }
        else
        {
            nowTime += Time.deltaTime;
            state = State.Day;
            if (nowTime <= dayTime / 2) globalLight.intensity = nowTime / (dayTime / 2);
            else if (nowTime > dayTime / 2 && nowTime < dayTime) globalLight.intensity = (dayTime - nowTime) / (dayTime / 2);
            else
            {
                nowTime = 0f;
                nowNightTime = nightTime;
                state = State.Night;
            }
        }
    }
}

public enum State
{
    Day = 0,
    Night = 1
}
