﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


[CustomEditor(typeof(MapEditors))]
public class MapEditorsEditor : Editor
{
    public VisualTreeAsset TreeAsset;
    private MapEditors _mapEditors;

    public VisualElement gridiconPanel;
    public BaseField<int> widthField;
    public BaseField<int> heightField;
    public BaseField<int> currentLevelField;

    public Label fileName;
    public Label result;

    public Toggle spritefullin;

    public List<Button> gridButtons = new List<Button>();

    public int iconEventCnt;

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
        // root.Q<Label>("title").text = "Custom Property Drawer";
        // root.Q<Button>("btn").clickable.clicked += () => Debug.Log("Clicked!");

        widthField = root.Q<BaseField<int>>("width_int");
        heightField = root.Q<BaseField<int>>("height_int");
        gridiconPanel = root.Q<VisualElement>("gridiconPanel");

        // 현재 레벨 인덱스
        currentLevelField = root.Q<BaseField<int>>("current_level");
        currentLevelField.RegisterValueChangedCallback((evt) => { _mapEditors.ChangeLevel(evt.newValue); });

        spritefullin = root.Q<Toggle>("spritefullin");
        spritefullin.RegisterValueChangedCallback((evt) => {_mapEditors.IconFullInState(spritefullin, widthField.value, heightField.value); });
        Debug.Log(spritefullin.value);
        
        // 결과 라벨
        fileName = root.Q<Label>("fileName_label");
        result = root.Q<Label>("result_label");
        

        root.Q<Button>("grid_add").clicked += ()=> { _mapEditors.IconAdd(gridiconPanel, gridButtons, iconEventCnt); };
        root.Q<Button>("createBtn").clicked += ()=> { _mapEditors.BtnCreate(result, widthField.value, heightField.value); };
        root.Q<Button>("deleteBtn").clicked += ()=> { _mapEditors.BtnClose(); };
        root.Q<Button>("saveBtn").clicked += ()=> { _mapEditors.SaveBtn(fileName, result, widthField.value, heightField.value); };
        root.Q<Button>("loadBtn").clicked += ()=> { _mapEditors.LoadBtn(fileName, result, currentLevelField); };

        iconEventCnt = 0;
        if(gridiconPanel != null)
        {
            // grid 버튼 등록 및 이벤트 추가
            foreach(var child in gridiconPanel.Children())
            {
                if(child is Button button)
                {
                    if(button.name != "grid_add")
                    {
                        gridButtons.Add(button);
                        gridButtons[iconEventCnt++].RegisterCallback<ClickEvent>(evt => _mapEditors.OnGridCheckButton(evt, gridButtons));
                    }
                }
            }
        }
        
        
        return root;
    }

    private void OnDisable()
    {
        for(int i = 0; i < gridButtons.Count; i++)
        {
            gridButtons[i].UnregisterCallback<ClickEvent>(evt => _mapEditors.OnGridCheckButton(evt, gridButtons));
        }

        if(_mapEditors != null)
        {
            _mapEditors.BtnClose();
        }
    }
}