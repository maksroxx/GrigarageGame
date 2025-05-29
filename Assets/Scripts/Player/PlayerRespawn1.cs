using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class PlayerRespawn1 : NetworkBehaviour
{
    [Header("Respawn Settings")]
    [SerializeField] private AudioClip checkpointSound;
    [SerializeField] private float finalScreenDelay = 1f;
    
    [Header("Network Settings")]
    [SerializeField] private LayerMask checkpointLayer;
    
    private NetworkVariable<Vector3> currentCheckpoint = new NetworkVariable<Vector3>();
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
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            currentCheckpoint.Value = transform.position;
        }
    }

     [ServerRpc(RequireOwnership = false)]
    public void RespawnPlayerServerRpc() // Добавлен недостающий метод
    {
        if (!IsServer) return;

        // Телепорт игрока к чекпоинту
        TeleportPlayerClientRpc(currentCheckpoint.Value);
        
        // Сброс здоровья
        if (TryGetComponent<Health>(out var health))
        {
            health.RespawnServerRpc();
        }

    //     // Синхронизация UI
    //     ResetPlayerUIClientRpc();
    // }

    // [ServerRpc(RequireOwnership = false)]
    // public void RespawnPlayerServerRpc()
    // {
    //     if (!IsServer) return;
        
    //     // Сброс состояния игрока
    //     playerHealth.RespawnServerRpc();
    //     TeleportPlayerClientRpc(currentCheckpoint.Value);
        
    //     // Синхронизация UI
    //     ResetPlayerUIClientRpc();
    // }

    [ClientRpc]
    public void TeleportPlayerClientRpc(Vector3 position)
    {
        transform.position = position;
        rb.linearVelocity = Vector2.zero;
    }

    [ClientRpc]
    public void ResetPlayerUIClientRpc()
    {
        if (IsOwner)
        {
            uiManager.ResetHealthDisplay();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsOwner) return;

        if (checkpointLayer == (checkpointLayer | (1 << collision.gameObject.layer)))
        {
            if (collision.CompareTag("Checkpoint"))
            {
                SetCheckpointServerRpc(collision.transform.position);
                PlayCheckpointSoundClientRpc();
            }
            else if (collision.CompareTag("Final"))
            {
                HandleFinalCheckpointServerRpc();
            }
        }
    }

    [ServerRpc]
    public void SetCheckpointServerRpc(Vector3 position)
    {
        currentCheckpoint.Value = position;
        DisableCheckpointClientRpc(position);
    }

    [ClientRpc]
    public void PlayCheckpointSoundClientRpc()
    {
        SoundManager.Instance.PlaySound(checkpointSound);
    }

    [ClientRpc]
    public void DisableCheckpointClientRpc(Vector3 position)
    {
        var checkpoints = FindObjectsOfType<Checkpoint>();
        foreach (var checkpoint in checkpoints)
        {
            if (Vector3.Distance(checkpoint.transform.position, position) < 0.1f)
            {
                checkpoint.Disable();
            }
        }
    }

    [ServerRpc]
    public void HandleFinalCheckpointServerRpc()
    {
        if (!IsServer) return;
        
        ShowFinalScreenClientRpc();
        StartCoroutine(EndGameCoroutine());
    }

    [ClientRpc]
    public void ShowFinalScreenClientRpc()
    {
        if (IsOwner)
        {
            playerMovement.enabled = false;
            StartCoroutine(ShowFinalScreenAfterDelay());
        }
    }

    private IEnumerator ShowFinalScreenAfterDelay()
    {
        yield return new WaitForSecondsRealtime(finalScreenDelay);
        uiManager.ShowFinalScreen();
    }

    [ServerRpc]
    public IEnumerator EndGameCoroutine()
    {
        yield return new WaitForSeconds(finalScreenDelay + 1f);
        NetworkManager.Singleton.Shutdown();
        LoadLobbyClientRpc();
    }

    [ClientRpc]
    public void LoadLobbyClientRpc()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }
}
}