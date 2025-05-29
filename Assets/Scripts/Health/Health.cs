using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private bool dead;

    [Header("Effects")]
    [SerializeField] private float iFramesDuration = 1f;
    [SerializeField] private int numberOfFlashes = 3;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;
    
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        currentHealth = startingHealth;
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            StartCoroutine(DamageEffect());
            SoundManager.Instance.PlaySound(hurtSound);
        }
        else if (!dead)
        {
            Die();
        }
    }

    private void Die()
    {
        dead = true;
        SoundManager.Instance.PlaySound(deathSound);
        StartCoroutine(RespawnAfterDeath());
    }

    private IEnumerator RespawnAfterDeath()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<PlayerRespawn>().RespawnPlayer();
    }

    public void Respawn()
    {
        dead = false;
        gameObject.SetActive(true);
        AddHealth(startingHealth);
        StartCoroutine(DamageEffect());
    }

    private IEnumerator DamageEffect()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration/(numberOfFlashes*2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration/(numberOfFlashes*2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
    }
}