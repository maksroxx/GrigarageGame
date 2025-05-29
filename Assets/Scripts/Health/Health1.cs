using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class Health1 : NetworkBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float startingHealth = 100f;
    public NetworkVariable<float> currentHealth = new NetworkVariable<float>();
    
    [Header("Damage Effects")]
    [SerializeField] private float iFramesDuration = 1f;
    [SerializeField] private int numberOfFlashes = 3;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    [Header("Collision Settings")]
    [SerializeField] private LayerMask damageLayer;
    
    private SpriteRenderer spriteRend;
    private bool isDead;
    private PlayerRespawn respawn;

    private void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        respawn = GetComponent<PlayerRespawn>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            currentHealth.Value = startingHealth;
            isDead = false;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(float damage, ServerRpcParams rpcParams = default)
    {
        if (isDead || !IsServer) return;

        currentHealth.Value = Mathf.Clamp(currentHealth.Value - damage, 0, startingHealth);
        
        if (currentHealth.Value <= 0)
        {
            DieClientRpc();
        }
        else
        {
            DamageEffectClientRpc();
        }
    }

    [ClientRpc]
    private void DamageEffectClientRpc()
    {
        StartCoroutine(InvulnerabilityEffect());
        SoundManager.Instance.PlaySound(hurtSound);
    }

    [ClientRpc]
    private void DieClientRpc()
    {
        if (IsOwner)
        {
            SoundManager.Instance.PlaySound(deathSound);
            StartCoroutine(RespawnAfterDeath());
        }
    }

    private IEnumerator InvulnerabilityEffect()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, damageLayer, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(gameObject.layer, damageLayer, false);
    }

    private IEnumerator RespawnAfterDeath()
    {
        isDead = true;
        yield return new WaitForSeconds(1f);
        respawn.RespawnPlayerServerRpc();
    }

    [ServerRpc]
    public void RespawnServerRpc()
    {
        if (!IsServer) return;
        
        currentHealth.Value = startingHealth;
        isDead = false;
        RespawnClientRpc();
    }

    [ClientRpc]
    private void RespawnClientRpc()
    {
        spriteRend.color = Color.white;
        Physics2D.IgnoreLayerCollision(gameObject.layer, damageLayer, false);
        StartCoroutine(InvulnerabilityEffect());
    }

    public void Heal(float amount)
    {
        if (!IsServer) return;
        currentHealth.Value = Mathf.Clamp(currentHealth.Value + amount, 0, startingHealth);
    }
}