using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MapEditor : EditorWindow
{
    //[SerializeField]
    //private VisualTreeAsset visualTree1;

    [MenuItem("Editor/EditorMap")]
    public static void ShowEditor()
    {
        // Ã¢ »ý¼º
        MapEditor window = GetWindow<MapEditor>();

        window.titleContent = new GUIContent("EditorMap");

        Vector2 size = new Vector2(600, 600);
        window.minSize = size;
        window.maxSize = size;
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        VisualTreeAsset visualTree1 = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/UXML/MapControllor.uxml");
        root.Add(visualTree1.Instantiate());
    }


}
