using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NodeEditorFramework;
using NodeEditorFramework.Standard;
using NodeEditorFramework.Utilities;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NodeInterface : MonoBehaviour {
    private NodeCanvas _canvas;
    private Node _currentNode;
    
	// Use this for initialization
	void Awake () {
		// _canvas = NodeEditorFramework.NodeEditorSaveManager.LoadNodeCanvas("Assets/Plugins/Node_Editor/Resources/Saves/Boolean_Dialogue.asset", false);
	    _canvas = NodeEditorSaveManager.LoadSceneNodeCanvas("SomeSaveName", true);
		NodeEditor.RecalculateAll (_canvas);
        Debug.Log(_canvas);
		Debug.Log ("Nodegraph loaded.");
	    if (_canvas.nodes.Count > 0) {
	        foreach (Node node in _canvas.nodes) {
                if(node is DialogueRoot) {
                    Traverse(node, null);
	                break;
	            }
	        }
	    }
	}
	
	// Update is called once per frame
    void OnGUI () {
        if (_currentNode == null) {
            Debug.Log("No root found.");
            return;
        }
        // Make a background box
        DialogueNode node = (DialogueNode) _currentNode;
        GUI.Box(new Rect(10,10,400,300), "Dialogue Menu");
        GUI.Label(new Rect(20, 40, 380, 60), node._dialogueText);
        var offset = 100;

        for (int index = 0; index < node._decisions.Count; index++) {
            var decision = node._decisions[index];
            if (decision == "") {
                continue;
            }
            var isEnabled = node.CheckConstraintAt(index);
            var isEnabledText = isEnabled ? "" : " [too dumb]";
            if (GUI.Button(new Rect(20, offset, 380, 20), decision + isEnabledText)) {
                if (isEnabled) {
                    GetNext(index);
                }
            }
            offset += 30;
        }

        if(GUI.Button(new Rect(20, 280, 380, 20), "Reset")) {
            Reset();
        }
    }

    /**
     * Needs to be worked on so it's useful.
     */
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
            Traverse(root.Outputs[0].connections[0].body, nextNode);
        } else if (nextNode is VariableChecker) {
            VariableChecker node = (VariableChecker) nextNode;
            node.Calculate();  // on calulate, it checks the checkable and continues downwards
            Traverse(node.selectedNode, nextNode);
        } else if (nextNode is DialogueNode) {
            _currentNode = nextNode;
        }
    }

    public void Reset() {
        Awake();
    }
}
