using UnityEngine;

public class CameraZoomControl : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float speed = 10f;
    [SerializeField] float maxZoomIn;
    [SerializeField] float maxZoomOut;
    [SerializeField] CameraTargetMouvement target;
    
    private float xAdjust;
    private float zAdjust;
    private float initRadius;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        xAdjust = transform.position.x - target.transform.position.x;
        zAdjust = transform.position.z - target.transform.position.z;
        initRadius = target.radius;
    }

    void Update()
    {
        float zoomValue = cam.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * speed;
        target.radius = maxZoomOut / zoomValue * initRadius;

        if (zoomValue >= maxZoomIn && zoomValue <= maxZoomOut)
        {
            cam.orthographicSize = zoomValue;
            cam.transform.position = 
                new Vector3(target.transform.position.x + xAdjust, transform.position.y, target.transform.position.z + zAdjust);
        }
    }
}
