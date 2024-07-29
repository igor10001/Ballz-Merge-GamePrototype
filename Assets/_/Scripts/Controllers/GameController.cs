using System;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    public GameState CurrentState { get; private set; }
    private GridController _gridController;
    private GameUIController _gameUIController;
    private ProjectileLauncher _projectileLauncher;

    [Inject]
    public void Construct(GridController gridController, GameUIController gameUIController, ProjectileLauncher projectileLauncher)
    {
        _gridController = gridController;
        _gameUIController = gameUIController;
        _projectileLauncher = projectileLauncher;
    }

    private void Awake()
    {
        CurrentState = GameState.Gameplay;
        _gridController.OnBlocksRowMove += GridControllerOnOnBlocksRowMove;
        _gridController.eventAggregator.Subscribe<FirstBlockInRowZeroEvent>(OnFirstBlockInRowZeroEvent);
        _gameUIController.OnSpeedBoostBtnClick += GameUIControllerOnOnSpeedBoostBtnClick;
    }

    private void OnDestroy()
    {
        _gridController.OnBlocksRowMove -= GridControllerOnOnBlocksRowMove;
        _gameUIController.OnSpeedBoostBtnClick -= GameUIControllerOnOnSpeedBoostBtnClick;

    }

    private void GameUIControllerOnOnSpeedBoostBtnClick(object sender, EventArgs e)
    {
        _projectileLauncher.CurrentBall.MoveSpeed *= 5;
    }

    private void GridControllerOnOnBlocksRowMove(object sender, EventArgs e)
    {
        _gameUIController.IncrementMovesCount();
    }

    private void OnFirstBlockInRowZeroEvent(FirstBlockInRowZeroEvent e)
    {
        OnGameOver();
    }

    private void OnGameOver()
    {
        
        CurrentState = GameState.GameOver;
        _projectileLauncher.SetGameState(GameState.GameOver);
        _gameUIController.ShowGameOver(); 
        _gameUIController.SetHighScore();
    }
    
}