using UnityEngine;

public enum MovementDirection { Horizontal, Vertical }

public class Enemy_Sideways : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private MovementDirection direction = MovementDirection.Horizontal;
    [SerializeField] private float movementDistance = 5f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private Color gizmoColor = Color.red;

    private bool movingBack;
    private float minEdge;
    private float maxEdge;

    private void Awake()
    {
        CalculateEdges();
    }

    private void CalculateEdges()
    {
        Vector3 pos = transform.position;
        if (direction == MovementDirection.Horizontal)
        {
            minEdge = pos.x - movementDistance;
            maxEdge = pos.x + movementDistance;
        }
        else
        {
            minEdge = pos.y - movementDistance;
            maxEdge = pos.y + movementDistance;
        }
    }

    private void Update()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        float currentPosition = direction == MovementDirection.Horizontal ? 
            transform.position.x : transform.position.y;

        if (movingBack)
        {
            if (currentPosition > minEdge)
            {
                Move(-1);
            }
            else
            {
                movingBack = false;
            }
        }
        else
        {
            if (currentPosition < maxEdge)
            {
                Move(1);
            }
            else
            {
                movingBack = true;
            }
        }
    }

    private void Move(int directionMultiplier)
    {
        Vector3 movement = direction == MovementDirection.Horizontal ?
            new Vector3(speed * directionMultiplier * Time.deltaTime, 0, 0) :
            new Vector3(0, speed * directionMultiplier * Time.deltaTime, 0);

        transform.position += movement;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Vector3 start, end;

        if (direction == MovementDirection.Horizontal)
        {
            start = transform.position + Vector3.left * movementDistance;
            end = transform.position + Vector3.right * movementDistance;
        }
        else
        {
            start = transform.position + Vector3.down * movementDistance;
            end = transform.position + Vector3.up * movementDistance;
        }

        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(start, 0.25f);
        Gizmos.DrawWireSphere(end, 0.25f);
    }
}