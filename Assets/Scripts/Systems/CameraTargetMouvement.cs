using UnityEngine;

public enum MouseButton
{
    LeftClick,
    RightClick,
    MiddleClick
}

public class CameraTargetMouvement : MonoBehaviour
{
    [SerializeField] MouseButton button;
    [SerializeField] float speed = 10f;
    public float radius = 2f;
    private Vector3 targetPos;
    private Vector3 initPos;
    private bool isMoving;

    private void SetTargetPosition()
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float point))
            targetPos = ray.GetPoint(point);

        isMoving = true;
    }

    private void MoveObject()
    {
        Vector3 v = Vector3.ClampMagnitude(targetPos - initPos, radius);
        targetPos = initPos + v;
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);

        if (transform.position == targetPos)
            isMoving = false;
    }

    private void Start()
    {
        initPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButton((int)button))
            SetTargetPosition();

        if (isMoving)
            MoveObject();
    }
}
