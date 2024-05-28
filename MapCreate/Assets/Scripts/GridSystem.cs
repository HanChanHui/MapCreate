using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class GridSystem<TGridObject>
{
    
    private int width;
    private int height;
    private float cellSize;
    private TGridObject[,] gridObjectArray;


    public GridSystem(int _width, int _height, float _cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.width = _width;
        this.height = _height;
        this.cellSize = _cellSize;

        gridObjectArray = new TGridObject[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = createGridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition _gridPosition)
    {
        return new Vector3(_gridPosition.x, 0, _gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 _worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(_worldPosition.x / cellSize),
            Mathf.RoundToInt(_worldPosition.z / cellSize)
            );
    }

    public void CreateDebugObject(Transform _debugPrefab)
    {
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GridPosition _gridPosition = new GridPosition(x, z);

                Transform debugTransform = GameObject.Instantiate(_debugPrefab, GetWorldPosition(_gridPosition), Quaternion.identity);
                
            }
        }
    }






    
}
