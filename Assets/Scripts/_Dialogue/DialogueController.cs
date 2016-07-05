using System;
using UnityEngine;
using System.Collections;

public class DialogueController : MonoBehaviour {

    private string _text;
    public string text { // setter, to extend later.
        set { _text = value; }
    }
}
