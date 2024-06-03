using UnityEngine;
using UnityEngine.UIElements;

public struct IconData
{
    public Sprite icon;
    public Button btn;
    public int rotate;

    public IconData(Sprite _icon, Button _btn, int _rotate)
    {
        this.icon = _icon;
        this.btn = _btn;
        this.rotate = _rotate;
    }
}
