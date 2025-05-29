// using UnityEngine;
// using System.Collections.Generic;
// using System.IO;

// public class Scoreboard : MonoBehaviour
// {
//     public static Scoreboard Instance { get; private set; }

//     [Header("Settings")]
//     [SerializeField] private int maxEntries = 7;

//     private List<ScoreEntry> scores = new List<ScoreEntry>();
//     private string savePath;

//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             savePath = Path.Combine(Application.persistentDataPath, "scores.json");
//             LoadScores();
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     // Добавление одиночной записи
//     public void AddPendingScore(ScoreEntry newScore)
//     {
//         scores.Add(newScore);
//         ProcessAndSave();
//     }

//     // Добавление списка записей
//     public void AddPendingScores(List<ScoreEntry> newScores)
//     {
//         scores.AddRange(newScores);
//         ProcessAndSave();
//     }

//     // Общая обработка и сохранение
//     private void ProcessAndSave()
//     {
//         // Сортировка по убыванию
//         scores.Sort((a, b) => b.score.CompareTo(a.score));
        
//         // Обрезка списка
//         if(scores.Count > maxEntries)
//             scores = scores.GetRange(0, maxEntries);
        
//         // Сохранение в файл
//         SaveToFile();
//     }

//     private void SaveToFile()
//     {
//         ScoreWrapper wrapper = new ScoreWrapper { scores = scores };
//         File.WriteAllText(savePath, JsonUtility.ToJson(wrapper));
//     }

//     public void LoadScores()
//     {
//         if (File.Exists(savePath))
//         {
//             string json = File.ReadAllText(savePath);
//             scores = JsonUtility.FromJson<ScoreWrapper>(json).scores;
//         }
//     }

//     [System.Serializable]
//     private class ScoreWrapper
//     {
//         public List<ScoreEntry> scores;
//     }
// }

// [System.Serializable]
// public class ScoreEntry
// {
//     public string playerName;
//     public int score;
// }