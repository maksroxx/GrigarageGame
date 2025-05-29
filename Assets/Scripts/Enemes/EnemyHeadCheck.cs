using UnityEngine;

public class EnemyHeadCheck : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float bounceForce = 10f;
    [SerializeField] private bool destroyParent = true;
    [SerializeField] private bool destroySelf = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if(playerRb != null)
            {
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
            }
            else
            {
                Debug.LogWarning("У игрока нет Rigidbody2D!");
            }

            if(destroyParent && transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            if(destroySelf)
            {
                Destroy(gameObject);
            }
        }
    }
}
