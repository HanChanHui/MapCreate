using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;


[CustomEditor(typeof(MapEditor))]
public class MapEditorsEditor : Editor
{
    private MapEditor _mapEditor;

    public VisualTreeAsset TreeAsset;
    public VisualElement scalePanel;
    public VisualElement tileSizePanel;
    public VisualElement tilePosPanel;



    // // ===============================================
    // public VisualElement gridiconPanel;
    // public BaseField<int> widthField;
    // public BaseField<int> heightField;
    // public BaseField<int> currentLevelField;

    // public Label fileName;
    // public Label result;

    // public Toggle spritefullin;

    public override VisualElement CreateInspectorGUI()
    {
        if (!TreeAsset)
        {
            return base.CreateInspectorGUI();
        }
        _mapEditor = (MapEditor)target;

        VisualElement root = new VisualElement();
        TreeAsset.CloneTree(root);
        
        scalePanel = root.Q<VisualElement>("ScalePanel");
        tileSizePanel = root.Q<VisualElement>("TileSizePanel");
        tilePosPanel = root.Q<VisualElement>("TilePosPanel");
        SliderRegister(scalePanel);
        SliderRegister(tileSizePanel);
        SliderRegister(tilePosPanel);
        
        
        // _mapEditors.Init();
        // // Add your UI content here
        // var inputMScript = root.Q<ObjectField>("unity-input-m_Script");
        // inputMScript.AddToClassList("unity-disabled");
        // inputMScript.Q(null, "unity-object-field__selector")?.SetEnabled(false);


        // widthField = root.Q<BaseField<int>>("width_int");
        // heightField = root.Q<BaseField<int>>("height_int");
        // gridiconPanel = root.Q<VisualElement>("gridiconPanel");
        // _mapEditors.IconLoad(gridiconPanel);

        // // 현재 레벨 인덱스
        // currentLevelField = root.Q<BaseField<int>>("current_level");
        // currentLevelField.RegisterValueChangedCallback((evt) => { _mapEditors.ChangeLevel(evt.newValue); });

        // spritefullin = root.Q<Toggle>("spritefullin");
        
        // // 결과 라벨
        // fileName = root.Q<Label>("fileName_label");
        // result = root.Q<Label>("result_label");
        
        
        
        // //root.Q<Button>("grid_add").clicked += ()=> { _mapEditors.IconAdd(gridiconPanel); };
        // root.Q<Button>("createBtn").clicked += ()=> { _mapEditors.BtnCreate(spritefullin, result, widthField.value, heightField.value); };
        // root.Q<Button>("deleteBtn").clicked += ()=> { _mapEditors.BtnClose(); };
        // root.Q<Button>("saveBtn").clicked += ()=> { _mapEditors.SaveBtn(fileName, result, widthField.value, heightField.value); };
        // root.Q<Button>("loadBtn").clicked += ()=> { _mapEditors.LoadBtn(fileName, result, currentLevelField, spritefullin); };
        // root.Q<Button>("iconrotationBtn").clicked += () => { _mapEditors.IconRotate(); };


        // //-----------------------------------------
            
        // //-----------------------------------------
        
        // _mapEditors.IconEvent(gridiconPanel);
        
        return root;
    }

    // private void OnDisable()
    // {
    //    _mapEditors.IconEventDelete();
    // }

    private void SliderRegister(VisualElement _panel)
    {
        if(_panel != null)
        {
            foreach(var child in _panel.Children())
            {
                if(child is SliderInt slider)
                {
                    var secondChild = slider.Children().ElementAtOrDefault(1);
                    if(secondChild is Label label)
                    {
                        slider.RegisterCallback<ChangeEvent<int>>((evt) => { SliderValueChanged(evt, label); });
                    }
                }
            }
        }
    }

    void SliderValueChanged(ChangeEvent<int> value, Label _sliderNumber)
    {
        int v = value.newValue;
        _sliderNumber.text = v.ToString();
    }

}