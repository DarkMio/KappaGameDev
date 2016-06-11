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
		_canvas = NodeEditorFramework.NodeEditorSaveManager.LoadNodeCanvas("Assets/Plugins/Node_Editor/Node_Editor/Resources/Saves/Test_Dialogue.asset", false);
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
	void Update () {
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
