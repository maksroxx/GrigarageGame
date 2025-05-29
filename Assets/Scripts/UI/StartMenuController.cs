using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button highscoreButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button multButton;
    // [SerializeField] private GameObject scoreboard;
    // [SerializeField] private Button closeScoreboardButton;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        exitButton.onClick.AddListener(OnExitClick);
        highscoreButton.onClick.AddListener(OnHighscoreClick);
        multButton.onClick.AddListener(OnMultipleerClick);
        // closeScoreboardButton.onClick.AddListener(OnCloseScoreboard);

        // scoreboard.SetActive(false);
    }

    private void OnPlayButtonClick()
    {
        SceneManager.LoadScene(1);
    }

    private void OnMultipleerClick()
    {
        SceneManager.LoadScene(2);
    }

    // Открытие панели рекордов
    private void OnHighscoreClick()
    {
        // scoreboard.SetActive(true);
    }

    private void OnCloseScoreboard()
    {
        // scoreboard.SetActive(false);
    }

    private void OnExitClick()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}

// using UnityEngine;
// using UnityEngine.SceneManagement;
// using TMPro;
// using UnityEngine.UI;

// public class StartMenuController : MonoBehaviour
// {
//     [Header("UI Elements")]
//     [SerializeField] private Button playButton;
//     [SerializeField] private Button highscoreButton;
//     [SerializeField] private Button exitButton;
//     [SerializeField] private GameObject nameInputPanel;
//     [SerializeField] private TMP_InputField nameInputField;
//     [SerializeField] private Button startGameButton;
//     [SerializeField] private Button closeNamePanelButton;
//     [SerializeField] private GameObject scoreboard;

//     [Header("Settings")]
//     [SerializeField] private string[] defaultNames = { "Warrior", "Hero", "Champion" };

//     private void Start()
//     {
//         InitializeButtons();
//         LoadPlayerName();
//         scoreboard.gameObject.SetActive(false);
//         nameInputPanel.SetActive(false);
//     }

//     private void InitializeButtons()
//     {
//         playButton.onClick.AddListener(OnPlayButtonClick);
//         highscoreButton.onClick.AddListener(OnHighscoreClick);
//         exitButton.onClick.AddListener(OnExitClick);
//         startGameButton.onClick.AddListener(OnStartGameClick);
//         closeNamePanelButton.onClick.AddListener(OnCloseNamePanel);
//     }

//     private void LoadPlayerName()
//     {
//         if (PlayerPrefs.HasKey("PlayerName"))
//             nameInputField.text = PlayerPrefs.GetString("PlayerName");
//     }

//     private void OnPlayButtonClick() => ShowNameInputPanel();
//     private void OnHighscoreClick() => scoreboard.gameObject.SetActive(true);
//     private void OnExitClick() => QuitGame();

//     private void OnStartGameClick()
//     {
//         SetPlayerName();
//         SavePlayerName();
//         SceneManager.LoadScene(1);
//     }

//     private void ShowNameInputPanel()
//     {
//         nameInputPanel.SetActive(true);
//         ToggleMainButtons(false);
//     }

//     private void SetPlayerName()
//     {
//         string name = string.IsNullOrWhiteSpace(nameInputField.text) 
//             ? GetRandomDefaultName() 
//             : nameInputField.text.Trim();
        
//         GameManager.Instance.SetPlayerName(name);
//     }

//     private string GetRandomDefaultName() => defaultNames[Random.Range(0, defaultNames.Length)];

//     private void SavePlayerName() => PlayerPrefs.SetString("PlayerName", GameManager.Instance.PlayerName);

//     private void OnCloseNamePanel()
//     {
//         nameInputPanel.SetActive(false);
//         ToggleMainButtons(true);
//     }

//     private void ToggleMainButtons(bool state)
//     {
//         playButton.gameObject.SetActive(state);
//         highscoreButton.gameObject.SetActive(state);
//         exitButton.gameObject.SetActive(state);
//     }

//     private void QuitGame()
//     {
//         #if UNITY_EDITOR
//         UnityEditor.EditorApplication.isPlaying = false;
//         #else
//         Application.Quit();
//         #endif
//     }
// }