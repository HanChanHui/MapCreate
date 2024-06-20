using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileObject
{
    private TileSystem<TileObject> tileSystem;
    private TilePosition tilePosition;
    private List<Object> objectList;

    public TileObject(TileSystem<TileObject> _tileSystem, TilePosition _gridPosition)
    {
        this.tileSystem = _tileSystem;
        this.tilePosition = _gridPosition;
        objectList = new List<Object>();
    }

    public override string ToString()
    {
        string objectString = "";
        foreach (Object obj in objectList)
        {
            objectString += obj + "\n";
        }

        return tilePosition.ToString() + "\n" + objectString;
    }

    public void AddObject(Object _obj)
    {
        objectList.Add(_obj);
    }

    public void RemoveObject(Object _obj)
    {
        objectList.Remove(_obj);
    }

    public List<Object> GetObjectList()
    {
        return objectList;
    }

    public bool HasAnyObject()
    {
        return objectList.Count > 0;
    }


    public Object GetObject()
    {
        if(HasAnyObject())
        {
            return objectList[0];
        }
        else
        {
            return null;
        }
    }


}
