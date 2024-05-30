using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Newtonsoft.Json;


class Maped{
    public string name = new string("");
    public int Lv = 0;
    public int width = 0;
    public int height = 0;
    public List<int> idx = new List<int>();

    public Maped()
    {

    }
}


public class MapWindow : EditorWindow
{
   /// <summary>
    /// 아이콘 저장
    /// </summary>
    public Sprite[] icon = new Sprite[10];
    /// <summary>
    /// 맵 정보를 담을 버튼
    /// </summary>
    public ButtonData[] buttons = new ButtonData[0];
    /// <summary>
    /// 현재 레벨
    /// </summary>
    public int currentLevel = 0;
    /// <summary>
    /// 타일 버튼을 선택한 인덱스
    /// </summary>
    public int selectInt = 0;
    /// <summary>
    /// Icon 갯수
    /// </summary>
    public int iconCnt = 0;
    public int iconEventCnt = 0;
    /// <summary>
    /// width/height 설정, 현재 레벨
    /// </summary>
    public BaseField<int> widthField;
    public BaseField<int> heightField;
    public BaseField<int> currentLevelField;
    /// <summary>
    /// 결과 출력
    /// </summary>
    public Label result;
    /// <summary>
    /// Panel들
    /// </summary>
    public VisualElement gridiconPanel;
    public VisualElement btnTool;
    /// <summary>
    /// Grid 버튼들
    /// </summary>
    public List<Button> gridButtons = new List<Button>();



    [MenuItem("Editor/EditorMap")]
    public static void ShowEditor()
    {
        // window Create
        MapEditor window = GetWindow<MapEditor>();

        window.titleContent = new GUIContent("EditorMap");

        Vector2 size = new Vector2(750f, 850f);
        window.minSize = size;
        window.maxSize = size;
    }

    [MenuItem("Editor/MapWindow")]
    public static void ShowMapWindow()
    {
        // window Create
        MapEditor window = GetWindow<MapEditor>();

        window.titleContent = new GUIContent("MapWindow");

        Vector2 size = new Vector2(750f, 850f);
        window.minSize = size;
        window.maxSize = size;
    }
    

    public void CreateGUI()
    {
        IconLoad();

        // UXML 와 USS 불러오기
        VisualElement root = rootVisualElement;
        VisualTreeAsset visualTree1 = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/UXML/MapControllor.uxml");
        root.Add(visualTree1.Instantiate());
        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UI Toolkit/USS/MapControllorStyle.uss");
        root.styleSheets.Add(styleSheet);

        btnTool = root.Q<VisualElement>("btnToolPanel");
        widthField = root.Q<BaseField<int>>("width_int");
        heightField = root.Q<BaseField<int>>("height_int");

        // 현재 레벨 인덱스
        currentLevelField = root.Q<BaseField<int>>("current_level");
        currentLevelField.RegisterValueChangedCallback((evt) => { ChangeLevel(evt.newValue); });

        // 결과 라벨
        result = root.Q<Label>("result_label");

        root.Q<Button>("grid_add").clicked += IconAdd;
        root.Q<Button>("createBtn").clicked += BtnCreate;
        root.Q<Button>("deleteBtn").clicked += BtnDestroy;
        root.Q<Button>("saveBtn").clicked += SaveBtn;
        root.Q<Button>("loadBtn").clicked += LoadBtn;
        
        
        
        // grid 버튼 등록 및 이벤트 추가
        gridiconPanel = root.Q<VisualElement>("gridiconPanel");
        foreach(var child in gridiconPanel.Children())
        {
            if(child is Button button)
            {
                if(button.name != "grid_add")
                {
                    gridButtons.Add(button);
                    gridButtons[iconEventCnt++].RegisterCallback<ClickEvent>(OnGridCheckButton);
                }
            }
        }
        
    }

    private void OnDisable()
    {
         for(int i = 0; i < gridButtons.Count; i++)
        {
            gridButtons[i].UnregisterCallback<ClickEvent>(OnGridCheckButton);
        }
    }

    private void IconLoad()
    {
        icon[iconCnt++] = null;
        icon[iconCnt++] = Resources.Load<Sprite>("Sprite/Tile 0");
        icon[iconCnt++] = Resources.Load<Sprite>("Sprite/Tile 1");
        icon[iconCnt++] = Resources.Load<Sprite>("Sprite/Tile 2");
        icon[iconCnt++] = Resources.Load<Sprite>("Sprite/Tile 3");
        icon[iconCnt++] = Resources.Load<Sprite>("Sprite/Tile 4");
        icon[iconCnt++] = Resources.Load<Sprite>("Sprite/Tile 5");

    }

    /// <summary>
    /// icon 파일 불러오기
    /// </summary>
    private void IconAdd()
    {
        var path = EditorUtility.OpenFilePanel("Open icon", Application.dataPath + 
                    "/Resources/sprite", "asset");
        string filename = Path.GetFileName(path);
        if(!string.IsNullOrEmpty(path))
        {
            string iconname = filename.Substring(0, filename.IndexOf('.'));
            IconCreate(iconname);
                
        }
    }
    /// <summary>
    /// icon 등록
    /// </summary>
    private void IconCreate(string _name)
    {
        icon[iconCnt++] = Resources.Load<Sprite>("Sprite/" + _name);
        Debug.Log(_name);
        
        // Icon 추가
        Button btn = new Button();
        btn.style.width = 50;
        btn.style.height = 50;
        btn.name = "grid_" + iconCnt.ToString();

        gridiconPanel.Add(btn);
        gridButtons.Add(btn);

        gridButtons[iconEventCnt++].RegisterCallback<ClickEvent>(OnGridCheckButton);
        btn.tabIndex = iconCnt - 1;
        btn.style.backgroundImage = new StyleBackground(icon[iconCnt - 1]);
    }

    /// <summary>
    /// 어떤 icon grid 버튼을 선택 했는지
    /// </summary>
    private void OnGridCheckButton(ClickEvent _evt)
    {
        var target = _evt.target as VisualElement;

        // 초기화
        if(!target.ClassListContains("button-grid--check"))
        {
            for(int i = 0; i < gridButtons.Count; i++)
            {
                if(gridButtons[i].ClassListContains("button-grid--check"))
                {
                    gridButtons[i].RemoveFromClassList("button-grid--check");
                }
            }

            // 선택
            target.AddToClassList("button-grid--check");
            selectInt = target.tabIndex;
        }
        else
        {
            // 취소
            target.RemoveFromClassList("button-grid--check");
            selectInt = 0;
        }  
    }

    /// <summary>
    /// Create 버튼을 눌러서 맵 생성.
    /// </summary>
    private void BtnCreate()
    {
        // grid가 이미 있을땐, 생성 불가
        if(btnTool.childCount != 0)
        {
            result.text = "Grid를 삭제 후 다시시도";
            return;
        }
        
        int maxRange = BtnToolPanelSize(widthField.value, heightField.value);
        buttons = new ButtonData[maxRange + 1];
        for(int i = 0; i < maxRange; i++)
        {
            buttons[i].btn = new Button();
            buttons[i].btn.style.width = 50;
            buttons[i].btn.style.height = 50;
            buttons[i].btn.name = i.ToString();
            btnTool.Add(buttons[i].btn);
            buttons[i].btn.clickable.clickedWithEventInfo += BtnClick;
            buttons[i].btn.style.backgroundImage = new StyleBackground(icon[0]);
        }

        MapWindow window = EditorWindow.GetWindow<MapWindow>();
        result.text = "생성 완료";
    }
    /// <summary>
    /// Load를 통해 불러오는 맵 생성
    /// </summary>
    private void BtnCreate(int _width, int _height)
    {
        int maxRange = BtnToolPanelSize(_width, _height);
        buttons = new ButtonData[maxRange + 1];
        for (int i = 0; i < maxRange; i++)
        {
            buttons[i].btn = new Button();
            buttons[i].btn.style.width = 50;
            buttons[i].btn.style.height = 50;
            buttons[i].btn.name = i.ToString();
            btnTool.Add(buttons[i].btn);
            buttons[i].btn.clickable.clickedWithEventInfo += BtnClick;
            buttons[i].btn.style.backgroundImage = new StyleBackground(icon[0]);
        }

        result.text = "생성 완료";
    }

    /// <summary>
    /// Panel Size 조절
    /// </summary>
    private int BtnToolPanelSize(int _width, int _height)
    {
        int width = _width * 50 + (6 * _width);
        btnTool.style.width = width > 750 ? 750 : width;
        btnTool.style.height = _height * 50 + (6 * _height);
        return _width * _height;
    }

    private void ChangeLevel(int _lv)
    {
        currentLevel = _lv;
    }

    private void BtnDestroy()
    {
        Debug.Log(btnTool.childCount);
        btnTool.Clear();
        
        buttons = new ButtonData[201];
    }

    private void BtnClick(EventBase _evt)
    {
        var btn = _evt.target as Button;
        int idx = int.Parse(btn.name);
        buttons[idx].btnIconNum = selectInt;
        btn.style.backgroundImage = new StyleBackground(icon[buttons[idx].btnIconNum]);
    }

    public void SaveBtn()
    {
        if(btnTool.childCount == 0)
        {
            result.text = "Grid를 삭제 후 다시시도";
            return;
        }
        
        //TODO Json으로 저장하는 방식 제작.
        Maped maps = new Maped();

        maps.Lv = currentLevel;
        maps.name = "Lv" + currentLevel.ToString();
        Debug.Log(maps.name);
        maps.width = widthField.value;
        maps.height = heightField.value;

        for(int i = 0; i < widthField.value * heightField.value; i++)
        {
            maps.idx.Add(buttons[i].btnIconNum);
        }
        string jsonData = JsonUtility.ToJson(maps);
        string path = Path.Combine(Application.dataPath + "/Resources/Maps", "Lv" + currentLevel.ToString() + ".json");
        File.WriteAllText(path, jsonData);

        AssetDatabase.Refresh();
        result.text = "저장 완료";
    }

    public void LoadBtn()
    {
        if(btnTool.childCount == 0)
        {
             var path = EditorUtility.OpenFilePanel("Open Level", Application.dataPath + 
                    "/Resources/Maps", "json");
            string filename = Path.GetFileName(path);
            if(!string.IsNullOrEmpty(path))
            {
                LoadLVBtn(path);
                currentLevelField.value = currentLevel;
            }
            result.text = "불러오기 완료";
        }
        else
        {
            result.text = "Grid를 삭제 후 다시시도";
        }
        
    }

    public void LoadLVBtn(string _path)
    {
        Maped maps = new Maped();

        string path = Path.Combine(_path);
        string jsonData = File.ReadAllText(path);
        maps = JsonUtility.FromJson<Maped>(jsonData);

        // 버튼 생성
        BtnCreate(maps.width, maps.height);
        // grid 배치
        int maxRange = maps.width * maps.height;
        for (int i = 0; i < maxRange; i++)
        {
            buttons[i].btnIconNum = maps.idx[i];
            buttons[i].btn.style.backgroundImage = new StyleBackground(icon[maps.idx[i]]);
        }
    }


}
