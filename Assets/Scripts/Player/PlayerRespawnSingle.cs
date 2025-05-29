using UnityEngine;
using System.Collections;

public class PlayerRespawnSingle : MonoBehaviour
{
    [SerializeField] private AudioClip checkpoint;
    [SerializeField] private float finalDelay = 1f;
    
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        currentCheckpoint = null;
    }

    public void RespawnPlayer()
    {
        if(currentCheckpoint == null) 
        {
            if(playerMovement != null) 
                playerMovement.enabled = false;
            
            if(rb != null) 
            {
                rb.linearVelocity = Vector2.zero;
                rb.simulated = false;
            }
            
            uiManager.GameOver();
            return;
        }
        
        if(playerMovement != null) 
            playerMovement.enabled = true;
            
        if(rb != null)
            rb.simulated = true;
            
        playerHealth.Respawn(); 
        transform.position = currentCheckpoint.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            SetNewCheckpoint(collision.transform);
            SoundManager.Instance.PlaySound(checkpoint);
            DisableCheckpoint(collision);
        }
        if (collision.CompareTag("Final"))
        {
            FinalSheck(collision);
        }
    }

    private void SetNewCheckpoint(Transform newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }

    private void DisableCheckpoint(Collider2D checkpoint)
    {
        checkpoint.GetComponent<Collider2D>().enabled = false;
    }

    private void FinalSheck(Collider2D checkpoint)
    {
        checkpoint.GetComponent<Collider2D>().enabled = false;
        
        if(checkpoint.TryGetComponent<Animator>(out var anim))
            anim.SetTrigger("appear");
        
        if(playerMovement != null)
            playerMovement.enabled = false;
        
        StartCoroutine(ShowFinalScreenAfterDelay());
    }

    private IEnumerator ShowFinalScreenAfterDelay()
    {
        yield return new WaitForSecondsRealtime(finalDelay);
        uiManager.ShowFinalScreen();
    }
}