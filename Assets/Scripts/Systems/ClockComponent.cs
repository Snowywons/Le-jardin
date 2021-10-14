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
    [SerializeField] Image arrowIndicator;

    [SerializeField] GameObject pauseText;

    private float currentDayTime;
    private float val;
    private void Update()
    {
        val = Time.deltaTime * factor * 50;
        currentDayTime += val;
        time += val;

        UpdateTimeText();
        UpdateArrowIndicator();

        if (currentDayTime >= 600)
            NextDay();
    }

    private void UpdateTimeText()
    {
        timeText.text = "";
        int hours = ((int)currentDayTime / (duration / 10)) % 12 + 9;
        int minutes = (int)currentDayTime % (duration / 10);

        if (timeText)
        {
            timeText.text = $"{hours}:";

            if (minutes < 10)
            {
                timeText.text += $"0{minutes}";
            }
            else
            {
                timeText.text += $"{minutes}";
            }

            if (hours < 12)
            {
                timeText.text += " am";
            }
            else
            {
                timeText.text += " pm";
            }
        }
    }

    private void UpdateDayText()
    {
        // TO DO : Un système de calendrier à la place

        if (dayText)
            dayText.text = $"Jour {GetDay()}";
    }

    private void UpdateArrowIndicator()
    {
        if (arrowIndicator)
        {
            Vector3 angle = new Vector3(0, 0, 90 - (currentDayTime % 600) * 180 / 600);
            arrowIndicator.transform.rotation = Quaternion.Euler(angle);
        }
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
        currentDayTime = 0;

        UpdateDayText();
    }

    public int GetDay() => ((int)time / duration) + 1;

    public float GetTime() => time;
}
