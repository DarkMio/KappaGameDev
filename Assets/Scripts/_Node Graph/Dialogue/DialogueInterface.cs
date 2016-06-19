using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using NodeEditorFramework;
using NodeEditorFramework.Standard;
using UnityEditor;
using UnityEngine.UI;

public class DialogueInterface : MonoBehaviour {
    public enum InterfaceState {
        Fresh,
        Dirty
    };

    private InterfaceState state = InterfaceState.Dirty;
    [HideInInspector]
    private NodeCanvas canvas;
    private Node _currentNode;
    public GameObject baseCanvas;
    public GameObject dialogueField;
    public GameObject choiceField;

    [HideInInspector]
    public string saveName;
    [HideInInspector]
    public int saveChoice;
    [HideInInspector]
    public string[] saveFiles;


    private List<GameObject> buttons = new List<GameObject>();

	void Awake () {
	    if (string.IsNullOrEmpty(saveName)) {
	        Debug.LogError("FATAL: No canvas given.");
	        return;
	    }

	    canvas = NodeEditorSaveManager.LoadSceneNodeCanvas(saveName, false);
		NodeEditor.RecalculateAll (canvas);
		Debug.Log ("Nodegraph loaded.");
	    if (canvas.nodes.Count <= 0) {
	        return;
	    }
	    foreach (Node node in canvas.nodes) {
	        if(node is DialogueRoot) {
	            Traverse(node, null);
	            break;
	        }
	    }
	}

    void OnGUI() {
        if (_currentNode == null) {
            Debug.Log("No root found.");
            return;
        }
        BuildUI();

        if (GUI.Button(new Rect(5, 5, 60, 20), "Reset")) {
            Reset();
        }
    }

    void BuildUI() {
        if (state == InterfaceState.Fresh) {
            return;
        }
        state = InterfaceState.Fresh;

        DialogueNode node = (DialogueNode) _currentNode;
        Text element = dialogueField.GetComponent<Text>();
        element.text = node._dialogueText;

        foreach (GameObject button in buttons) {
            Destroy(button);
        }


        for (int i = 0; i < node._decisions.Count; i++) {
            var decision = node._decisions[i];
            if (decision == "") {
                continue;
            }

            BuildButton(node, i, decision);
        }

    }

    /**
     * Builds a neat button given its iteration count.
     */
    private void BuildButton(DialogueNode node, int i, string decision) {
        // figure out of the button is enabled or not.
        var isEnabled = node.CheckConstraintAt(i);
        var isEnabledText = isEnabled ? "" : " [too dumb]";
        GameObject buttonPanel = Instantiate(choiceField) as GameObject;  // build a button
        buttons.Add(buttonPanel);  // register the button
        RectTransform transform = buttonPanel.transform as RectTransform;
        buttonPanel.transform.SetParent(baseCanvas.transform);  // nest it properly in the object hirachy
        transform.anchoredPosition = Vector2.down * (i % 3) * (transform.rect.height + 10);  // move it around a bit

        int rows = (node._decisions.Count - 1)/3;
        int pos = i/3;
        if (rows > 0) { // if we have more than one single row, we have to move it (move it!)
            transform.anchoredPosition += Vector2.left * 340;
            transform.anchoredPosition += Vector2.right * pos * 340;
        }

        Text buttonText = buttonPanel.GetComponentInChildren<Text>();
        if (buttonText == null) {
            Debug.LogError("FATAL: Nodegraph ButtonText is null");
        } else {
            buttonText.text = decision + isEnabledText;
        }
        Button button = buttonPanel.GetComponent<Button>();
        button.onClick.AddListener(() => { // And register an EventListener when the button is clicked.
            state = InterfaceState.Dirty;
            if (isEnabled) {
                GetNext(i);
            }
        });
    }

    private void GetNext(int nextChoice) {
        Debug.Log(nextChoice);
        Node nextNode = _currentNode.Outputs[nextChoice].connections[0].body;
        Traverse(nextNode, null);
    }

    private void Traverse(Node nextNode, Node lastNode) {
        if (nextNode == lastNode) {
            throw new LockRecursionException("Recursion runs over the same object over and over again.");
        }

        if (nextNode is DialogueRoot) {
            DialogueRoot root = (DialogueRoot) nextNode;
            if (root.Outputs.Count > 0 && root.Outputs[0].connections.Count > 0) {
                Traverse(root.Outputs[0].connections[0].body, nextNode);
            } else {
                Debug.LogError("FATAL: Root not connected?");
            }
        } else if (nextNode is VariableChecker) {
            VariableChecker node = (VariableChecker) nextNode;
            node.Calculate();  // on calulate, it checks the checkable and continues downwards
            Traverse(node.selectedNode, nextNode);
        } else if (nextNode is DialogueNode) {
            _currentNode = nextNode;
        }
    }

    public void Reset() {
        state = InterfaceState.Dirty;
        Awake();
    }
}

[CustomEditor(typeof(DialogueInterface))]
public class DialogueInterfaceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogueInterface dialogue = target as DialogueInterface;
        if (string.IsNullOrEmpty(dialogue.saveName)) {
            dialogue.saveFiles = NodeEditorSaveManager.GetSceneSaves();
        }

        if (dialogue.saveFiles.Length == 0) {
            return;
        }


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Nodegraph:");

        var newChoice = EditorGUILayout.Popup(dialogue.saveChoice, dialogue.saveFiles);
        EditorGUILayout.EndHorizontal();
        if (newChoice != dialogue.saveChoice) {
            dialogue.saveChoice = newChoice;
            dialogue.saveName = dialogue.saveFiles[dialogue.saveChoice];
            dialogue.Reset();
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Force Reloading")) {
            dialogue.saveName = dialogue.saveFiles[dialogue.saveChoice];
            dialogue.Reset();
        }
        if (GUILayout.Button("Reload saves")) {
            dialogue.saveFiles = NodeEditorSaveManager.GetSceneSaves();
        }
        EditorGUILayout.EndHorizontal();
        DrawDefaultInspector();
    }
}
