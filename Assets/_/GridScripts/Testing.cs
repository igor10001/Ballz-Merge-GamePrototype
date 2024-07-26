using System;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public int width = 10; // Set your desired width
    public int height = 10; // Set your desired height
    public Vector2 cellSize = new Vector2(1f, 1f); // Set your desired cell width and height
    public Vector2 spacing = new Vector2(0.2f, 0.2f); // Distance between grid objects
    public GridObj referenceGridObject;

    private Grid grid;

    void Start()
    {
       
        ProjectileLauncher.Instance.OnBallSpawn += HandleOnBallSpawn;
        grid = new Grid(width, height, cellSize, spacing, referenceGridObject, 10);
    }
    private void HandleOnMoveBlockLine(object sender, EventArgs e)
    {
        grid.DeleteRow(0);
    }
    private void HandleOnBallSpawn( object sender, EventArgs e)
    {
        ProjectileLauncher.Instance.m_CurrentBall.OnMoveBlockLine += HandleOnMoveBlockLine;
    }
   

    
}