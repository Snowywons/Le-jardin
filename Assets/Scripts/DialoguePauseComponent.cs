using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePauseComponent : MonoBehaviour
{
    private ClockComponent clock;
    private OnClickSystem click;
    // Start is called before the first frame update
    void Start()
    {
        clock = FindObjectOfType<ClockComponent>();
        click = FindObjectOfType<OnClickSystem>();
    }

    public void SetClockPause(bool isPaused)
    {
        clock.SetPause(isPaused);
        click.enabled = !isPaused;
    }
}
