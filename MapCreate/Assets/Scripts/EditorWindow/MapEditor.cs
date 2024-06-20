using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

class Map{
    [JsonProperty("name")]
    public string name { get; set; }
    [JsonProperty("lv")]
    public int lv { get; set; }
    [JsonProperty("width")]
    public int width { get; set; }
    [JsonProperty("height")]
    public int height { get; set; }
    [JsonProperty("idx")]
    public List<List<int>> idx { get; set; }

    public Map()
    {
        this.name = "";
        this.lv = 0;
        this.width = 0;
        this.height = 0;
        this.idx = new List<List<int>>();
    }
}

public class MapEditor : MonoBehaviour
{
    public float worldTime = 0f;
    public float deltaTime = 0f;
    public float fps = 0;

    //[Header("Scale")]
    [Range(0, 100)] public int scaleX;
    [Range(0, 100)] public int scaleY;

    //[Header("TileSize")]
    [Range(0, 120)] public int tileSizeX;
    [Range(0, 120)] public int tileSizeY;

    //[Header("TilePos")]
    [Range(0, 100)] public int tilePosX;
    [Range(0, 100)] public int tilePosY;

    //[Header("Color")]
    public int colorR;
    public int colorG;
    public int colorB;
    public int colorA;

    //[Header("Img")]
    public int imgIdx;
    public int pickingIdxX;
    public int pickingIdxY;

    //[Header("MouseOver")]
    public int mouseIdxX;
    public int mouseIdxY;

    /// <summary>
    /// 맵 정보를 담을 버튼
    /// </summary>
    public ButtonData[] buttons = new ButtonData[100];
    //public ImgData[] imgs = new ImgData[];

    /// <summary>
    /// 아이콘 / 회전값 저장
    /// </summary>
    public IconData[] iconData = new IconData[10];
    //public Dictionary<int , IconData[]> iconData = new Dictionary<int, IconData[]>();

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

    private Map maps = new Map();

    // -------------------------------------
    public List<Button> selectedButtons = new List<Button>();


    private Dictionary<int , GameObject> modelPrefabs = new Dictionary<int, GameObject>();

    private void Update() 
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        worldTime += Time.deltaTime;
        fps = 1.0f / deltaTime;
    }

    public void Init()
    {
        currentLevel = 0;
        selectInt = 0;
        selecticonRotate = 0;
        iconEventCnt = 0;
        iconCnt = 0;
        modelPrefabs[1] = Resources.Load<GameObject>("Base_0");
        modelPrefabs[2] = Resources.Load<GameObject>("Base_1");
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
                    iconData[iconEventCnt].btn = button;
                    iconData[iconEventCnt++].btn.RegisterCallback<ClickEvent>(OnGridCheckButton);
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

            foreach(Sprite sprite in sprites)
            {
                iconData[iconCnt].icon = sprite;
                iconData[iconCnt].rotate = 0;
                IconCreate(_panel, sprite.name);
            }
        }
        else
        {
            Debug.Log("스프라이트가 없습니다.");
        }
       
    }

    // icon 추가 기능
    /*
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
    }*/

    public void IconCreate (VisualElement _panel, string _name)
    {
        Button btn = new Button();
        btn.style.width = 50;
        btn.style.height = 50;
        btn.name = _name;

        _panel.Add(btn);
        btn.AddToClassList("button-grid");

        string targetIndex = _name.ToString().Substring(_name.ToString().IndexOf('_') + 1);
        btn.tabIndex = int.Parse(targetIndex);
        btn.style.backgroundImage = new StyleBackground(iconData[iconCnt].icon);
        iconCnt++;
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
        var target = _evt.target as Button;

        // 초기화
        if(!target.ClassListContains("button-grid--check"))
        {
            for(int i = 0; i < iconCnt; i++)
            {
                Debug.Log(iconData[i].btn + ", " + iconData[i].rotate);
                if(iconData[i].btn.ClassListContains("button-grid--check"))
                {
                    iconData[i].btn.RemoveFromClassList("button-grid--check");
                }
            }

            // 선택
            target.AddToClassList("button-grid--check");

            selectInt = target.tabIndex;
            Debug.Log(selectInt);
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
        //maps = new Map();
        maps.width = _width;
        maps.height = _height;
        
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
            int index = i;
            buttons[i].btn = new Button();
            buttons[i].btn.style.width = 50;
            buttons[i].btn.style.height = 50;
            buttons[i].btn.name = i.ToString();
            _btnTool.Add(buttons[i].btn);

            buttons[i].btn.RegisterCallback<ClickEvent>(evt => {BtnClick(evt, _toggle, maxRange);});
            buttons[i].btn.style.backgroundImage = new StyleBackground(iconData[0].icon);
        }

        window.btnCreate.clicked += ()=> {LoadLevel();};
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
        maps.idx = new List<List<int>>(maxRange);
        for (int i = 0; i < maxRange; i++)
        {
             maps.idx.Add(new List<int> { buttons[i].btnIconNum, buttons[i].btnIconRotate });
        }

        float level = currentLevel > 100 ? ((float)currentLevel / 1000) : ((float)currentLevel / 100);
        string str_lv = level == 0 ?  new string("00") : level.ToString().Substring(level.ToString().IndexOf('.') + 1, 2);
        maps.lv = currentLevel;
        maps.name = "Stage" + "_" + _width + "x" + _height + "_" + "0" + str_lv;
        _fileName.text = maps.name;

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
        //Map maped = LoadJsonFile<Map>(Application.dataPath + "/Resources/Maps", _filepath);
        maps = LoadJsonFile<Map>(Application.dataPath + "/Resources/Maps", _filepath);

        // 버튼 생성
        BtnCreate(_toggle, _result, maps.width, maps.height);
        _fileName.text = maps.name;
        _currentlv.value = maps.lv;

        int idx = 0;   
        foreach(List<int> innerList in maps.idx)
        {
            BtnClickState(buttons[idx].btn, idx, innerList[0], innerList[1]);
            idx++;
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
        string filePath = string.Format("{0}/{1}.json", _loadPath, _fileName);
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found", filePath);
        }

        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            string jsonData = Encoding.UTF8.GetString(data);

            Debug.Log($"Loaded JSON data: {jsonData}");
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }

    void LoadLevel()
    {
        int width = maps.width;
        int height = maps.height;

        GameObject gridParent = new GameObject("Grids");
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int index = maps.idx[i * width + j][0];
                int rotation = maps.idx[i * width + j][1];

                if (modelPrefabs.ContainsKey(index))
                {
                    Vector3 position = new Vector3(i, 0, j);
                    Quaternion rotationQuaternion = Quaternion.Euler(0, rotation * 90, 0);
                    GameObject gridmodel = Instantiate(modelPrefabs[index], position, rotationQuaternion);
                    gridmodel.transform.parent = gridParent.transform;
                }
            }
        }
    }
}
