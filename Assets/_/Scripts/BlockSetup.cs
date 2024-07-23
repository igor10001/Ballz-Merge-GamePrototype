using UnityEngine;

public class BlockSetup : MonoBehaviour
{
    public Block[,] grid;
    public int rows = 7;
    public int columns = 5;

    public void InitializeGrid()
    {
        grid = new Block[columns, rows];
    }

    public void AddBlock(Block block)
    {
        grid[block.gridPosition.x, block.gridPosition.y] = block;
    }

    public void MoveBlock(Block block, Vector2Int direction)
    {
        Vector2Int newPosition = block.gridPosition + direction;
        if (IsValidPosition(newPosition))
        {
            grid[block.gridPosition.x, block.gridPosition.y] = null;
            block.Move(direction);
            if (grid[newPosition.x, newPosition.y] != null)
            {
                Block adjacentBlock = grid[newPosition.x, newPosition.y];
                if (adjacentBlock.number == block.number)
                {
                    DestroyBlock(block);
                    DestroyBlock(adjacentBlock);
                }
            }
            grid[block.gridPosition.x, block.gridPosition.y] = block;
        }
    }

    public bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.x < columns && position.y >= 0 && position.y < rows;
    }

    public void DestroyBlock(Block block)
    {
        grid[block.gridPosition.x, block.gridPosition.y] = null;
        Destroy(block.gameObject);
    }
}
