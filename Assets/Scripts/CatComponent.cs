using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatComponent : MonoBehaviour
{
    [SerializeField] Transform skin;
    [SerializeField] float scaleAmount;
    [SerializeField] float animationSpeed;

    [SerializeField] ParticleSystem ps;

    private Vector3 localScale;

    private void Start()
    {
        localScale = skin.localScale;
    }

    public void Play()
    {
        ps.Play();
        StartCoroutine(Scale());
    }

    private IEnumerator Scale()
    {
        float time = 0f;
        Vector3 desiredScale = localScale / (1f + scaleAmount);

        while (time < 1f)
        {
            skin.localScale = Vector3.Lerp(skin.localScale, desiredScale, time);
            time += Time.deltaTime * animationSpeed;
            yield return null;
        }

        time = 0;

        while (time < 1f)
        {
            skin.localScale = Vector3.Lerp(skin.localScale, localScale, time);
            time += Time.deltaTime * animationSpeed;
            yield return null;
        }
    }
}
