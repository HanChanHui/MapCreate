using UnityEngine.UIElements;

public class ImgData
{
    public int imgNum;
    public int tileX;
    public int tileY;
    public int imgRotate;
    public VisualElement img;

    public ImgData(int _imgNum, int _imgRotate, VisualElement _img)
    {
        this.imgNum = _imgNum;
        this.imgRotate = _imgRotate;
        this.img = _img;
    }
}
