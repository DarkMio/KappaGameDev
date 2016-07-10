using System;
using System.Collections.Generic;
using System.Threading;
using NodeEditorFramework;
using NodeEditorFramework.Standard;
#if UNITY_EDITOR
using UnityEditor;
#endif
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

    public List<string> registeredElements {
        get { return search.elements; }
    }

    private SearchState search;

    void Start()
    {
        Awake();
    }

    private void Awake() {
        if (string.IsNullOrEmpty(saveName)) {
            Debug.LogError("CRITICAL: CraftingInterface has no save name");
            return;
        }

        canvas = NodeEditorSaveManager.LoadSceneNodeCanvas(saveName, false);
        NodeEditor.RecalculateAll(canvas);

        if (canvas != null)
        {
            Debug.Log("NodeGraph loaded.");
        } else
        {
            Debug.Log("Good job, no NodeGraph found.");
        }
    }

    /**
     * Method to reset this object properly (for example to invalidate all UI)
     */
    public void Reset() {
        Awake();
    }

    public void Register(string name) {
        if (canvas == null)
        {
            Debug.LogError("No canvas loaded, cannot filter!");
            return;
        }
        if (search == null) {
            List<Node> nodes = canvas.nodes;
            foreach (Node node in nodes) {
                IngredientNode iNode = node as IngredientNode;
                if (iNode == null)
                {
                    continue;
                }
                if (iNode.ingredientName != name)
                {
                    continue;
                }
                List<Node> candidates = CollectOutputNodes(iNode);
                search = new SearchState(candidates, name);
                break;
            }
        } else {
            search.Filter(name);
        }
    }

    private List<Node> CollectOutputNodes(IngredientNode node) {
        List<Node> result = new List<Node>();
        List<NodeOutput> outputs = node.Outputs;
        foreach (NodeOutput opt in outputs) {
            foreach (NodeInput ipt in opt.connections) {
                Node candidate = ipt.body;
                result.Add(candidate);
            }
        }

        return result;
    }

    public Node Retrieve() {
        if (search != null) {
            return search.Retrieve();
        } else {
            return null;
        }
    }

    public void Remove(string remove)
    {
        SearchState Cache = search;
        search = null;
        for(int i = 0; i < Cache.elements.Count; i++)
        {
            if (Cache.elements[i] == remove)
            {
                Cache.elements.Remove(remove);
                break;
            }
        }
        foreach(string s in Cache.elements)
        {
            this.Register(s);
        }
    }

    public void ResetHistory()
    {
        search = null;
    }

    class SearchState {
        public List<Node> nodes;
        public List<string> elements;

        public SearchState(List<Node> nodes, string element) {
            this.nodes = nodes;
            elements = new List<string>();
            elements.Add(element);
        }

        public void Filter(string name) {
            elements.Add(name);
            for (int index = 0; index < nodes.Count; index++) { // go through all nodes
                Node node = nodes[index];
                bool hasIngredient = false;

                foreach (NodeInput ipt in node.Inputs) { // look at all parents and if you've found the element
                    Node parent = ipt.connection.body;   // then signalize it - so it won't be deleted.
                    IngredientNode ingredient = GetIngredientNode(parent);
                    if (ingredient.ingredientName == name) {
                        hasIngredient = true;
                        break;
                    }
                }

                if (!hasIngredient) {
                    nodes.Remove(node);
                }
            }
        }

        // Recursive search - until we crash and burn.
        private IngredientNode GetIngredientNode(Node node) {
            Type nodeType = node.GetType();
            if (nodeType == typeof(IngredientNode)) {
                return (IngredientNode) node;
            } else if (nodeType == typeof(VariableChecker)) {
                return GetIngredientNode((VariableChecker) node.Inputs[0].body);
            } else {
                return null;
            }
        }

        // Currently doesn't check for multiple of the same ingredient.  ¯\_(ツ)_/¯
        public Node Retrieve() {
            if (nodes.Count == 0 || nodes.Count < 1) { // we either have too many, or too little
                return null;
            }
            return nodes[0];
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(CraftingInterface))]
public class CraftingInterfaceInspector : Editor
{
    public override void OnInspectorGUI()
    {
        CraftingInterface crafting = target as CraftingInterface;

        if (crafting.saveChoices == null)
        {
            crafting.saveChoices = NodeEditorSaveManager.GetSceneSaves();
        }

        if (crafting.saveChoices.Length == 0)
        {
            return;
        }
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Nodegraph:");
        var newChoice = EditorGUILayout.Popup(crafting.saveChoice, crafting.saveChoices);
        EditorGUILayout.EndHorizontal();
        if (newChoice != crafting.saveChoice)
        {
            crafting.saveChoice = newChoice;
            crafting.saveName = crafting.saveChoices[crafting.saveChoice];
            crafting.Reset();
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Force Reloading"))
        {
            crafting.Reset();
        }
        if (GUILayout.Button("Reload saves"))
        {
            crafting.saveChoices = NodeEditorSaveManager.GetSceneSaves();
        }
        EditorGUILayout.EndHorizontal();
        DrawDefaultInspector();
    }
}
#endif
