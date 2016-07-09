using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DwellerRandomDialogue : MonoBehaviour {

    private DialogueInterface dialogue;

    public List<string> dialogueNames;

	// Use this for initialization
	void Start () {
	    dialogue = GetComponent<DialogueInterface>();
	    dialogue.baseCanvas = GameObject.FindGameObjectWithTag("Base UI");
	    dialogue.dialogueField = GameObject.FindGameObjectWithTag("Dialogue Text");
	    int select = Random.Range(0, dialogueNames.Count);
	    dialogue.saveName = dialogueNames[select];
	    dialogue.saveChoice = 0;
        dialogue.Reset();
	}
}
