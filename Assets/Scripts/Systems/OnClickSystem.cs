using UnityEngine;

public class OnClickSystem : MonoBehaviour
{
    private string cameraName = "Main Camera";

    [SerializeField] new Camera camera;

    private void Start()
    {
        FindCamera();
    }

    private void FindCamera()
    {
        if (!camera)
        {
            Camera camera = GameObject.Find(cameraName).GetComponent<Camera>();
            if (camera)
                this.camera = camera;
        }
    }

    private void Update()
    {
        if (camera)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log($"{hit.transform.name}");

                    OnClickComponent onClickComponent = hit.transform.GetComponent<OnClickComponent>();
                    if (onClickComponent)
                        onClickComponent.OnClick.Invoke();
                }
            }
        }
        else
        {
            FindCamera();
        }
    }
}