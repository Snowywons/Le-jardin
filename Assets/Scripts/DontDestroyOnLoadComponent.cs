using UnityEngine;

public class DontDestroyOnLoadComponent : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        GameObject obj = GameObject.Find(gameObject.name);

        if (obj && obj != gameObject)
        {
            Debug.Log($"{gameObject.name} destroyed");
            Destroy(gameObject);
        }
    }

    public void ForceDestroy()
    {
        Destroy(gameObject);
    }
}
