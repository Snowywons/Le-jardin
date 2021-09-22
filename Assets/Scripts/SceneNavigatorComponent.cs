using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneNavigatorComponent : MonoBehaviour
{
    public const int WORLD = 0;
    public const int WAREHOUSE = 1;

    public void Load(int sceneId)
    {
        StartCoroutine(ELoad(sceneId));
    }

    private IEnumerator ELoad(int sceneId)
    {
        yield return null;

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
    }

    public int GetActiveSceneIndex() => SceneManager.GetActiveScene().buildIndex;
    public string GetActiveSceneName() => SceneManager.GetActiveScene().name;
}
