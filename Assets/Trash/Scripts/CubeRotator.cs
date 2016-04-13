using UnityEngine;

/** Mio, 13.04.2016
 * CubeRotator rotates cube bya specified vector.
 * Just ... to move anything, really.
 */
public class CubeRotator : MonoBehaviour {
    public Vector3 rotation;
    private Transform _cube;

	void Start () {
	    _cube = GetComponent<Transform>();
	}
	
	void Update () {
	    _cube.Rotate(rotation * Time.deltaTime);
	}
}
