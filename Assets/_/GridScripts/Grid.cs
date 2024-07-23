using CodeMonkey.Utils;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private int[,] gridArray;
    private float cellSize; 
    private 

    public Grid(int width, int height, float cellSize, Transform blockPrefab)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridArray = new int[width, height];
        InitializeGridObj();
    }

    private void InitializeGridObj()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y), 20, Color.white, TextAnchor.MiddleCenter);
            }

        }
    }
    instantiante game obj
    set to 
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }
}
