using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private Vector2 cellSize; // Width and height of each cell
    private Vector2 spacing; // Distance between grid objects
    private GridObject[,] gridObjects;  // Array to store the grid objects
    private int numberOfObjects; // Number of grid objects to spawn

    public Grid(int width, int height, Vector2 cellSize, Vector2 spacing, GridObject referenceGridObject, int numberOfObjects)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.spacing = spacing;
        this.numberOfObjects = numberOfObjects;
        InitializeGridObjects(referenceGridObject);
    }

    private void InitializeGridObjects(GridObject referenceGridObject)
    {
        gridObjects = new GridObject[width, height];
        List<Vector2Int> availablePositions = new List<Vector2Int>();

        // Populate all possible positions
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                availablePositions.Add(new Vector2Int(x, y));
            }
        }

        // Shuffle positions to randomize
        Shuffle(availablePositions);

        // Place objects at random positions
        for (int i = 0; i < Mathf.Min(numberOfObjects, availablePositions.Count); i++)
        {
            Vector2Int position = availablePositions[i];
            GridObject newGridObject = ScriptableObject.Instantiate(referenceGridObject);
            CreateGridObject(position.x, position.y, newGridObject);
        }
    }

    private void CreateGridObject(int x, int y, GridObject gridObject)
    {
        Vector3 worldPosition = GetWorldPosition(x, y);
        Transform blockTransform = Object.Instantiate(gridObject.prefab, worldPosition, Quaternion.identity);
        
        // Adjust scale based on cellSize
        Vector3 cellScale = new Vector3(cellSize.x / blockTransform.GetComponent<Renderer>().bounds.size.x,
                                         cellSize.y / blockTransform.GetComponent<Renderer>().bounds.size.y, 
                                         1);
        blockTransform.localScale = cellScale;

        // Set the grid position in the GridObject
        gridObject.gridPosition = new Vector2(x, y);

        // Update the Transform in the GridObject
        gridObject.prefab = blockTransform;

        // Assign a unique random color to each block
        blockTransform.GetComponent<SpriteRenderer>().color = GetRandomColor();

        // Create and configure TextMesh
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(blockTransform);
        textObj.transform.localPosition = Vector3.zero;
        TextMesh textMesh = textObj.AddComponent<TextMesh>();
        textMesh.text = $"({x}, {y})";
        textMesh.fontSize = 60;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.characterSize = 0.1f;
        textMesh.color = Color.black; // Set text color

        gridObjects[x, y] = gridObject;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        // Calculate position with cellSize and spacing
        return new Vector3(x * (cellSize.x + spacing.x) + spacing.x / 2, 
                           y * (cellSize.y + spacing.y) + spacing.y / 2, 
                           0);
    }

    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }

    public void SwapGridObjects(int x1, int y1, int x2, int y2)
    {
        GridObject obj1 = GetGridObjectAt(x1, y1);
        GridObject obj2 = GetGridObjectAt(x2, y2);

        if (obj1 != null && obj2 != null)
        {
            // Log positions before swap
            Debug.Log($"Before Swap: GridObject1 ({x1}, {y1}) and GridObject2 ({x2}, {y2})");

            // Swap positions in gridObjects array
            gridObjects[x1, y1] = obj2;
            gridObjects[x2, y2] = obj1;

            // Swap positions in the world
            Vector3 tempPosition = obj1.prefab.position;
            obj1.prefab.position = obj2.prefab.position;
            obj2.prefab.position = tempPosition;

            // Log positions after swap
            Debug.Log($"After Swap: GridObject1 ({x1}, {y1}) now at {obj1.prefab.position}, GridObject2 ({x2}, {y2}) now at {obj2.prefab.position}");

            // Update the grid position in GridObjects
            obj1.gridPosition = new Vector2(x2, y2);
            obj2.gridPosition = new Vector2(x1, y1);
        }
    }

    private GridObject GetGridObjectAt(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridObjects[x, y];
        }
        return null;
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
}
