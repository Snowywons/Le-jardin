using System.Collections;
using UnityEngine;
using System.Linq;

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
    public Animator animator;

    private DiscountComponent discountComponent;

    private void Start()
    {
        initYRotation = transform.rotation.eulerAngles.y;
        GameSystem.Instance.Clock.eventsOnNextDay.AddListener(Appear);
        discountComponent = FindObjectOfType<DiscountComponent>();
        Appear();
    }

    private IEnumerator Walk()
    {
        isWalking = true;
        animator.SetBool("isWalking", true);
        yield return null;

        Vector3 to;

        if (direction.Equals(Direction.North))
        {
            to = endPoint.position;
            yRotation = 0;
            direction = Direction.South;
        }
        else
        {
            to = startPoint.position;
            yRotation = 180;
            direction = Direction.North;
        }

        to.y = transform.position.y;
        transform.localRotation = Quaternion.Euler(0, initYRotation + yRotation, 0);

        do
        {
            transform.position = Vector3.MoveTowards(transform.position, to, walkSpeed * Time.deltaTime);
            yield return null;
        }
        while (Vector3.Distance(transform.position, to) > 0.0001f);

        transform.position = to;

        isWalking = false;
        animator.SetBool("isWalking", false);
    }

    public void Appear()
    {
        int day = GameSystem.Instance.Clock.GetDay() - 1;
        var discount = discountComponent.discountsList.Where(d => d.day == day).FirstOrDefault();

        if (discount != null && discount.discountType.Equals(DiscountType.Goblin))
        {
            if (direction.Equals(Direction.North) && !isWalking)
                StartCoroutine(Walk());
        }
        else
        {
            if (direction.Equals(Direction.South) && !isWalking)
                StartCoroutine(Walk());
        }
    }
}
