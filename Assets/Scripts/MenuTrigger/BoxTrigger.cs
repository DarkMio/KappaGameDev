using UnityEngine;
using System.Collections;

public class BoxTrigger : MenuTrigger {
	public float distance;
	public GameObject boxMenu;

	// Use this for initialization
	void Start () {
		triggerDistance = distance;
    }
	
	void Awake() {
		Start();
	}
	
	void OnValidate() { // In editor mode, to tinker with
		Start();
	}
	

	
	public override void TriggerMenu() {
		if(boxMenu != null) {
			boxMenu.SetActive(true);
		} else {
			Debug.Log("I have no menu to spawn in. :(");
		}
		// Debug.Log(Time.realtimeSinceStartup + " : Hello, I would open a Menu.");
	}
}
