using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Direction
{
    North,
    South
}

public class GoblinMovementComponent : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] float walkSpeed = 1f;

    private Direction direction;
    private bool isWalking;
    private float yRotation;
    private float initYRotation;

    private void Start()
    {
        initYRotation = transform.rotation.eulerAngles.y;
    }

    private IEnumerator Walk()
    {
        isWalking = true;

        float time = 0;

        Vector3 from, to;

        if (direction.Equals(Direction.North))
        {
            from = startPoint.position;
            to = endPoint.position;
            yRotation = 0;
            direction = Direction.South;
        }
        else
        {
            from = endPoint.position;
            to = startPoint.position;
            yRotation = 180;
            direction = Direction.North;
        }

        from.y = transform.position.y;
        to.y = transform.position.y;
        transform.localRotation = Quaternion.Euler(0, initYRotation + yRotation, 0);

        while (time < 1)
        {
            transform.position = Vector3.Lerp(from, to, time);
            time += Time.deltaTime * walkSpeed;
            yield return null;
        }

        isWalking = false;
    }

    public void ToggleAppear()
    {
        if (!isWalking)
            StartCoroutine(Walk());
    }
}
