using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public int rows;
    public int columns;
    public int cellSize;
    private Transform blockPrefab;
    void Start()
    {
        Grid grid = new Grid(6, 6, 20f);
    }

}
