using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour {

    private Light l;

    public Vector2 flickerRange;


	// Use this for initialization
	void Start () {
	    l = GetComponent<Light>();
	}

	// Update is called once per frame
	void Update () {
	    var val = Random.Range(flickerRange.x, flickerRange.y);
	    var final = Vector2.MoveTowards(new Vector2(l.intensity, 0), new Vector2(val, 0), 0.01f);

	    l.intensity = final.x;

	}
}
