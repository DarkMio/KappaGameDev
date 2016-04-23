using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Destroy() { // cya
		Destroy(this);
	}
	
	public void Disable() {
		gameObject.SetActive(false);
	}
}
