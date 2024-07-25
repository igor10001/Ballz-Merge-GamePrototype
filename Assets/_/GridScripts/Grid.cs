using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private Vector2 cellSize;
    private Vector2 spacing;
    private GridObj[,] gridObjects;
    private int numberOfObjects;
    private GridObj referenceGridObject;

    public Grid(int width, int height, Vector2 cellSize, Vector2 spacing, GridObj referenceGridObject, int numberOfObjects)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.spacing = spacing;
        this.numberOfObjects = numberOfObjects;
        this.referenceGridObject = referenceGridObject;
        InitializeGridObjects();
    }

    public void DebugGridObjects()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridObj obj = GetGridObjectAt(x, y);
                if (obj != null)
                {
                    Debug.Log($"GridObject at ({x}, {y}): Health = {obj.number}, Position = {obj.transform.position}");
                }
                else
                {
                    Debug.Log($"GridObject at ({x}, {y}) is null.");
                }
            }
        }
    }

    private void InitializeGridObjects()
    {
        gridObjects = new GridObj[width, height];
        List<Vector2Int> availablePositions = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                availablePositions.Add(new Vector2Int(x, y));
            }
        }

        Shuffle(availablePositions);

        for (int i = 0; i < Mathf.Min(numberOfObjects, availablePositions.Count); i++)
        {
            Vector2Int position = availablePositions[i];
            CreateGridObject(position.x, position.y);
        }
    }

    private void CreateGridObject(int x, int y)
    {
        Vector3 worldPosition = GetWorldPosition(x, y);
        GridObj newGridObj = Object.Instantiate(referenceGridObject, worldPosition, Quaternion.identity);
        newGridObj.Initialize(this, GetRandomPairNumber(), new Vector2(x, y));

        gridObjects[x, y] = newGridObj;

        newGridObj.transform.localScale = new Vector3(cellSize.x / newGridObj.GetComponent<Renderer>().bounds.size.x,
                                                      cellSize.y / newGridObj.GetComponent<Renderer>().bounds.size.y, 1);

        newGridObj.GetComponent<SpriteRenderer>().color = GetRandomColor();
        CreateTextMesh(newGridObj);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * (cellSize.x + spacing.x) + spacing.x / 2, y * (cellSize.y + spacing.y) + spacing.y / 2, 0);
    }

    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }

    public void MoveGridObject(int x1, int y1, int x2, int y2)
    {
        GridObj obj = GetGridObjectAt(x1, y1);
        if (obj != null)
        {
            gridObjects[x2, y2] = obj;
            gridObjects[x1, y1] = null;
            obj.gridPosition = new Vector2(x2, y2);
            obj.transform.position = GetWorldPosition(x2, y2);
        }
    }

    public void MergeGridObjects(int x1, int y1, int x2, int y2)
    {
        GridObj obj1 = GetGridObjectAt(x1, y1);
        GridObj obj2 = GetGridObjectAt(x2, y2);

        if (obj1 != null && obj2 != null && obj1.number == obj2.number)
        {
            obj1.number += obj2.number;
            Object.Destroy(obj2.gameObject);
            gridObjects[x2, y2] = null;
        }
    }

    public GridObj GetGridObjectAt(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridObjects[x, y];
        }
        return null;
    }

    public bool IsPositionValid(Vector2Int position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < width && position.y < height;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void RemoveGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            GridObj obj = gridObjects[x, y];
            if (obj != null)
            {
                Object.Destroy(obj.transform.gameObject);
                gridObjects[x, y] = null;
            }
        }
    }

    private int GetRandomPairNumber()
    {
        int[] pairNumbers = { 2, 4, 6, 8, 10 };
        return pairNumbers[Random.Range(0, pairNumbers.Length)];
    }

    private void CreateTextMesh(GridObj gridObj)
    {
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(gridObj.transform);
        textObj.transform.localPosition = Vector3.zero;
        TextMesh textMesh = textObj.AddComponent<TextMesh>();
        textMesh.text = gridObj.number.ToString();
        textMesh.fontSize = 60;
        textMesh.color = Color.black;
    }
}
