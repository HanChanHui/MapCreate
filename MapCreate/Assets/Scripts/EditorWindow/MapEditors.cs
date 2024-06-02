using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Newtonsoft.Json;

class Map{
    public string name = new string("");
    public int lv;
    public int width = 0;
    public int height = 0;
    public int[,] idx = new int[0,0];

    public Map()
    {

    }
}

public class MapEditors : MonoBehaviour
{
   

        /// <summary>
    /// 맵 정보를 담을 버튼
    /// </summary>
    public ButtonData[] buttons = new ButtonData[100];
    /// <summary>
    /// 아이콘 / 회전값 저장
    /// </summary>
    public IconData[] iconData = new IconData[10];

    /// <summary>
    /// 현재 레벨
    /// </summary>
    public int currentLevel;
    /// <summary>
    /// 타일 버튼을 선택한 인덱스
    /// </summary>
    public int selectInt;
    public int selecticonRotate;

    /// <summary>
    /// Icon 갯수
    /// </summary>
    public int iconCnt;

    public MapWindow window;
    public VisualElement btnTool;


    public void Init()
    {
        currentLevel = 0;
        selectInt = 0;
        selecticonRotate = 0;
        iconCnt = 0;
        IconLoad();
    }

    public void IconLoad()
    {
        iconData[iconCnt].icon = null;
        iconData[iconCnt++].rotate = selecticonRotate;
        iconData[iconCnt].icon = Resources.Load<Sprite>("Sprite/Tile 0");
        iconData[iconCnt++].rotate = selecticonRotate;
        iconData[iconCnt].icon = Resources.Load<Sprite>("Sprite/Tile 1");
        iconData[iconCnt++].rotate = selecticonRotate;
        iconData[iconCnt].icon = Resources.Load<Sprite>("Sprite/Tile 2");
        iconData[iconCnt++].rotate = selecticonRotate;
        iconData[iconCnt].icon = Resources.Load<Sprite>("Sprite/Tile 3");
        iconData[iconCnt++].rotate = selecticonRotate;
        iconData[iconCnt].icon = Resources.Load<Sprite>("Sprite/Tile 4");
        iconData[iconCnt++].rotate = selecticonRotate;
        iconData[iconCnt].icon = Resources.Load<Sprite>("Sprite/Tile 5");
        iconData[iconCnt++].rotate = selecticonRotate;
    }
    /// <summary>
    /// icon 파일 불러오기
    /// </summary>
    public void IconAdd(VisualElement _panel,  int _iconEventCnt)
    {
        var path = EditorUtility.OpenFilePanel("Open icon", Application.dataPath + 
                    "/Resources/sprite", "asset");
        string filename = Path.GetFileName(path);
        if(!string.IsNullOrEmpty(path))
        {
            string iconname = filename.Substring(0, filename.IndexOf('.'));
            IconCreate(_panel, iconname, _iconEventCnt);    
        }
    }
    /// <summary>
    /// icon 등록
    /// </summary>
    public void IconCreate(VisualElement _panel, string _name, int _iconEventCnt)
    {
        iconData[iconCnt].icon = Resources.Load<Sprite>("Sprite/" + _name);
        iconData[iconCnt++].rotate = 0;

        // Icon 추가
        Button btn = new Button();
        btn.style.width = 50;
        btn.style.height = 50;
        btn.name = "grid_" + iconCnt.ToString();

        _panel.Add(btn);
        iconData[_iconEventCnt].btn = btn;

        iconData[_iconEventCnt++].btn.RegisterCallback<ClickEvent>(evt => OnGridCheckButton(evt, _iconEventCnt));
        btn.AddToClassList("button-grid");
        btn.tabIndex = iconCnt - 1;
        btn.style.backgroundImage = new StyleBackground(iconData[iconCnt - 1].icon);
    }
    /// <summary>
    /// icon 회전
    /// </summary>
    public void IconRotate()
    {
        selecticonRotate = iconData[selectInt].rotate;
        selecticonRotate = (selecticonRotate + 1) % 4;
        int rot = selecticonRotate * 90;
        Debug.Log(new StyleRotate((StyleKeyword)rot));
        switch (selecticonRotate)
        {
            case 0:
                iconData[selectInt].btn.style.rotate = new Rotate(new Angle(rot));
                iconData[selectInt].rotate = selecticonRotate;
                break;
            case 1:
                iconData[selectInt].btn.style.rotate = new Rotate(new Angle(rot));
                iconData[selectInt].rotate = selecticonRotate;
                break;
            case 2:
                iconData[selectInt].btn.style.rotate = new Rotate(new Angle(rot));
                iconData[selectInt].rotate = selecticonRotate;
                break;
            case 3:
                iconData[selectInt].btn.style.rotate = new Rotate(new Angle(rot));
                iconData[selectInt].rotate = selecticonRotate;
                break;
        }
    }

    /// <summary>
    /// 어떤 icon grid 버튼을 선택 했는지
    /// </summary>
    public void OnGridCheckButton(ClickEvent _evt, int _maxCnt)
    {
        var target = _evt.target as VisualElement;

        // 초기화
        if(!target.ClassListContains("button-grid--check"))
        {
            for(int i = 0; i < _maxCnt; i++)
            {
                if(iconData[i].btn.ClassListContains("button-grid--check"))
                {
                    iconData[i].btn.RemoveFromClassList("button-grid--check");
                }
            }

            // 선택
            target.AddToClassList("button-grid--check");
            // TODO 회전값을 받아서 적용하게끔 적용.
            selectInt = target.tabIndex;
            selecticonRotate = iconData[selectInt].rotate;
        }
        else
        {
            // 취소
            target.RemoveFromClassList("button-grid--check");
            selectInt = 0;
            selecticonRotate = 0;
        }  
    }

    /// <summary>
    /// Create 버튼을 눌러서 맵 생성.
    /// </summary>
    public void BtnCreate(Toggle _toggle, Label _result, int _width, int _height)
    {
        // grid가 이미 있을땐, 생성 불가
        if(btnTool != null && btnTool.childCount != 0)
        {
            _result.text = "Grid를 생성 혹은 삭제 후 다시시도";
            return;
        }
        
        WindowCreate(_toggle, _width, _height);
        
        
        _result.text = "생성 완료";
    }

    /// <summary>
    /// Map Window 창 생성
    /// </summary>
    private void WindowCreate(Toggle _toggle, int _width, int _height)
    {
        window = EditorWindow.GetWindow<MapWindow>();
        window.titleContent = new GUIContent("MapWindow");

        Vector2 size = new Vector2(_width * 50 + (10 * _width), _height * 50 + (10 * _height));
        window.minSize = size;
        window.maxSize = size;

        // Window에 있는 btnTool 복사
        btnTool = window.btnTool;

        WindowBtnCreate(btnTool, _toggle, _width, _height);
    }

    private void WindowBtnCreate(VisualElement _btnTool, Toggle _toggle, int _width, int _height)
    {
        int maxRange = BtnToolPanelSize(_btnTool, _width, _height);
        buttons = new ButtonData[maxRange + 1];
        for(int i = 0; i < maxRange; i++)
        {
            buttons[i].btn = new Button();
            buttons[i].btn.style.width = 50;
            buttons[i].btn.style.height = 50;
            buttons[i].btn.name = i.ToString();
            _btnTool.Add(buttons[i].btn);

            buttons[i].btn.RegisterCallback<ClickEvent>(evt => {BtnClick(evt, _toggle, maxRange);});
            //buttons[i].btn.clickable.clickedWithEventInfo += BtnClick;
            buttons[i].btn.style.backgroundImage = new StyleBackground(iconData[0].icon);
        }
    }

    /// <summary>
    /// Panel Size 조절
    /// </summary>
    public int BtnToolPanelSize(VisualElement _btnTool, int _width, int _height)
    {
        _btnTool.style.width = _width * 50 + (7 * _width);
        _btnTool.style.height = _height * 50 + (7 * _height);
        return _width * _height;
    }

    public void ChangeLevel(int _lv)
    {
        currentLevel = _lv;
    }

    public void BtnClose()
    {
        if(window == null)
            return;
        btnTool.Clear();
        window.Close();

        buttons = new ButtonData[201];
    }
    /// <summary>
    /// 버튼 클릭에 대한 이벤트
    /// </summary>
    public void BtnClick(ClickEvent _evt, Toggle _toggle, int _maxRange)
    {
        if(!_toggle.value)
        {
            var btn = _evt.target as Button;
            int idx = int.Parse(btn.name);
            buttons[idx].btnIconNum = selectInt;
            buttons[idx].btnIconRotate = selecticonRotate;
            btn.style.backgroundImage = new StyleBackground(iconData[buttons[idx].btnIconNum].icon);
            btn.style.rotate = new Rotate(new Angle(selecticonRotate * 90));
        }
        else
        {
            int maxRange = _maxRange;
            for (int i = 0; i < maxRange; i++)
            {
                buttons[i].btnIconNum = selectInt;
                buttons[i].btnIconRotate = selecticonRotate;
                buttons[i].btn.style.backgroundImage = new StyleBackground(iconData[buttons[i].btnIconNum].icon);
                buttons[i].btn.style.rotate = new Rotate(new Angle(selecticonRotate * 90));
            }
        }
    }


    public void SaveBtn(Label _fileName, Label _result,  int _width, int _height)
    {
        if(btnTool == null || btnTool.childCount == 0)
        {
            _result.text = "Grid가 없습니다.";
            return;
        }

        //TODO Json으로 저장하는 방식 제작.
        int maxRange = _width * _height;
        Map maps = new Map();
        maps.idx = new int[maxRange + 1, 2];
        float level = currentLevel > 100 ? ((float)currentLevel / 1000) : ((float)currentLevel / 100);
        string str_lv = level >= 0 ?  new string("0") : level.ToString().Substring(level.ToString().IndexOf('.') + 1, 2);
        Debug.Log(str_lv);
        maps.lv = currentLevel;
        maps.name = "Stage" + "_" + _width + "x" + _height + "_" + "0" + str_lv;
        _fileName.text = maps.name;
        Debug.Log(maps.name);
        maps.width = _width;
        maps.height = _height;
        

        for(int i = 0; i < maxRange; i++)
        {
            maps.idx[i,0] = buttons[i].btnIconNum;
            maps.idx[i,1] = buttons[i].btnIconRotate;
        }
        string jsonData = JsonUtility.ToJson(maps);
        string path = Path.Combine(Application.dataPath + "/Resources/Maps", maps.name + ".json");
        File.WriteAllText(path, jsonData);

        AssetDatabase.Refresh();
        _result.text = "저장 완료";
    }

    public void LoadBtn(Label _fileName, Label _result, BaseField<int> _currentlv, Toggle _toggle)
    {
        if(btnTool != null || btnTool.childCount == 0)
        {
            var path = EditorUtility.OpenFilePanel("Open Level", Application.dataPath + 
                    "/Resources/Maps", "json");
            string filename = Path.GetFileName(path);
            if(!string.IsNullOrEmpty(path))
            {
                LoadLVBtn(_fileName, _result, _currentlv, _toggle, path);
                //currentLevelField.value = currentLevel;
            }
            _result.text = "불러오기 완료";
        }
        else
        {
            _result.text = "Grid를 생성 혹은 삭제 후 다시시도";
        }
        
    }

    public void LoadLVBtn(Label _fileName, Label _result, BaseField<int> _currentlv, Toggle _toggle, string _path)
    {
        Map maps = new Map();

        string path = Path.Combine(_path);
        string jsonData = File.ReadAllText(path);
        maps = JsonUtility.FromJson<Map>(jsonData);
        _fileName.text = maps.name;
        _currentlv.value = maps.lv;
        // 버튼 생성
        BtnCreate(_toggle, _result, maps.width, maps.height);
        // grid 배치
        int maxRange = maps.width * maps.height;
        for (int i = 0; i < maxRange; i++)
        {
            buttons[i].btnIconNum = maps.idx[i,0];
            buttons[i].btnIconRotate = maps.idx[i, 1];
            buttons[i].btn.style.backgroundImage = new StyleBackground(iconData[maps.idx[i, 0]].icon);
            buttons[i].btn.style.rotate = new Rotate(new Angle(buttons[i].btnIconRotate * 90));
        }
    }
 
}
