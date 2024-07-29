using UnityEngine;
using UnityEngine.UI;
using System;

public class GameUIController : MonoBehaviour
{
    public GameUIView gameUIView;
    private ScoreModel scoreModel;
    [SerializeField] private Button speedBoostBtn;

    public event EventHandler OnSpeedBoostBtnClick;
    private void Start()
    {
        speedBoostBtn.onClick.AddListener(() =>{OnSpeedBoostBtnClick?.Invoke(this, EventArgs.Empty); speedBoostBtn.onClick.RemoveAllListeners();});
        LoadHighScore();
        UpdateUI();
    }

    private void UpdateUI()
    {
        gameUIView.UpdateScore(scoreModel.HighScore);
        gameUIView.UpdateMovesCount(scoreModel.MovesCount);
    }

    public void LoadHighScore()
    {
        scoreModel = GameSaver.LoadData();
    }

    public void SaveHighScore()
    {
        GameSaver.SaveData(new ScoreModel { HighScore = scoreModel.HighScore });
    }

    public void SetHighScore()
    {
        if (scoreModel.MovesCount > scoreModel.HighScore)
        {
            scoreModel.HighScore = scoreModel.MovesCount;
            gameUIView.UpdateScore(scoreModel.HighScore);
            SaveHighScore();
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

    public void ShowGameOver()
    {
        gameUIView.ShowGameOver();
    }
}
