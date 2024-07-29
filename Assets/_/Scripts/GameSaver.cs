using System.IO;
using UnityEngine;

public static class GameSaver
{
    private static string saveFilePath = Path.Combine(Application.persistentDataPath, "saveScore.json");

    public static void SaveData(ScoreModel gameStateModel)
    {
        Debug.Log(gameStateModel.HighScore);
        string json = JsonUtility.ToJson(gameStateModel);
        File.WriteAllText(saveFilePath, json);
    }

    public static ScoreModel LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<ScoreModel>(json);
        }
        else
        {
            return new ScoreModel();
        }
    }
}