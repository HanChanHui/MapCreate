using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


[CustomEditor(typeof(MapEditors))]
public class MapEditorsEditor : Editor
{
    public VisualTreeAsset TreeAsset;
    private MapEditors _mapEditors;

    // ===============================================
    public VisualElement gridiconPanel;
    public BaseField<int> widthField;
    public BaseField<int> heightField;
    public BaseField<int> currentLevelField;

    public Label fileName;
    public Label result;

    public Toggle spritefullin;

    public override VisualElement CreateInspectorGUI()
    {
        if (!TreeAsset)
            return base.CreateInspectorGUI();

        _mapEditors = (MapEditors)target;

        VisualElement root = new VisualElement();
        TreeAsset.CloneTree(root);
        _mapEditors.Init();
        

        // Add your UI content here
        var inputMScript = root.Q<ObjectField>("unity-input-m_Script");
        inputMScript.AddToClassList("unity-disabled");
        inputMScript.Q(null, "unity-object-field__selector")?.SetEnabled(false);


        widthField = root.Q<BaseField<int>>("width_int");
        heightField = root.Q<BaseField<int>>("height_int");
        gridiconPanel = root.Q<VisualElement>("gridiconPanel");
        _mapEditors.IconLoad(gridiconPanel);

        // 현재 레벨 인덱스
        currentLevelField = root.Q<BaseField<int>>("current_level");
        currentLevelField.RegisterValueChangedCallback((evt) => { _mapEditors.ChangeLevel(evt.newValue); });

        spritefullin = root.Q<Toggle>("spritefullin");
        
        // 결과 라벨
        fileName = root.Q<Label>("fileName_label");
        result = root.Q<Label>("result_label");
        

        root.Q<Button>("grid_add").clicked += ()=> { _mapEditors.IconAdd(gridiconPanel); };
        root.Q<Button>("createBtn").clicked += ()=> { _mapEditors.BtnCreate(spritefullin, result, widthField.value, heightField.value); };
        root.Q<Button>("deleteBtn").clicked += ()=> { _mapEditors.BtnClose(); };
        root.Q<Button>("saveBtn").clicked += ()=> { _mapEditors.SaveBtn(fileName, result, widthField.value, heightField.value); };
        root.Q<Button>("loadBtn").clicked += ()=> { _mapEditors.LoadBtn(fileName, result, currentLevelField, spritefullin); };
        root.Q<Button>("iconrotationBtn").clicked += () => { _mapEditors.IconRotate(); };

        
        _mapEditors.IconEvent(gridiconPanel);
        
        return root;
    }

    private void OnDisable()
    {
       _mapEditors.IconEventDelete();
    }

}