using System;
using UnityEngine;
using Zenject;


public class GameController : MonoBehaviour
{
    public GameState CurrentState { get; private set; }
    private  GridController _gridController;
    private  GameUIController _gameUIController;
    [Inject]
    public void Construct(GridController _gridController, GameUIController _gameUIController)
    {
        this._gridController = _gridController;
        this._gameUIController = _gameUIController;
    }

    private void Awake()
    {
        CurrentState = GameState.Gameplay;
        _gridController.OnBlocksRowMove +=  GridControllerOnOnBlocksRowMove;
        _gridController.eventAggregator.Subscribe<FirstBlockInRowZeroEvent>(OnFirstBlockInRowZeroEvent);
    }

    private void GridControllerOnOnBlocksRowMove(object sender, EventArgs e)
    {
        _gameUIController.IncrementMovesCount();
    }
    private void OnFirstBlockInRowZeroEvent(FirstBlockInRowZeroEvent e)
    {
        CurrentState = GameState.GameOver;
    }
}

