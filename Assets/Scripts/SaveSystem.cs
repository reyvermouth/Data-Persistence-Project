using UnityEngine;
using System.IO;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;
    public Text bestScoreText;
    public InputField nameInputField;
    public string currentPlayerName;
    public int currenScore;
    GameData data = new GameData();
    private string path;

    void Start()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGameData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class PlayerRecord
    {
        public string playerName;
        public int highScore;
    }

    [System.Serializable]
    public class GameData
    {
        public List<PlayerRecord> playerRecords = new List<PlayerRecord>();
    }

    public int GetHighScoreByName(string name)
    {
        PlayerRecord record = data.playerRecords.Find(r => r.playerName.ToLower() == name.ToLower());
        return record != null ? record.highScore : 0;
    }    

    public void SaveHighScore(string name, int score)
    {
        PlayerRecord record = data.playerRecords.Find(r => r.playerName.ToLower() == name.ToLower());

        if (record != null)
        {
            if (score > record.highScore)
            {
                record.highScore = score;
            }
        }
        else
        {
            PlayerRecord newRecord = new PlayerRecord();
            newRecord.playerName = name;
            newRecord.highScore = score;
            data.playerRecords.Add(newRecord);
        }

        string json = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString("SaveData", json);
        PlayerPrefs.Save();
    }

    private void LoadGameData()
    {
        if (PlayerPrefs.HasKey("SaveData"))
        {
            string json = PlayerPrefs.GetString("SaveData");
            data = JsonUtility.FromJson<GameData>(json);
        }    
    }
    
}
