using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class MapWindow : EditorWindow
{
    public int Editormap;
    
    public VisualElement btnTool;

    public Button btnCreate;

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        VisualTreeAsset visualTree1 = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/UXML/WindowController.uxml");
        root.Add(visualTree1.Instantiate().Children().FirstOrDefault());

        //root.style.flexGrow = 1;

        
        btnTool = root.Q<VisualElement>("btnToolPanel");

        btnCreate = root.Q<Button>("Create");
    }
}
