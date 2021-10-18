using System;
using UnityEngine;
using UnityEngine.UI;

enum Days
{
    Dimanche,
    Lundi,
    Mardi,
    Mercredi,
    Jeudi,
    Vendredi,
    Samedi
}

public class ClockComponent : MonoBehaviour
{
    [SerializeField] [Range(0, 24)] int dayStartAt;
    [SerializeField] [Range(0, 24)] int dayEndAt;
    [SerializeField] float daySpeed = 1f;

    [SerializeField] Text timeText;
    [SerializeField] Text dayText;
    [SerializeField] Image arrowIndicator;

    private int duration;
    private float time;
    private float currentDayTime;

    private bool isPaused;

    private void Start()
    {
        duration = (dayEndAt - dayStartAt) * (60 * 60);
        UpdateDayText();
    }

    private void Update()
    {
        if (!isPaused)
        {
            float t = daySpeed * Time.deltaTime;
            currentDayTime += t;
            time += t;
        }

        UpdateTimeText();
        UpdateArrowIndicator();

        if (currentDayTime >= duration)
            NextDay();
    }

    private void UpdateTimeText()
    {
        int hours = (int)currentDayTime / (60 * 60) + dayStartAt;
        int minutes = ((int)currentDayTime / 60) % 60;

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

    private void UpdateDayText()
    {
        int day = GetDay();
        dayText.text = $"{Enum.GetName(typeof(Days), day % 7).Substring(0, 3)}. {day}";
    }

    private void UpdateArrowIndicator()
    {
        Vector3 angle = new Vector3(0, 0, 90 - (currentDayTime % duration) * 180 / duration);
        arrowIndicator.transform.rotation = Quaternion.Euler(angle);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
    }

    public void NextDay()
    {
        time = ((int)time / duration) * duration;
        currentDayTime = 0;

        UpdateDayText();
    }

    public int GetDay() => ((int)time / duration) + 1;

    public float GetTime() => time;
}
