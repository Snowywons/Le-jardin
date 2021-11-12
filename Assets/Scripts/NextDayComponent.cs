using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextDayComponent : MonoBehaviour
{
    public void NextDay()
    {
        GameSystem.Instance.Clock.NextDay();
    }
}
