using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Events;

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

    [Header("References")]
    [SerializeField] string timeTextName;
    [SerializeField] string dayTextName;
    [SerializeField] string arrowIndicatorName;

    private Text timeText;
    private Text dayText;
    private Image arrowIndicator;

    private int duration;
    private float time;
    private float currentDayTime;

    private bool isPaused;

    private bool ready;

    [HideInInspector] public UnityEvent eventsOnNextDay = new UnityEvent();

    private void Start()
    {
        duration = (dayEndAt - dayStartAt) * (60 * 60);
        UpdateDayText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            NextDay();
            return;
        }

        if (!isPaused)
        {
            float t = daySpeed * Time.deltaTime;
            currentDayTime += t;
            time += t;
        }

        if (ready == false)
            return;

        UpdateDayText();
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
        if (day > 31) return;

        dayText.text = $"{Enum.GetName(typeof(Days), (day - 1) % 7).Substring(0, 3)}. {day}";
    }

    private void UpdateArrowIndicator()
    {
        Vector3 angle = new Vector3(0, 0, 90 - (currentDayTime % duration) * 180 / duration);
        arrowIndicator.transform.rotation = Quaternion.Euler(angle);
    }

    public void SetPause(bool pause)
    {
        isPaused = pause;
    }

    public void NextDay()
    {
        time = GetDay() * duration;
        currentDayTime = 0;
        eventsOnNextDay?.Invoke();

        UpdateDayText();
    }

    public int GetDay() => duration > 0 ? ((int)time / duration) + 1 : (int)time;

    public float GetTime() => time;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        ready = FindReferences();
    }

    private bool FindReferences()
    {
        GameObject obj = GameObject.Find(timeTextName);
        timeText = obj ? obj.GetComponent<Text>() : null;

        obj = GameObject.Find(dayTextName);
        dayText = obj ? obj.GetComponent<Text>() : null;

        obj = GameObject.Find(arrowIndicatorName);
        arrowIndicator = obj ? obj.GetComponent<Image>() : null;

        return timeText && dayText && arrowIndicator;
    }
}
