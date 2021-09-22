using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockComponent : MonoBehaviour
{
    public static float Time { get; private set; }
    private int duration = 600; //600 == 1 day
    private float factor = 1f;

    private void Update()
    {
        Time += UnityEngine.Time.deltaTime * factor;
    }
    public void Pause()
    {
        factor = 0f;
    }

    public void Resume()
    {
        factor = 1f;
    }

    public void NextDay()
    {
        Time = (((int)Time / duration) + 1) * duration;
    }

    public int GetDay()
    {
        return (int)Time / duration;
    }
}
