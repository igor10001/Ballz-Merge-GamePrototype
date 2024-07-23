using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BlockSetup blockSetup;
    public GameObject blockPrefab;
    public int minBlocksPerRow = 1;
    public int maxBlocksPerRow = 4;

    private void Start()
    {
        blockSetup.InitializeGrid();
        SpawnNewRow();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Handle ball launch
        }

        /*if (*//* Check if turn ends *//*)
        {
            EndTurn();
        }*/
    }

    private void EndTurn()
    {
        MoveBlocksDown();
        SpawnNewRow();
        CheckForMerges();
    }

    private void MoveBlocksDown()
    {
        for (int y = 1; y < blockSetup.rows; y++)
        {
            for (int x = 0; x < blockSetup.columns; x++)
            {
                Block block = blockSetup.grid[x, y];
                if (block != null)
                {
                    blockSetup.MoveBlock(block, Vector2Int.down);
                }
            }
        }
    }

    private void SpawnNewRow()
    {
        int blockCount = Random.Range(minBlocksPerRow, maxBlocksPerRow + 1);
        for (int i = 0; i < blockCount; i++)
        {
            Vector2Int position = new Vector2Int(Random.Range(0, blockSetup.columns), blockSetup.rows - 1);
            if (blockSetup.grid[position.x, position.y] == null)
            {
                GameObject blockObject = Instantiate(blockPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
                Block block = blockObject.GetComponent<Block>();
                block.Initialize(Random.Range(1, 10), position);
                blockSetup.AddBlock(block);
            }
        }
    }

    private void CheckForMerges()
    {
        for (int y = 0; y < blockSetup.rows; y++)
        {
            for (int x = 0; x < blockSetup.columns; x++)
            {
                Block block = blockSetup.grid[x, y];
                if (block != null)
                {
                    CheckAdjacentBlocks(block);
                }
            }
        }
    }

    private void CheckAdjacentBlocks(Block block)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (Vector2Int direction in directions)
        {
            Vector2Int adjacentPosition = block.gridPosition + direction;
            if (blockSetup.IsValidPosition(adjacentPosition))
            {
                Block adjacentBlock = blockSetup.grid[adjacentPosition.x, adjacentPosition.y];
                if (adjacentBlock != null && adjacentBlock.number == block.number)
                {
                    blockSetup.DestroyBlock(block);
                    blockSetup.DestroyBlock(adjacentBlock);
                }
            }
        }
    }
}
