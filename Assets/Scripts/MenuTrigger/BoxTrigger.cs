using UnityEngine;
using System.Collections;

public class BoxTrigger : MenuTrigger {
	public float distance;
	public GameObject boxMenu;
    public GameObject boxMenu2;
	public string findingTag;
	public bool isTriggered = false;
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
		isTriggered = true;
		if(boxMenu != null) {
			boxMenu.SetActive(true);
            if(boxMenu2 != null)
            {
                boxMenu2.SetActive(true);
            }
		} else {
			Debug.Log("I have no menu to spawn in. :(");
		}
		// Debug.Log(Time.realtimeSinceStartup + " : Hello, I would open a Menu.");
	}
}
