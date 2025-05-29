using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float maxJumpHold = 0.3f;
    [SerializeField] private int maxAirJumps = 1;

    [Header("Key Bindings")]
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Fall Settings")]
    [SerializeField] private float deathYLevel = -10f;
    [SerializeField] private Color gizmoColor = Color.red;
    [SerializeField] private float gizmoLineLength = 100f;

    private Rigidbody2D rb;
    private float jumpHoldCounter;
    private bool isJumping;
    private int availableJumps;
    private PlayerRespawn respawn;

    private void Awake()
    {   
        rb = GetComponent<Rigidbody2D>();
        respawn = GetComponent<PlayerRespawn>();
        availableJumps = maxAirJumps + 1;
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        CheckFallDeath();
    }

    private void CheckFallDeath()
    {
        if(transform.position.y < deathYLevel)
        {
            respawn.RespawnPlayer();
        }
    }

    private void HandleMovement()
    {
        float horizontal = 0f;
        if (Input.GetKey(rightKey)) horizontal += 1f;
        if (Input.GetKey(leftKey)) horizontal -= 1f;
        
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        
        if (horizontal > 0.01f)
            transform.localScale = new Vector3(5, 5, 5);
        else if (horizontal < -0.01f)
            transform.localScale = new Vector3(-5, 5, 5);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(jumpKey) && availableJumps > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;
            jumpHoldCounter = maxJumpHold;
            availableJumps--;
        }

        if (Input.GetKey(jumpKey) && isJumping)
        {
            if(jumpHoldCounter > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpHoldCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(jumpKey))
        {
            isJumping = false;
        }

        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            availableJumps = maxAirJumps + 1;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Vector3 start = new Vector3(-gizmoLineLength, deathYLevel, 0);
        Vector3 end = new Vector3(gizmoLineLength, deathYLevel, 0);
        Gizmos.DrawLine(start, end);

// #if UNITY_EDITOR
//         GUIStyle style = new GUIStyle();
//         style.normal.textColor = gizmoColor;
//         Handles.Label(
//             new Vector3(0, deathYLevel + 0.5f, 0), 
//             " Death Level", 
//             style
//         );
// #endif
    }
}