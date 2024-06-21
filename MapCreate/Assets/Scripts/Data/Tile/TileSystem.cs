using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileSystem<TTileObject> : IEnumerable<TTileObject>
{
    private int width;
    private int height;
    private float cellSize;
    private TTileObject[,] tileObjectArray;
    private VisualElement parentElement;

    public TileSystem(int _width, int _height, float _cellSize, Func<TileSystem<TTileObject>, TilePosition, Sprite, TTileObject> _createTileObject, VisualElement _parentElement)
    {
        this.width = _width;
        this.height = _height;
        this.cellSize = _cellSize;
        this.parentElement = _parentElement;

        tileObjectArray = new TTileObject[_width, _height];

        for(int x = 0; x < _width; x++)
        {
            for(int z = 0; z < _height; z++)
            {
                TilePosition gridPosition = new TilePosition(x, z);
                tileObjectArray[x, z] = _createTileObject(this, gridPosition, null);
                VisualElement tileElement = (tileObjectArray[x, z] as TileObject).tileVE;
                parentElement.Add(tileElement);
                tileElement.style.borderLeftWidth = 0;
                tileElement.style.borderRightWidth = 0;
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

    public IEnumerator<TTileObject> GetEnumerator()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                yield return tileObjectArray[x, z];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

