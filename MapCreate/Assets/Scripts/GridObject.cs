using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    
    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private List<Sprite> spriteList;
    

    public GridObject(GridSystem<GridObject> _gridSystem, GridPosition _gridPosition)
    {
        this.gridSystem = _gridSystem;
        this.gridPosition = _gridPosition;
    }

    public override string ToString()
    {
        string spriteString = "";
        foreach(Sprite sprite in spriteList)
        {
            spriteString += sprite.name + "\n";
        }
        return gridPosition.ToString() + "\n" + spriteString;
    }



}
