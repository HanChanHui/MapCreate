using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileObject
{
    private TileSystem<TileObject> tileSystem;
    public TilePosition tilePosition;
    private List<Object> objectList;
    public Sprite tileSprite;
    public VisualElement tileVE;
    public int tileRotate;
    public int tileSpriteNum;


    public TileObject(TileSystem<TileObject> _tileSystem, TilePosition _gridPosition, Sprite _tileSprite)
    {
        this.tileSystem = _tileSystem;
        this.tilePosition = _gridPosition;
        this.tileSprite = _tileSprite;
        objectList = new List<Object>();
        this.tileVE = new VisualElement();
        this.tileVE.style.backgroundImage = new StyleBackground(_tileSprite);
        this.tileVE.style.backgroundColor = Color.blue;
        this.tileVE.style.width = 50;
        this.tileVE.style.height = 50;
    }

    public void UpdateTile(Sprite sprite, int rotate)
    {
        this.tileSprite = sprite;
        this.tileRotate = rotate;
        this.tileVE.style.backgroundImage = new StyleBackground(sprite);
        this.tileVE.style.rotate = new Rotate(new Angle(rotate * 90));
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
