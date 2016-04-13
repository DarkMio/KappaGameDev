using UnityEngine;
using System.Collections;

/** Mio, 13.04.2016
 * Smoothly moves the player via axis inputs,
 * while staying on the fixed unit grid.
 */
public class PlayerController : MonoBehaviour {
    private Vector3 _movement;
    public int speed; // in Pixels per Second...

	void Start () {
	    _movement = Vector3.zero;
	}
	
	void Update () {
	    _movement.z += Input.GetAxis("Vertical") * speed * Time.deltaTime;
        _movement.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
	}
    
    private void LateUpdate() {
        // Clamp the current movement
        Vector3 clamped_movement = new Vector3((int)_movement.x, 0, (int)_movement.z);
        // Check if a movement is needed (more than 1px move)
        if (clamped_movement.magnitude >= 1.0f) {
            // Update velocity, removing the actual movement
            _movement = _movement - clamped_movement;
            if (clamped_movement != Vector3.zero){
                // Move to the new position
                transform.position = transform.position + clamped_movement;
            }
        }
    }
}
