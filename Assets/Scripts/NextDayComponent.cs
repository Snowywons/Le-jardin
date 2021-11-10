using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextDayComponent : MonoBehaviour
{
    private bool isLoading;
    private bool isFading;

    public void NextDay()
    {
        if (isLoading) return;

        GameSystem.Instance.Clock.NextDay();
        FindObjectOfType<SceneNavigatorComponent>().Load(1);
    }
}
