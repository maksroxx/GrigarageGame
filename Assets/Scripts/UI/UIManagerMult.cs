using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManagerMult : MonoBehaviour
{
    [Header("UI Screens")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject finalScreen;

    [Header("UI Settings")]
    [SerializeField] private string backgroundTag = "Split";

    [Header("Settings")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private float returnToMenuDelay = 2f;

    private bool isPaused = false;
    private bool isDead = false;
    private bool isSplit = true;

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey))
            TogglePause();
    }

    public void ShowFinalScreen()
    {
        if (isDead) return;
        
        isDead = true;
        isSplit = false;
        finalScreen.SetActive(true);
        GameObject[] backgroundObjects = GameObject.FindGameObjectsWithTag(backgroundTag);
        foreach(GameObject bgObject in backgroundObjects)
        {
            bgObject.SetActive(isSplit);
        }
        Time.timeScale = 0f;
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSecondsRealtime(returnToMenuDelay);
        SceneManager.LoadScene(2);
    }

    public void TogglePause()
    {
        if (isDead) return;
        
        isPaused = !isPaused;
        isSplit = !isSplit;
        GameObject[] backgroundObjects = GameObject.FindGameObjectsWithTag(backgroundTag);
        foreach(GameObject bgObject in backgroundObjects)
        {
            bgObject.SetActive(isSplit);
        }
        pauseScreen.SetActive(isPaused);
        
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void GameOver()
    {
        isDead = true;

        GameObject[] backgroundObjects = GameObject.FindGameObjectsWithTag(backgroundTag);
        foreach(GameObject bgObject in backgroundObjects)
        {
            bgObject.SetActive(false);
        }

        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        // Для работы в редакторе
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}