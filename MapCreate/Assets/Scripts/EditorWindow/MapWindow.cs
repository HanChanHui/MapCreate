using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class MapWindow : EditorWindow
{
    public int Editormap;
    
    public VisualElement btnTool;


    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        VisualTreeAsset visualTree1 = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/UXML/WindowController.uxml");
        root.Add(visualTree1.Instantiate());

        btnTool = root.Q<VisualElement>("btnToolPanel");

    }
}
