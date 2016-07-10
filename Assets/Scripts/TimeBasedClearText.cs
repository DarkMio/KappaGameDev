using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeBasedClearText : MonoBehaviour {

    private Text thing;
    private string original;
    private float time;
    private bool clearFlag = false;

    public float timeout;

	// Use this for initialization
	void Start () {
	    thing = GetComponent<Text>();
	    original = String.Copy(thing.text);
	    time = Time.timeSinceLevelLoad;
	}

	// Update is called once per frame
	void Update () {
	    if (original != thing.text) {
	        if (!clearFlag) {
	            clearFlag = true;
	            time = Time.timeSinceLevelLoad;
	        }

	        if (time + timeout < Time.timeSinceLevelLoad) {
	            thing.text = "";
	            clearFlag = false;
	        }
        }
	}
}
