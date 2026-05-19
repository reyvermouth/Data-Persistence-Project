using UnityEngine;
using System.Collections.Generic;
using System.IO;

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

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/savefile.json";

    public static GameData LoadGame()
    {
        if (!File.Exists(path))
        {
            return new GameData();
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GameData>(json);
    }

    public static void SaveGame(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }
}
    

