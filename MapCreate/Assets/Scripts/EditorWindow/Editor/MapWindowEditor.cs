using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(MapWindow))]
public class MapWindowEditor : Editor
{
    public VisualTreeAsset TreeAsset;
    private MapWindow _mapWindow;

    public override VisualElement CreateInspectorGUI()
    {
        if (!TreeAsset)
            return base.CreateInspectorGUI();

        _mapWindow = (MapWindow)target;

        VisualElement root = new VisualElement();
        TreeAsset.CloneTree(root);

        // Add your UI content here
        var inputMScript = root.Q<ObjectField>("unity-input-m_Script");
        inputMScript.AddToClassList("unity-disabled");
        inputMScript.Q(null, "unity-object-field__selector")?.SetEnabled(false);
        // root.Q<Label>("title").text = "Custom Property Drawer";
        // root.Q<Button>("btn").clickable.clicked += () => Debug.Log("Clicked!");
        
        return root;
    }
}