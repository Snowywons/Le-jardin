using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockComponent : MonoBehaviour
{
    private float time;
    private int duration = 600; //600 secondes == 1 day || 10 minutes == 1 day
    private float factor = 1f;

    [SerializeField] Text timeText;
    [SerializeField] Text dayText;

    [SerializeField] GameObject pauseText;

    private void Update()
    {
        time += Time.deltaTime * factor;

        UpdateTimeText();

        if (!pauseText)
            Debug.Log("no there anymore!");
    }

    private void UpdateTimeText()
    {
        if (timeText)
            timeText.text = $"{((int)time / (duration / 10)) % 10} : {(int)time % (duration / 10)}";
    }

    private void UpdateDayText()
    {
        // TO DO : Un système de calendrier à la place

        if (dayText)
            dayText.text = $"Jour {GetDay()}";
    }

    public void Pause()
    {
        factor = 0f;
        pauseText.SetActive(true);
    }

    public void Resume()
    {
        factor = 1f;
        pauseText.SetActive(false);
    }

    public void NextDay()
    {
        time = (((int)time / duration) + 1) * duration;

        UpdateDayText();
    }

    public int GetDay() => ((int)time / duration) + 1;

    public float GetTime() => time;
}
