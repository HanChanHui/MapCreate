using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSystem<TTileObject>
{
    private int width;
    private int height;
    private float cellSize;
    private TTileObject[,] tileObjectArray;

    public TileSystem(int width, int height, float cellSize, Func<TileSystem<TTileObject>, TilePosition, TTileObject> createTileObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        tileObjectArray = new TTileObject[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                TilePosition gridPosition = new TilePosition(x, z);
                tileObjectArray[x, z] = createTileObject(this, gridPosition);
            }
        }

    }

    public Vector3 GetWordlPosition(TilePosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }

    public TilePosition GetGridPosition(Vector3 worldPosition)
    {
        return new TilePosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
            );
    }

    // public void CreateDebugObject(Transform debugPrefab)
    // {
    //     for (int x = 0; x < width; x++)
    //     {
    //         for (int z = 0; z < height; z++)
    //         {
    //             TilePosition gridPosition = new TilePosition(x, z);

    //             Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWordlPosition(gridPosition), Quaternion.identity);
    //             GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
    //             gridDebugObject.SetGridObject(GetGridObject(gridPosition));
    //         }
    //     }
    // }

    public TTileObject GetGridObject(TilePosition gridPosition)
    {
        return tileObjectArray[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(TilePosition gridPosition)
    {
        return gridPosition.x >= 0 && 
               gridPosition.z >= 0 && 
               gridPosition.x < width && 
               gridPosition.z < height;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
}

