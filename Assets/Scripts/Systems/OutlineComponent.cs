using UnityEngine;

public class OutlineComponent : MonoBehaviour
{
    [SerializeField] GameObject outline;

    public void Activate()
    {
        outline.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        outline.gameObject.SetActive(false);
    }
}
