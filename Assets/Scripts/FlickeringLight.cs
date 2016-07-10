using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour {

    private Light l;

    public Vector2 flickerRange;

    public bool moveLight;
    public float maxDistanceLength;
    public float maxDistanceTravel;

    private Vector3 movePosition;
    private float brightness;

	// Use this for initialization
	void Start () {
	    l = GetComponent<Light>();
	}

	// Update is called once per frame
	void Update () {
	    var val = Random.Range(flickerRange.x, flickerRange.y);
	    var final = Vector2.MoveTowards(new Vector2(l.intensity, 0), new Vector2(val, 0), 0.01f);

	    l.intensity = final.x;

	    if (moveLight && Time.frameCount % 5 == 0) {
	        movePosition = new Vector2(maxDistanceLength * Random.value, maxDistanceLength * Random.value);
	    }

        l.transform.localPosition = Vector3.MoveTowards(l.transform.localPosition, new Vector3(movePosition.x, movePosition.y, l.transform.localPosition.z), maxDistanceTravel);


	}
}
