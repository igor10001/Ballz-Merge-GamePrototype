using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridObj : MonoBehaviour
{
    public int number;
    public Vector2 gridPosition;
    public TextMesh textMesh;

    private Grid grid;

    public void Initialize(Grid grid, int number, Vector2 gridPosition)
    {
        this.grid = grid;
        this.number = number;
        this.gridPosition = gridPosition;
        UpdateText();
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
                //CheckForMerge();
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

    private void UpdateText()
    {
        if (textMesh != null)
        {
            textMesh.text = number.ToString();
        }
    }

    public void CheckForMerge()
    {
        // Check adjacent cells for merging
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
                }
            }
        }
    }
}
