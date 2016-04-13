using UnityEngine;
using System.Collections;

public class CubeRotator : MonoBehaviour {
    public Vector3 rotation;
    Transform cube;
	// Use this for initialization
	void Start () {
	    cube = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
	    cube.Rotate(rotation * Time.deltaTime);
	}
}
