using UnityEngine;

public class GameUIController : MonoBehaviour
{
    public GameUIView gameUIView;
    private ScoreModel scoreModel;

    private void Start()
    {
        LoadData();
        UpdateUI();
    }

    private void UpdateUI()
    {
        gameUIView.UpdateScore(scoreModel.HighScore);
        gameUIView.UpdateMovesCount(scoreModel.MovesCount);
    }

    public void LoadData()
    {
        scoreModel = GameSaver.LoadData();
    }

    public void SaveData()
    {
        GameSaver.SaveData(scoreModel);
    }
    
    public void SetHighScore(int newHighScore)
    {
        if (newHighScore > scoreModel.HighScore)
        {
            scoreModel.HighScore = newHighScore;
            gameUIView.UpdateScore(scoreModel.HighScore);
        }
    }

    public void IncrementMovesCount()
    {
        scoreModel.MovesCount++;
        gameUIView.UpdateMovesCount(scoreModel.MovesCount);
    }

    public void SetMovesCount(int count)
    {
        scoreModel.MovesCount = count;
        gameUIView.UpdateMovesCount(scoreModel.MovesCount);
    }
}