using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NodeEditorFramework;
using NodeEditorFramework.Standard;
using UnityEditor;

public class CraftingInterface : MonoBehaviour
{
    public NodeCanvas canvas;

    void Awake()
    {
        if (canvas == null)
        {
            return;
        }

        NodeEditor.RecalculateAll(canvas);
        Debug.Log("NodeGraph loaded.");
    }

    public void Reset()
    {
        Awake();
    }
}

[CustomEditor(typeof(CraftingInterface))]
public class CraftingInterfaceInspector : Editor
{
    private string[] _saves;
    private int _choice = 0;
    public override void OnInspectorGUI()
    {

        if (_saves == null)
        {
            _saves = NodeEditorSaveManager.GetSceneSaves();
        }

        if (_saves.Length == 0)
        {
            return;
        }
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Nodegraph:");
        var newChoice = EditorGUILayout.Popup(_choice, _saves);
        EditorGUILayout.EndHorizontal();
        if (newChoice != _choice)
        {
            _choice = newChoice;
            var crafting = target as CraftingInterface;
            crafting.canvas = NodeEditorSaveManager.LoadSceneNodeCanvas(_saves[_choice], false);
            crafting.Reset();
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Force Reloading"))
        {
            var crafting = target as CraftingInterface;
            crafting.canvas = NodeEditorSaveManager.LoadSceneNodeCanvas(_saves[_choice], false);
            crafting.Reset();
        }
        if (GUILayout.Button("Reload saves"))
        {
            _saves = NodeEditorSaveManager.GetSceneSaves();
        }
        EditorGUILayout.EndHorizontal();
        DrawDefaultInspector();
    }
}
