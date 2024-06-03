using UnityEngine.UIElements;

public struct ButtonData
{
    public int btnIconNum;
    public int btnIconRotate;
    public Button btn;

    public ButtonData(int _btnIconNum, int _btnIconRotate, Button _button)
    {
        this.btnIconNum = _btnIconNum;
        this.btnIconRotate = _btnIconRotate;
        this.btn = _button;
    }

}
