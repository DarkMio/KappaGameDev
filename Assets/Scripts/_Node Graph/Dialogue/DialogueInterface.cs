using System;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using NodeEditorFramework;
using NodeEditorFramework.Standard;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;

public class DialogueInterface : MenuTrigger {
    public enum InterfaceState {
        Fresh,
        Dirty,
    };

    private InterfaceState state = InterfaceState.Dirty;
    private NodeCanvas canvas;
    private Node _currentNode;
    public GameObject baseCanvas;
    public GameObject dialogueField;
    public GameObject dialoguePrefab;
    public GameObject choiceField;
    public bool isExhausted = false; // no more dialogue.

    // Following block is for the typing magic.
    public float interval = 4;
    public string cursorChar = "|";
    public int charsPerFrame = 4;
    public float typeInterval = 100;
    public float timeStampType = Time.time;
    public float timeStampBlink = Time.time;
    private GUITextManager guiManager;
    private AudioSource audio;

    /**
     * To make the custom inspector for graph selection consistent
     */
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
	    foreach (Node node in canvas.nodes) { // searching for appropiate root
	        if(node is DialogueRoot) {
	            Traverse(node, null);
	            break;
	        }
	    }
	    isExhausted = false;
        guiManager = new GUITextManager(this);
        audio = GetComponent<AudioSource>();
        dialogueField = GameObject.FindGameObjectWithTag("Dialogue Text");
        triggerDistance = 50;
	}

    void OnGUI() {
        if (_currentNode == null) {
            throw new NullReferenceException("There is no root present inside the node graph.");
        }
        BuildUI();

        #if UNITY_EDITOR  // debug button, compile directive for inside unity editor.
        if (GUI.Button(new Rect(5, 5, 60, 20), "Reset")) {
            Reset();
        }
        #endif
    }

    void OnDestroy() {
        Destroy(dialogueField);
    }

    public override void TriggerMenu() {
        Debug.Log("Triggered");
        if(dialogueField == null) {
            dialogueField = Instantiate(dialoguePrefab);
            dialogueField.transform.SetParent(baseCanvas.transform, false);
            // dialogueField.GetComponent<RectTransform>().SetParent(baseCanvas.GetComponent<RectTransform>());
            // dialogueField.transform.parent = baseCanvas.transform;
            // dialogueField.transform.localPosition = Vector3.zero;
        }

    }

    void BuildUI() {
        if(dialogueField == null || !dialogueField.activeSelf) {
            return;
        }
        // Set dialogue text
        var element = dialogueField.GetComponentInChildren<Text>();
        guiManager.text = ((DialogueNode)_currentNode)._dialogueText;
        guiManager.Update();
        element.text = guiManager.currentText;
        if(guiManager.isWriting) {
            if(!audio.isPlaying) {
                GetComponent<AudioSource>().Play();
            }
        } else {
            if(audio.isPlaying) {
                GetComponent<AudioSource>().Stop();
            }
        }

        if (state == InterfaceState.Fresh) {
            return;
        }
        state = InterfaceState.Fresh;
        if (baseCanvas == null || dialogueField == null || choiceField == null) {
            Debug.LogError("FATAL: DialogueInterface has no sufficient references on baseCanvas, dialogueField or choiceField");
            return;
        }

        var node = _currentNode as DialogueNode;

        // delete all buttons at first
        foreach (GameObject button in buttons) {
            Destroy(button);
        }

        // careful: Outputs is the amount of VALID decisions
        // while decisions contains all node decisions - which could be inactive due to disabled outputs
        // so we want the iteration to go along the Outputs.Count - but we want the decision to that output,
        // which is seperate by implementation
        for (var i = 0; i < node.Outputs.Count; i++) {
            var decision = node._decisions[i];
            if (decision == "") {
                continue;
            }

            BuildButton(node, i, decision);
        }

        if (node.Outputs.Count == 0) {
            isExhausted = true;
        }

    }

    /**
     * Builds a neat button given its iteration count.
     */
    private void BuildButton(DialogueNode node, int i, string decision) {
        // figure out of the button is enabled or not.
        var isEnabled = node.CheckConstraintAt(i);
        var isEnabledText = isEnabled ? "" : " [too dumb]";
        var buttonPanel = Instantiate(choiceField) as GameObject;  // build a button
        buttons.Add(buttonPanel);  // register the button
        var rectTransform = buttonPanel.transform as RectTransform;
        buttonPanel.transform.SetParent(baseCanvas.transform);  // nest it properly in the object hirachy
        // ReSharper disable once PossibleNullReferenceException
        rectTransform.anchoredPosition = Vector2.down * (i % 3) * (rectTransform.rect.height + 10) * 0.5f;  // move it around a bit

        var rows = (node.Outputs.Count - 1)/3;
        var pos = i/3;
        if (rows > 0) { // if we have more than one single row, we have to move it (move it!)
            rectTransform.anchoredPosition += Vector2.left * 340;
            rectTransform.anchoredPosition += Vector2.right * pos * 340;
        }

        var buttonText = buttonPanel.GetComponentInChildren<Text>();
        if (buttonText == null) {
            Debug.LogError("FATAL: Nodegraph ButtonText is null");
        } else {
            buttonText.text = decision + isEnabledText;
        }
        var button = buttonPanel.GetComponent<Button>();
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
            var root = (DialogueRoot) nextNode;
            if (root.Outputs.Count > 0 && root.Outputs[0].connections.Count > 0) {
                Traverse(root.Outputs[0].connections[0].body, nextNode);
            } else {
                Debug.LogError("FATAL: Root not connected?");
            }
        } else if (nextNode is VariableChecker) {
            var node = (VariableChecker) nextNode;
            node.Calculate();  // on calulate, it checks the checkable and continues downwards
            // ReSharper disable once TailRecursiveCall
            Traverse(node.selectedNode, nextNode);
        } else if (nextNode is DialogueNode) {
            _currentNode = nextNode;
        } else if (nextNode is RandomBranchNode) {
            var node = (RandomBranchNode) nextNode;
            var otp = nextNode.Outputs[UnityEngine.Random.Range(0, nextNode.Outputs.Count)];
            Traverse(otp.connections[0].body, nextNode);
        }
    }

    public void Reset() {
        Awake();
        state = InterfaceState.Dirty;
    }

    public void ResetCompletely(string saveName) {
        saveFiles = NodeEditorSaveManager.GetSceneSaves();
        this.saveName = saveName;
        for (int i = 0; i < saveFiles.Length; i++) {
            if (saveFiles[i] == saveName) {
                saveChoice = i;
                break;
            }
        }
        Awake();
        state = InterfaceState.Dirty;
    }

    private class GUITextManager {
        private DialogueInterface parent;
        string _finalText;
        public string currentText;
        private int charCount;

        public bool isWriting = false;

        private bool blinkTextCharacter = false;
        public string text {
            set {
                if (_finalText != value) {
                    blinkTextCharacter = false;
                    isWriting = false;
                    _finalText = value;
                    currentText = "";
                    charCount = 0;
                }
            }

            get {
                return _finalText;
            }
        }

        public GUITextManager(DialogueInterface parent) {
            this.parent = parent;
            _finalText = "";
            parent.interval = 0.5f;
            parent.cursorChar = "|";
            parent.charsPerFrame = 2;
            parent.typeInterval = 1.4f;
            parent.timeStampType = Time.time;
            parent.timeStampBlink = Time.time;
        }

        public void Update() {
            Type();
            BlinkText();
            isWriting = _finalText.Length > currentText.Length;
        }

        void Type() {
            parent.timeStampType = Time.time;
            if (blinkTextCharacter) {
                currentText = currentText.Substring(0, currentText.Length - parent.cursorChar.Length);
            }
            int toWrite = Mathf.RoundToInt(UnityEngine.Random.value * (parent.charsPerFrame+1) % parent.charsPerFrame); // Random a range of characters to write
            charCount += toWrite;
            currentText = _finalText.Substring(0, Math.Min(charCount, _finalText.Length));
            if (blinkTextCharacter) {
                currentText += parent.cursorChar;
            }
        }

        void BlinkText() {
            if(Time.time - parent.timeStampBlink >= parent.interval) {
                parent.timeStampBlink = Time.time;
                if(!blinkTextCharacter) {
                    currentText = currentText + parent.cursorChar;
                } else {
                    currentText = currentText.Substring(0, currentText.Length - parent.cursorChar.Length);
                }
                blinkTextCharacter = !blinkTextCharacter;
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DialogueInterface))]
public class DialogueInterfaceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var dialogue = target as DialogueInterface;
        if (dialogue == null) {
            return;
        }
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
#endif
