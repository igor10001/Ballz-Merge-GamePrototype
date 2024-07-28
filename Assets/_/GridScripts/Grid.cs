using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Grid
{
    private int width;
    private int height;
    private Vector2 cellSize;
    private Vector2 spacing;
    private GridObj[,] gridObjects;
    private GridObj referenceGridObject;
    private BlockSpawnRules blockSpawnRules;
    private BlockCountSpawnChances blockCountSpawnChances;
    private int moveCount;

    public Grid(int width, int height, Vector2 cellSize, Vector2 spacing, GridObj referenceGridObject, BlockSpawnRules blockSpawnRules, BlockCountSpawnChances blockCountSpawnChances)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.spacing = spacing;
        this.referenceGridObject = referenceGridObject;
        this.blockSpawnRules = blockSpawnRules;
        this.blockCountSpawnChances = blockCountSpawnChances;
        InitializeGridObjects();
    }
    public void MergeGridObjects(int x1, int y1, int x2, int y2)
    {
        GridObj obj1 = GetGridObjectAt(x1, y1);
        GridObj obj2 = GetGridObjectAt(x2, y2);

        if (obj1 != null && obj2 != null && obj1.Number == obj2.Number)
        {
            obj1.transform.DOScale(Vector3.zero, 0.7f).OnComplete(() =>
            {
                Object.Destroy(obj1.gameObject);
            });

            obj2.transform.DOScale(Vector3.zero, 0.7f).OnComplete(() =>
            {
                Object.Destroy(obj2.gameObject);
            });

            gridObjects[x1, y1] = null;
            gridObjects[x2, y2] = null;

            obj1.CheckForMerge();
        }
    }

    public bool IsPositionValid(Vector2Int position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < width && position.y < height;
    }
    private void InitializeGridObjects()
    {
        gridObjects = new GridObj[width, height];
        moveCount = 0;
        SpawnInitialBlocks();
    }

   


    private void CreateGridObject(int x, int y)
    {
        Vector3 worldPosition = GetWorldPosition(x, y);
        GridObj newGridObj = Object.Instantiate(referenceGridObject, worldPosition, Quaternion.identity);
        newGridObj.Initialize(this, GetRandomBlockNumber(), new Vector2(x, y));

        gridObjects[x, y] = newGridObj;

        newGridObj.transform.localScale = new Vector3(cellSize.x / newGridObj.GetComponent<Renderer>().bounds.size.x,
                                                      cellSize.y / newGridObj.GetComponent<Renderer>().bounds.size.y, 1);

        newGridObj.GetComponent<SpriteRenderer>().color = GetRandomColor();
        CreateTextMesh(newGridObj);
    }

    private int GetRandomBlockNumber()
    {
        var rule = GetCurrentBlockNumberRule();
        return rule.possibleBlockNumbers[Random.Range(0, rule.possibleBlockNumbers.Count)];
    }

    private BlockSpawnRules.MoveRangeBlockNumbers GetCurrentBlockNumberRule()
    {
        foreach (var rule in blockSpawnRules.blockNumberRules)
        {
            if (moveCount >= rule.minMoves && moveCount <= rule.maxMoves)
            {
                return rule;
            }
        }
        return blockSpawnRules.blockNumberRules[0];
    }

    private BlockCountSpawnChances.MoveRangeBlockCounts GetCurrentBlockCountRule()
    {
        foreach (var rule in blockCountSpawnChances.blockCountRules)
        {
            if (moveCount >= rule.minMoves && moveCount <= rule.maxMoves)
            {
                return rule;
            }
        }
        return blockCountSpawnChances.blockCountRules[0];
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
            obj.GridPosition = new Vector2(x2, y2);
            obj.transform.DOMove(GetWorldPosition(x2, y2), 1f).OnComplete(() =>
            {
                obj.CheckForMerge();
            });
        }
    }

    public void IncrementMoveCount()
    {
        moveCount++;
    }

   
    

    private int DetermineNumberOfBlocksToSpawn(BlockCountSpawnChances.MoveRangeBlockCounts rule)
    {
        float rand = Random.value * 100;
        float cumulativeChance = 0;

        foreach (var chance in rule.blockCountChances)
        {
            cumulativeChance += chance.probability;
            if (rand <= cumulativeChance)
            {
                return chance.blockCount;
            }
        }
        return 1; // Default to spawning at least one block
    }

    

    public void RemoveGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            GridObj obj = gridObjects[x, y];
            if (obj != null)
            {
                obj.transform.DOScale(Vector3.zero, 0.7f).OnComplete(() =>
                {
                    Object.Destroy(obj.transform.gameObject);
                    gridObjects[x, y] = null;
                });
            }
        }
    }

    private List<Vector2Int> GetAvailablePositions()
    {
        List<Vector2Int> availablePositions = new List<Vector2Int>();
        for (int y = height - 1; y >= 0; y--) // Start from the top row
        {
            for (int x = 0; x < width; x++) // Move left to right within the row
            {
                if (gridObjects[x, y] == null)
                {
                    availablePositions.Add(new Vector2Int(x, y));
                }
            }
        }
        return availablePositions;
    }

    private void SpawnInitialBlocks()
    {
        List<Vector2Int> availablePositions = GetAvailablePositions();

        for (int i = 0; i < Mathf.Min(10, availablePositions.Count); i++)
        {
            Vector2Int position = availablePositions[i];
            CreateGridObject(position.x, position.y);
        }
    }

    public void SpawnNewBlocks()
    {
        var rule = GetCurrentBlockCountRule();
        int blocksToSpawn = DetermineNumberOfBlocksToSpawn(rule);

        List<Vector2Int> availablePositions = GetAvailablePositions();

        for (int i = 0; i < Mathf.Min(blocksToSpawn, availablePositions.Count); i++)
        {
            Vector2Int position = availablePositions[i];
            CreateGridObject(position.x, position.y);
        }
    }

    public void DebugGridCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridObj gridObj = gridObjects[x, y];
                string status = gridObj == null ? "null" : "occupied";
                Debug.Log($"Cell ({x}, {y}): {status}");
            }
        }
    }

    private void CreateTextMesh(GridObj gridObj)
    {
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(gridObj.transform);
        textObj.transform.localPosition = Vector3.zero; 

        TextMesh textMesh = textObj.AddComponent<TextMesh>();
        textMesh.text = gridObj.Number.ToString();
        textMesh.fontSize = 32;
        textMesh.color = Color.black;

        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;

        Bounds textBounds = textMesh.GetComponent<Renderer>().bounds;
        Vector3 offset = new Vector3(-textBounds.extents.x, -textBounds.extents.y, 0);
        textObj.transform.localPosition = offset;
        textObj.transform.localPosition = Vector3.zero; 
    }

    public void DeleteRow(int row)
    {
        if (row < 0 || row >= height)
        {
            Debug.LogError("Row index is out of bounds.");
            return;
        }

        for (int x = 0; x < width; x++)
        {
            RemoveGridObject(x, row);
        }

        for (int y = row + 1; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (gridObjects[x, y] != null)
                {
                    MoveGridObject(x, y, x, y - 1);
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            gridObjects[x, height - 1] = null;
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
}
