using UnityEngine;
using UnityEngine.UIElements;

public struct ButtonData
{
    public int[,] btnIconNum;
    public Button btn;

    public ButtonData(int[,] _iconNum, Button _button)
    {
        this.btnIconNum = _iconNum;
        this.btn = _button;
    }

}
