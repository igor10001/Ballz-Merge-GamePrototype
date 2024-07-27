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
    [Inject]
    private ProjectileLauncher projectileLauncher;
    private Grid grid;

    void Start()
    {
        grid = new Grid(width, height, cellSize, spacing, referenceGridObject, 10);
    
        projectileLauncher.OnBallSpawn += HandleOnBallSpawn;
    }
  
    private void HandleOnBallSpawn( object sender, EventArgs e)
    {
        projectileLauncher.m_CurrentBall.OnMoveBlockLine += HandleOnMoveBlockLine;
    }
   
    private void HandleOnMoveBlockLine(object sender, EventArgs e)
    {
        grid.DeleteRow(0);
    }
    
}