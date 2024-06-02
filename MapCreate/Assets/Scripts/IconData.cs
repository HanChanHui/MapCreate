using UnityEngine;
using UnityEngine.UIElements;

public struct IconData
{
    public Sprite icon;
    public int rotate;

    public IconData(Sprite _icon, int _rotate)
    {
        this.icon = _icon;
        this.rotate = _rotate;
    }
}
