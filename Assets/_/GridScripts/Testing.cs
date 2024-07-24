using UnityEngine;

public class Testing : MonoBehaviour
{
    public int width = 10; // Set your desired width
    public int height = 10; // Set your desired height
    public Vector2 cellSize = new Vector2(1f, 1f); // Set your desired cell width and height
    public GridObject referenceGridObject;

    private Grid grid;

    void Start()
    {
        // Create a Grid with specified dimensions and number of objects
        grid = new Grid(width, height, cellSize, referenceGridObject, 10);
    }

    void Update()
    {
        // Example swap call for testing purposes
        if (Input.GetKeyDown(KeyCode.S))
        {
            grid.SwapGridObjects(0, 0, 1, 1); // Swap objects at (0, 0) and (1, 1)
        }
    }
}