using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreComponent : MonoBehaviour
{
    [SerializeField] Text scorePointsText;

    private void OnEnable()
    {
        scorePointsText.text = FindObjectOfType<SaveSystemComponent>().scorePoints.ToString();
    }
}
