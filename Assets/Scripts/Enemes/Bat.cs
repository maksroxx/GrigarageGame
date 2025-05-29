using UnityEngine;

public class Bat : EnemyDamage
{
    [Header("Bat Attributes")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float detectionRadius = 7f;
    [SerializeField] private float smoothTime = 0.2f;
    
    private Transform player;
    private Vector2 currentVelocity;
    private bool isChasing;
    private bool hasTouchedPlayer;
    private Vector3 originalScale;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalScale = transform.localScale;
        hasTouchedPlayer = false;
    }

    private void Update()
    {
        if (player == null || hasTouchedPlayer) return;

        CheckForPlayer();
        HandleMovement();
        UpdateFacingDirection();
    }

    private void CheckForPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        isChasing = distance <= detectionRadius;
    }

    private void HandleMovement()
    {
        if (isChasing)
        {
            Vector2 targetPos = player.position;
            Vector2 newPos = Vector2.SmoothDamp(
                transform.position,
                targetPos,
                ref currentVelocity,
                smoothTime,
                moveSpeed
            );
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        }
    }

    private void UpdateFacingDirection()
    {
        if (currentVelocity.x > 0.1f)
        {
            transform.localScale = new Vector3(
                -originalScale.x,
                originalScale.y,
                originalScale.z
            );
        }
        else if (currentVelocity.x < -0.1f)
        {
            transform.localScale = new Vector3(
                originalScale.x,
                originalScale.y,
                originalScale.z
            );
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if(collision.CompareTag("Player"))
        {
            hasTouchedPlayer = true;
            currentVelocity = Vector2.zero;
            isChasing = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = hasTouchedPlayer ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}