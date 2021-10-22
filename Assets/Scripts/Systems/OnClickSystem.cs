using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickSystem : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] string cameraName = "Main Camera";
    public bool enabled = true;

    private void Start()
    {
        FindCamera();
    }

    private void FindCamera()
    {
        if (!cam)
            cam = GameObject.Find(cameraName).GetComponent<Camera>();
    }

    private void Update()
    {
        if (cam && enabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);
                                
                foreach(RaycastHit hit in hits)
                { 
                    OnClickComponent onClickComponent = hit.transform.GetComponent<OnClickComponent>();
                    if (onClickComponent)
                    {
                        onClickComponent.OnClick.Invoke();
                        break;
                    }
                }
            }
        }
        else
        {
            FindCamera();
        }
    }
}
