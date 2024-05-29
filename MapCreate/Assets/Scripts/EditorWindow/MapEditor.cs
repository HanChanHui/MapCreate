using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Newtonsoft.Json;

class Map{
    public string name = new string("");
    public int Lv = 0;
    public List<int> idx = new List<int>();

    public Map()
    {

    }
}

public class MapEditor : EditorWindow
{
    /// <summary>
    /// 아이콘 저장
    /// </summary>
    public Sprite[] icon = new Sprite[10];
    /// <summary>
    /// 맵 정보를 담을 버튼
    /// </summary>
    public ButtonData[] buttons = new ButtonData[201];
    /// <summary>
    /// 현재 레벨
    /// </summary>
    public int currentLevel = 0;
    /// <summary>
    /// 타일 버튼을 선택한 인덱스
    /// </summary>
    public int selectInt = 0;
    /// <summary>
    /// 정수 필드
    /// </summary>
    public BaseField<int> levelField;

    public VisualElement btnTool;
    public BaseField<int> widthField;
    public BaseField<int> heightField;

    public VisualElement gridBtnPanel;

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

    public void CreateGUI()
    {
        // TODO 추후 아이콘 방식 변경 -> 플러스 버튼을 누르면 아이콘을 넣을 수 있게
        IconLoad();

        // UXML 와 USS 불러오기
        VisualElement root = rootVisualElement;
        VisualTreeAsset visualTree1 = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/UXML/MapControllor.uxml");
        root.Add(visualTree1.Instantiate());
        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UI Toolkit/USS/MapControllorStyle.uss");
        root.styleSheets.Add(styleSheet);

        btnTool = rootVisualElement.Q<VisualElement>("btnTool");
        widthField = root.Q<BaseField<int>>("width_int");
        heightField = root.Q<BaseField<int>>("height_int");

        // 현재 레벨 인덱스
        levelField = root.Q<BaseField<int>>("IntegerField");
        levelField.RegisterValueChangedCallback((evt) => { LoadLVBtn(evt.newValue); });

        root.Q<Button>("createBtn").clicked += BtnCreate;
        root.Q<Button>("saveBtn").clicked += SaveBtn;
        root.Q<Button>("loadBtn").clicked += LoadBtn;
        
        // grid 버튼 등록 및 이벤트 추가
        gridBtnPanel = root.Q<VisualElement>("gridCheckTool");
        int cnt = 0;
        foreach(var child in gridBtnPanel.Children())
        {
            if(child is Button button)
            {
                gridButtons.Add(button);
                gridButtons[cnt++].RegisterCallback<ClickEvent>(OnGridCheckButton);
            }
        }
        

        //BtnCreate();
        
        // 맨처음에는 1레벨을 불러온다.
        //LoadLVBtn(1);

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
        icon[0] = null;
        icon[1] = Resources.Load<Sprite>("Tile 0");
        icon[2] = Resources.Load<Sprite>("Tile 1");
        icon[3] = Resources.Load<Sprite>("Tile 2");
        icon[4] = Resources.Load<Sprite>("Tile 3");
        icon[5] = Resources.Load<Sprite>("Tile 4");
        icon[6] = Resources.Load<Sprite>("Tile 5");
    }

    private void OnGridCheckButton(ClickEvent _evt)
    {
        var target = _evt.target as VisualElement;

        // 초기화
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



    private void BtnCreate()
    {
        // 버튼들을 생성하고 초기화

        if(btnTool.childCount != 0)
        {
            BtnDestroy();
        }
        
        for(int i = 0; i < widthField.value * heightField.value; i++)
        {
            buttons[i].btn = new Button();
            buttons[i].btn.style.width = 50;
            buttons[i].btn.style.height = 50;
            buttons[i].btn.name = i.ToString();
            btnTool.Add(buttons[i].btn);
            buttons[i].btn.clickable.clickedWithEventInfo += BtnClick;
            buttons[i].btn.style.backgroundImage = new StyleBackground(icon[0]);
        }
    }

    private void BtnDestroy()
    {
        Debug.Log(btnTool.childCount);
        for(int i = 0; i < btnTool.childCount; i++)
        {
            if(buttons[i].btn != null)
            {
                btnTool.Remove(buttons[i].btn);
                buttons[i].btnIconNum = 0;
            }
        }
        buttons = new ButtonData[201];
    }

    private void BtnClick(EventBase _evt)
    {
        var btn = _evt.target as Button;
        int idx = int.Parse(btn.name);
        buttons[idx].btnIconNum = selectInt;
        btn.style.backgroundImage = new StyleBackground(icon[buttons[idx].btnIconNum]);
    }

    public void CreateBtn()
    {
        //_root.Q<Label>("Text").text = "";
        // int check = 0;
        // while(true)
        // {
        //     check++;
        //     if(!Resources.Load("Map/Lv" + check))
        //     {
        //         break;
        //     }
        // }

        // currentLevel = check;
        // levelField.value = currentLevel;
        // for(int i = 0; i < 143; i++)
        // {
        //     buttons[i].btnIconNum = 0;
        //     buttons[i].btn.style.backgroundImage = new StyleBackground(icon[buttons[i].btnIconNum]);
        // }
    }

    public void SaveBtn()
    {
        //TODO Json으로 저장하는 방식 제작.
        Map maps = new Map();

        
        //StreamWriter streamSave = new StreamWriter(Application.dataPath + "/Resources/Maps/Lv" +
        //currentLevel.ToString() + ".json");
        //streamSave.WriteLine("0,1,2,3,4,5,6,7,8,9,10,11");
        maps.Lv = currentLevel;
        maps.name = "Lv" + currentLevel.ToString();
        int cnt = 0;
        for(int i = 0; i < 13; i++)
        {
            //string s = null;
            for(int j = 0; j < 13; j++)
            {
                maps.idx.Add(buttons[cnt].btnIconNum);
                // if(j < 11)
                // {
                //     s += buttons[cnt].btnIconNum + ",";
                // }
                // else
                // {
                //     s += buttons[cnt].btnIconNum + "\n";
                // }
                cnt++;
                //maps.idx.Add(i);
            }
            //streamSave.Write(s);
        }
        string jsonData = JsonUtility.ToJson(maps);
        string path = Path.Combine(Application.dataPath + "/Resources/Maps", "Lv" + currentLevel.ToString());
        File.WriteAllText(path, jsonData);


        //streamSave.Flush();
        //streamSave.Close();

        AssetDatabase.Refresh();
    }

    public void LoadBtn()
    {
        var path = EditorUtility.OpenFilePanel("Open Level", Application.dataPath + 
                    "/Resources/Maps", "json");
        string filename = Path.GetFileName(path);
        if(!string.IsNullOrEmpty(path))
        {
            currentLevel = int.Parse(filename.Substring(2, filename.Length - 6));
            LoadLVBtn(currentLevel);
            levelField.value = currentLevel;
        }
    }

    public void LoadLVBtn(int _lv)
    {
        Map maps = new Map();
        
        if(!Resources.Load("Maps/Lv" + _lv))
        {
            for(int i = 0; i < widthField.value * heightField.value; i++)
            {
                buttons[i].btnIconNum = 0;
                buttons[i].btn.style.backgroundImage = new StyleBackground(icon[buttons[i].btnIconNum]);
            }
            return;
        }

        currentLevel = _lv;
        //string path = "Maps/Lv" + _lv;
        //var data = CacheServerConnectionChangedParameters 

        string path = Path.Combine(Application.dataPath + "/Resources/Maps", "Lv" + currentLevel.ToString());
        string jsonData = File.ReadAllText(path);
        maps = JsonUtility.FromJson<Map>(jsonData);

        int cnt = 0;
        for(int i = 0; i < 12; i++)
        {
            for(int j = 0; j < 12; j++)
            {
                //TODO Json 불러오는 방식 사용.
                //int dataSet = maps[i].idx;
                //buttons[i].btnIconNum = ;
                //buttons[i].btn.style.backgroundImage
            }
        }
    }


}
