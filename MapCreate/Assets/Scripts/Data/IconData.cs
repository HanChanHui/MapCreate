using UnityEngine;
using UnityEngine.UIElements;

public struct IconData
{
    public Sprite icon;
    public VisualElement iconVE;
    public int rotate;

    public IconData(Sprite _icon, VisualElement _iconVE, int _rotate)
    {
        this.icon = _icon;
        this.iconVE = _iconVE;
        this.rotate = _rotate;
    }
}
