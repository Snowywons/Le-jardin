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

    private SaveSystemComponent savesystem;

    private int duration;
    private float time;

    private bool isPaused;

    private bool ready;

    [HideInInspector] public UnityEvent eventsOnNextDay = new UnityEvent();

    private void Start()
    {
        savesystem = FindObjectOfType<SaveSystemComponent>();
        duration = (dayEndAt - dayStartAt) * (60 * 60);
        UpdateDayText();
    }

    private void Update()
    {
        if (isPaused || !ready) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            savesystem.currentDay = 31;
            NextDay();
            return;
        }

        float t = daySpeed * Time.deltaTime;
        savesystem.currentDayTime += t;
        time += t;

        if (savesystem.currentDayTime >= duration)
        {
            NextDay();
            return;
        }

        UpdateDayText();
        UpdateTimeText();
        UpdateArrowIndicator();
    }

    private void UpdateTimeText()
    {
        int hours = (int)savesystem.currentDayTime / (60 * 60) + dayStartAt;
        int minutes = ((int)savesystem.currentDayTime / 60) % 60;

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
        Vector3 angle = new Vector3(0, 0, 90 - (savesystem.currentDayTime % duration) * 180 / duration);
        arrowIndicator.transform.rotation = Quaternion.Euler(angle);
    }

    public void SetPause(bool pause)
    {
        isPaused = pause;
    }

    public void NextDay()
    {
        time = ++savesystem.currentDay * duration;
        savesystem.currentDayTime = 0;
        eventsOnNextDay?.Invoke();
        SetPause(true);
        if (savesystem.currentDay < 31) {
            Debug.Log("Load Warehouse");
            FindObjectOfType<SceneNavigatorComponent>().Load(SceneNavigatorComponent.WAREHOUSE); }
    }

    //public int GetDay() => duration > 0 ? ((int)time / duration) + 1 : (int)time;
    public int GetDay() => savesystem.currentDay;

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
