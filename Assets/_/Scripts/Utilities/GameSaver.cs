using System.IO;
using UnityEngine;

public static class GameSaver
{
    private static string saveFilePath = Path.Combine(Application.persistentDataPath, "saveScore.json");

    public static void SaveData(ScoreModel gameStateModel)
    {
        string json = JsonUtility.ToJson(gameStateModel);
        File.WriteAllText(saveFilePath, json);
    }

    public static ScoreModel LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            
            ScoreModel scoreModel =  JsonUtility.FromJson<ScoreModel>(json);
            return scoreModel;

        }
        else
        {
            return new ScoreModel();
        }
    }
}