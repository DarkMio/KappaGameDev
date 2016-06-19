using NodeEditorFramework;
using UnityEditor;
using UnityEngine;

public class CraftingInterface : MonoBehaviour {
    private NodeCanvas canvas;

    /**
     * To make the custom inspector for graph selection consistent
     */
    [HideInInspector]
    public int saveChoice = 0;
    [HideInInspector]
    public string[] saveChoices;
    [HideInInspector]
    public string saveName;

    private void Awake() {
        if (string.IsNullOrEmpty(saveName)) {
            return;
        }

        canvas = NodeEditorSaveManager.LoadSceneNodeCanvas(saveName, false);
        NodeEditor.RecalculateAll(canvas);
        Debug.Log("NodeGraph loaded.");
    }

    /**
     * Method to reset this object properly (for example to invalidate all UI)
     */
    public void Reset() {
        Awake();
    }
}

[CustomEditor(typeof(CraftingInterface))]
public class CraftingInterfaceInspector : Editor {
    public override void OnInspectorGUI() {
        CraftingInterface crafting = target as CraftingInterface;

        if (crafting.saveChoices == null) {
            crafting.saveChoices = NodeEditorSaveManager.GetSceneSaves();
        }

        if (crafting.saveChoices.Length == 0) {
            return;
        }
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Nodegraph:");
        var newChoice = EditorGUILayout.Popup(crafting.saveChoice, crafting.saveChoices);
        EditorGUILayout.EndHorizontal();
        if (newChoice != crafting.saveChoice) {
            crafting.saveChoice = newChoice;
            crafting.Reset();
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Force Reloading")) {
            crafting.Reset();
        }
        if (GUILayout.Button("Reload saves")) {
            crafting.saveChoices = NodeEditorSaveManager.GetSceneSaves();
        }
        EditorGUILayout.EndHorizontal();
        DrawDefaultInspector();
    }
}