using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum LevelName
{
    MENU,
    WORLD,
    WAREHOUSE
}

public class SceneNavigatorComponent : MonoBehaviour
{
    public const int MENU = (int)LevelName.MENU;
    public const int WORLD = (int)LevelName.WORLD;
    public const int WAREHOUSE = (int)LevelName.WAREHOUSE;

    private bool isLoading;

    public void Load(int sceneId)
    {
        FindObjectOfType<SaveSystemComponent>().Save();
        StartCoroutine(ELoad(sceneId));
    }

    private IEnumerator ELoad(int sceneId)
    {
        yield return null;

        if (!isLoading)
        {
            isLoading = true;

            FadeInOutComponent fadeInOutComponent = FindObjectOfType<FadeInOutComponent>();
            fadeInOutComponent.StopAllCoroutines();
            StartCoroutine(fadeInOutComponent.FadeIn());
            yield return new WaitForSeconds(0.6f);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }

            isLoading = false;
        }
    }

    public int GetActiveSceneIndex() => SceneManager.GetActiveScene().buildIndex;
    public string GetActiveSceneName() => SceneManager.GetActiveScene().name;
}
