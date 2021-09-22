using UnityEngine;
using UnityEngine.SceneManagement;
public class GuiComponent : MonoBehaviour
{
    [SerializeField] string cameraName;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += AssignCamera;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= AssignCamera;
    }

    private void AssignCamera(Scene scene, LoadSceneMode mode)
    {
        Canvas canvas = GetComponent<Canvas>();
        GameObject camera = GameObject.Find(cameraName);

        if (canvas && camera)
            canvas.worldCamera = camera.GetComponent<Camera>();
    }
}
