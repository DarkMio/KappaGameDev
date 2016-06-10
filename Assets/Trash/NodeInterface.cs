using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

[ExecuteInEditMode]
public class NodeInterface : MonoBehaviour {

	// Use this for initialization
	void Start () {
		NodeCanvas x = NodeEditorFramework.NodeEditorSaveManager.LoadNodeCanvas("Assets/Plugins/Node_Editor/Resources/Saves/test_case.asset");
		NodeEditor.RecalculateAll (x);
		Debug.Log ("I may have loaded.");
		foreach(Node n in x.nodes) {
			
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

	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("------------------------->");
		Start ();
	}
}
