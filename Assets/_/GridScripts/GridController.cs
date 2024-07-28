using System;
using UnityEngine;
using Zenject;

public class GridController : MonoBehaviour
{
    public int width = 10; 
    public int height = 10; 
    public Vector2 cellSize = new Vector2(1f, 1f); 
    public Vector2 spacing = new Vector2(0.2f, 0.2f);
    public GridObj referenceGridObject;
    public BlockSpawnRules blockSpawnRules;
    public BlockCountSpawnChances blockCountSpawnChances;

    private Grid grid;
    private IEventAggregator eventAggregator;

    [Inject]
    public void Construct(IEventAggregator eventAggregator)
    {
        this.eventAggregator = eventAggregator;
    }

    void Start()
    {
        grid = new Grid(width, height, cellSize, spacing, referenceGridObject, blockSpawnRules, blockCountSpawnChances);
        eventAggregator.Subscribe<BallSpawnedEvent>(OnBallSpawned);
        eventAggregator.Subscribe<BallMoveBlockLineEvent>(OnMoveBlockLine);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            grid.DebugGridCells();

        }
    }

    private void OnDestroy()
    {
        eventAggregator.Unsubscribe<BallSpawnedEvent>(OnBallSpawned);
        eventAggregator.Unsubscribe<BallMoveBlockLineEvent>(OnMoveBlockLine);
    }

    private void OnBallSpawned(BallSpawnedEvent e)
    {
        // Subscribe once
        e.Ball.OnMoveBlockLine += HandleOnMoveBlockLine;
    }

    private void HandleOnMoveBlockLine(object sender, EventArgs e)
    {
        eventAggregator.Publish(new BallMoveBlockLineEvent());
    }

    private void OnMoveBlockLine(BallMoveBlockLineEvent e)
    {
        grid.IncrementMoveCount();
        grid.DeleteRow(0);
        grid.SpawnNewBlocks();
        //grid.CheckForMerges();
    }
}