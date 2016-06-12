using System;
using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Standard;
using NodeEditorFramework.Utilities;
using UnityEngine.UI;

public class NodeInterface : MonoBehaviour {
    public Text mainText;
    public Text decisionText;
    public Text decisionInput;
    private NodeCanvas _canvas;
    private Node _currentNode;
    
	// Use this for initialization
	void Start () {
		_canvas = NodeEditorFramework.NodeEditorSaveManager.LoadNodeCanvas("Assets/Plugins/Node_Editor/Resources/Saves/Test_Dialogue.asset", false);
		NodeEditor.RecalculateAll (_canvas);
        Debug.Log(_canvas);
		Debug.Log ("Nodegraph loaded.");
	    if (_canvas.nodes.Count > 0) {
	        _currentNode = _canvas.nodes[0];
	    }

        SetFieldText();
        /*
        foreach (Node n in x.nodes) {
			
			// n.Calculate ();
			string s = "> " + n.name;
			n.ClearCalculation ();
			foreach (NodeInput ni in n.Inputs) {
				if (ni.connection != null) {
					s += " | INPUT: " + ni.connection.GetValue<float> ();
				}
			}

			foreach (NodeOutput no in n.Outputs) {
				s += " | OUTPUT: " + no.GetValue<float> ();
			}

			// Debug.Log (s);
		}
        */

	}
	
	// Update is called once per frame
    void OnGUI () {
        // Make a background box
        DialogueNode node = (DialogueNode) _currentNode;
        GUI.Box(new Rect(10,10,400,300), "Loader Menu");
        GUI.Label(new Rect(20, 40, 380, 60), node._dialogueText);
        var offset = 100;

        for (int index = 0; index < node._decisions.Count; index++) {
            var decision = node._decisions[index];
            if (GUI.Button(new Rect(20, offset, 380, 20), decision)) {
                GetNext(index);
            }
            offset += 20;
        }
    }

    /**
     * Needs to be worked on so it's useful.
     */
    private void GetNext(int nextChoice) {
        _currentNode = _currentNode.Outputs[nextChoice].connections[0].body;
    }

    public void GetNext() {
        if (_currentNode == null) {
            throw new ArgumentNullException("The current Node is null.");
        }
        int outputNumber;
        try {
            outputNumber = Int32.Parse(decisionInput.text);
        } catch (FormatException) {
            Debug.Log("Invalid number format exception - returning");
            return;
        }

        if (outputNumber >= _currentNode.Outputs.Count || outputNumber < 0) {
            Debug.Log("Invalid output number, might be too high");
            return;
        }
        Debug.Log(outputNumber);
        _currentNode = _currentNode.Outputs[outputNumber].connections[0].body;
        SetFieldText();
    }

    private void SetFieldText() {
        decisionText.text = "";
        for (int index = 0; index < ((DialogueNode) _currentNode)._decisions.Count; index++) {
            string decision = ((DialogueNode) _currentNode)._decisions[index];
            decisionText.text += index + ": " + decision + "\n";
        }
        mainText.text = ((DialogueNode) _currentNode)._dialogueText;
    }

    public void Reset() {
        Start();
    }
}
