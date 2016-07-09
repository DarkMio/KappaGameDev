using UnityEngine;
using System.Collections;

public class BoxTrigger : MenuTrigger {
	public float distance;
	public GameObject boxMenu;
	public string findingTag;
	public bool isTriggered = false;
	// Use this for initialization
	void Start () {
		triggerDistance = distance;
		if(boxMenu == null && findingTag != null) {
			GameObject.FindWithTag(findingTag);
		}
	}
	
	void Awake() {
		Start();
	}
	
	void OnValidate() { // In editor mode, to tinker with
		Start();
	}
	

	
	public override void TriggerMenu() {
		isTriggered = true;
		if(boxMenu != null) {
			boxMenu.SetActive(true);
		} else {
			Debug.Log("I have no menu to spawn in. :(");
		}
		// Debug.Log(Time.realtimeSinceStartup + " : Hello, I would open a Menu.");
	}
}
