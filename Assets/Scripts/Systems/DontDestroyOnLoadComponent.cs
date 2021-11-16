using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DontDestroyOnLoadComponent : MonoBehaviour
{
    public List<LevelName> excludedLevelNames = new List<LevelName>();
    public bool destroyOld;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        GameObject obj = GameObject.Find(gameObject.name);

        if (obj && obj != gameObject)
        {
            if (destroyOld)
                Destroy(obj);
            else
                Destroy(gameObject);
        }
    }

    public void ForceDestroy()
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (excludedLevelNames.Exists((n) => (int) n == scene.buildIndex))
            ForceDestroy();
    }
}
