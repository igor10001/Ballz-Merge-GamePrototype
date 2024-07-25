using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridObj : MonoBehaviour
{
    public int number;
    public Vector2 gridPosition;
    public TextMeshProUGUI text;

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
                grid.MergeGridObjects((int)gridPosition.x, (int)gridPosition.y, newPosition.x, newPosition.y);
            }
        }
    }

    public void OnBallHit(Vector2 hitDirection)
    {
        Move(hitDirection);
    }
}