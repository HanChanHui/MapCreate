using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Newtonsoft.Json;
using System.Text;

class Map{
    public string name;
    public int lv;
    public int width;
    public int height;
    public int[,] idx;

    public Map()
    {
        this.name = new string("");
        this.lv = 0;
        this.width = 0;
        this.height = 0;
        this.idx = new int[0,0];
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
    /// Icon 갯수 / IconEvent 갯수
    /// </summary>
    public int iconCnt;
    public int iconEventCnt;
    /// <summary>
    /// Grid Window
    /// </summary>
    public MapWindow window;
    public VisualElement btnTool;


    public void Init()
    {
        currentLevel = 0;
        selectInt = 0;
        selecticonRotate = 0;
        iconEventCnt = 0;
        iconCnt = 0;
    }
    public void IconEvent(VisualElement _gridiconPanel)
    {
        if(_gridiconPanel != null)
        {
            // grid 버튼 등록 및 이벤트 추가
            foreach(var child in _gridiconPanel.Children())
            {
                if(child is Button button)
                {
                    if(button.name != "grid_add")
                    {
                        iconData[iconEventCnt].btn = button;
                        iconData[iconEventCnt++].btn.RegisterCallback<ClickEvent>(OnGridCheckButton);
                    }
                }
            }
        }
    }

    public void IconEventDelete()
    {
        for(int i = 0; i < iconEventCnt - 1; i++)
        {
            iconData[i].btn.UnregisterCallback<ClickEvent>(OnGridCheckButton);
        }

        BtnClose();
    }

    public void IconLoad(VisualElement _panel)
    {
        iconData[iconCnt].icon = null;
        iconData[iconCnt++].rotate = selecticonRotate;

        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprite");
        
        if(sprites.Length > 0)
        {
            for(int i = 0; i < sprites.Length; i++)
            {
                iconData[iconCnt].icon = sprites[i];
                iconData[iconCnt++].rotate = 0;

                // Icon 추가
                Button btn = new Button();
                btn.style.width = 50;
                btn.style.height = 50;
                btn.name = "grid_" + iconCnt.ToString();

                _panel.Add(btn);
                btn.AddToClassList("button-grid");
                btn.tabIndex = iconCnt - 1;
                btn.style.backgroundImage = new StyleBackground(iconData[iconCnt - 1].icon);
            }
        }
        else
        {
            Debug.Log("스프라이트가 없습니다.");
        }

    }
    /// <summary>
    /// icon 파일 불러오기
    /// </summary>
    public void IconAdd(VisualElement _panel)
    {
        var path = EditorUtility.OpenFilePanel("Open icon", Application.dataPath + 
                    "/Resources/sprite", "asset");
        string filename = Path.GetFileName(path);
        if(!string.IsNullOrEmpty(path))
        {
            string iconname = filename.Substring(0, filename.IndexOf('.'));
            IconCreate(_panel, iconname);    
        }
    }
    /// <summary>
    /// icon 등록
    /// </summary>
    public void IconCreate(VisualElement _panel, string _name)
    {
        iconData[iconCnt].icon = Resources.Load<Sprite>("Sprite/" + _name);
        iconData[iconCnt++].rotate = 0;

        // Icon 추가
        Button btn = new Button();
        btn.style.width = 50;
        btn.style.height = 50;
        btn.name = "grid_" + iconCnt.ToString();

        _panel.Add(btn);
        iconData[iconEventCnt].btn = btn;

        iconData[iconEventCnt++].btn.RegisterCallback<ClickEvent>(OnGridCheckButton);
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
    public void OnGridCheckButton(ClickEvent _evt)
    {
        var target = _evt.target as VisualElement;

        // 초기화
        if(!target.ClassListContains("button-grid--check"))
        {
            for(int i = 0; i < iconCnt; i++)
            {
                if(iconData[i].btn.ClassListContains("button-grid--check"))
                {
                    iconData[i].btn.RemoveFromClassList("button-grid--check");
                }
            }

            // 선택
            target.AddToClassList("button-grid--check");

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
    /// Map Window 창 생성.
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
    /// <summary>
    /// Map Window 창에 버튼 생성.
    /// </summary>
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
            buttons[i].btn.style.backgroundImage = new StyleBackground(iconData[0].icon);
        }
    }

    /// <summary>
    /// Panel Size 조절.
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
        {
            return;
        }
        btnTool.Clear();
        window.Close();

        buttons = new ButtonData[201];
    }
    /// <summary>
    /// 버튼 클릭에 대한 이벤트.
    /// </summary>
    public void BtnClick(ClickEvent _evt, Toggle _toggle, int _maxRange)
    {
        if(!_toggle.value)
        {
            var btn = _evt.target as Button;
            int idx = int.Parse(btn.name);
            BtnClickState(btn, idx, selectInt, selecticonRotate);
        }
        else
        {
            int maxRange = _maxRange;
            for (int i = 0; i < maxRange; i++)
            {
                BtnClickState(buttons[i].btn, i, selectInt, selecticonRotate);
            }
        }
    }
    /// <summary>
    /// 버튼 선택 상태.
    /// </summary>
    private void BtnClickState(Button _btn, int _idx, int _selectInt, int _selecticonRotate)
    {
        buttons[_idx].btnIconNum = _selectInt;
        buttons[_idx].btnIconRotate = _selecticonRotate;
        _btn.style.backgroundImage = new StyleBackground(iconData[_selectInt].icon);
        _btn.style.rotate = new Rotate(new Angle(selecticonRotate * 90));
    }
    /// <summary>
    /// 생성한 Grid를 Json 방식으로 저장.
    /// </summary>
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
        maps.idx = new int[maxRange, 2];
        float level = currentLevel > 100 ? ((float)currentLevel / 1000) : ((float)currentLevel / 100);
        string str_lv = level == 0 ?  new string("00") : level.ToString().Substring(level.ToString().IndexOf('.') + 1, 2);
        maps.lv = currentLevel;
        maps.name = "Stage" + "_" + _width + "x" + _height + "_" + "0" + str_lv;
        _fileName.text = maps.name;
        maps.width = _width;
        maps.height = _height;
        
        for(int i = 0; i < maxRange; i++)
        {
            maps.idx[i,0] = buttons[i].btnIconNum;
            maps.idx[i,1] = buttons[i].btnIconRotate;
        }

        string jsonData = JsonConvert.SerializeObject(maps);
        CreateJsonFile(Application.dataPath + "/Resources/Maps", maps.name, jsonData);

        AssetDatabase.Refresh();
        _result.text = "저장 완료";
    }
    /// <summary>
    /// 저장된 Json을 다시 불러오기.
    /// </summary>
    public void LoadBtn(Label _fileName, Label _result, BaseField<int> _currentlv, Toggle _toggle)
    {
        if(btnTool != null && btnTool.childCount != 0)
        {
            _result.text = "Grid를 생성 혹은 삭제 후 다시시도";
            return;
        }

        var path = EditorUtility.OpenFilePanel("Open Level", Application.dataPath + 
                    "/Resources/Maps", "json");
        string filename = Path.GetFileName(path);
        filename = filename.Substring(0, filename.ToString().IndexOf('.'));
        if(!string.IsNullOrEmpty(path))
        {
            LoadLVBtn(_fileName, _result, _currentlv, _toggle, filename);
        }
        _result.text = "불러오기 완료";
    }

    public void LoadLVBtn(Label _fileName, Label _result, BaseField<int> _currentlv, Toggle _toggle, string _filepath)
    {
        Map maps = LoadJsonFile<Map>(Application.dataPath + "/Resources/Maps", _filepath);

        // 버튼 생성
        BtnCreate(_toggle, _result, maps.width, maps.height);
        _fileName.text = maps.name;
        _currentlv.value = maps.lv;

        // grid 배치
        int maxRange = maps.width * maps.height;
        for (int i = 0; i < maxRange; i++)
        {
            BtnClickState(buttons[i].btn, i, maps.idx[i,0], maps.idx[i, 1]);
        }
    }

    private void CreateJsonFile(string _createPath, string _fileName, string _jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", _createPath, _fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(_jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    private T LoadJsonFile<T>(string _loadPath, string _fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", _loadPath, _fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonConvert.DeserializeObject<T>(jsonData);
    }
 
}
