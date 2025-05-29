using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float fallDelay = 1f;    // Задержка перед падением
    [SerializeField] private float destroyDelay = 2f; // Время до уничтожения
    [SerializeField] private float shakePower = 0.1f; // Сила тряски

    private Rigidbody2D rb;
    private bool isFalling = false;
    private Vector3 initialPosition;
    private bool isShaking = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.isKinematic = true;
        }
        initialPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Player"))
        {
            if (collision.relativeVelocity.y <= 0)
            {
                StartCoroutine(ShakeAndFall());
            }
        }
    }

    private IEnumerator ShakeAndFall()
    {
        isFalling = true;
        isShaking = true;
        
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < fallDelay)
        {
            if (isShaking)
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-shakePower, shakePower),
                    Random.Range(-shakePower, shakePower),
                    0
                );
                transform.position = startPosition + randomOffset;
            }
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition;
        isShaking = false;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.gravityScale = 1;
        }
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    private void ResetPlatform()
    {
        StopAllCoroutines();
        transform.position = initialPosition;
        isFalling = false;
        isShaking = false;
        
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
            rb.gravityScale = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, GetComponent<Collider2D>().bounds.size);
    }
}