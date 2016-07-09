using UnityEngine;
using System.Collections;

public class DialogueOnDestroy : MonoBehaviour {

	void OnDestroy() {
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		go.GetComponentInChildren<PlayerController>().enablePlayercontroller();
	}
}
