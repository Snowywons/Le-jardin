using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutComponent : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    public float fadeSpeed;

    private void Awake()
    {
        fadeImage.color = Color.black;
    }

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeIn()
    {
        while (fadeImage.color.a < 0.99f)
        {
            fadeImage.color = Color.Lerp(fadeImage.color, Color.black, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        fadeImage.color = Color.black;
    }

    public IEnumerator FadeOut()
    {
        while (fadeImage.color.a > 0.01f)
        {
            fadeImage.color = Color.Lerp(fadeImage.color, Color.clear, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        fadeImage.color = Color.clear;
    }
}
