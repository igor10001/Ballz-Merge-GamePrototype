using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridObj : MonoBehaviour
{
    private int number;

    private Vector2 gridPosition;

    public int Number
    {
        get { return number; }
        set { number = value; }
    }

    public Vector2 GridPosition
    {
        get { return gridPosition; }
        set { gridPosition = value; }

    }
    private Grid grid;

    public void Initialize(Grid grid, int number, Vector2 gridPosition)
    {
        this.grid = grid;
        this.number = number;
        this.gridPosition = gridPosition;
    }

    public void Move(Vector2 direction)
    {
        Vector2Int newPosition = new Vector2Int((int)gridPosition.x + (int)direction.x, (int)gridPosition.y + (int)direction.y);

        if (grid.IsPositionValid(newPosition))
        {
            GridObj targetObj = grid.GetGridObjectAt(newPosition.x, newPosition.y);

            if (targetObj == null)
            {
                grid.MoveGridObject((int)gridPosition.x, (int)gridPosition.y, newPosition.x, newPosition.y);
            }
            else if (targetObj.number == number)
            {
                CheckForMerge();
            }
        }
    }

    public void OnBallHit(Vector2 hitDirection)
    {
        Move(-hitDirection);
    }

  

    public void CheckForMerge()
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (var direction in directions)
        {
            Vector2Int adjacentPosition = new Vector2Int((int)gridPosition.x + direction.x, (int)gridPosition.y + direction.y);
            if (grid.IsPositionValid(adjacentPosition))
            {
                GridObj adjacentObj = grid.GetGridObjectAt(adjacentPosition.x, adjacentPosition.y);
                if (adjacentObj != null && adjacentObj.number == number)
                {
                    grid.MergeGridObjects((int)gridPosition.x, (int)gridPosition.y, adjacentPosition.x, adjacentPosition.y);
                    break; 
                }
            }
        }
    }
}
