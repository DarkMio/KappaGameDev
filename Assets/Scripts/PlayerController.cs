using UnityEngine;
using UnityEngine.UI;

/** Mio, 13.04.2016
 * Smoothly moves the player via axis inputs,
 * while staying on the fixed unit grid.
 */
 [ExecuteInEditMode]
public class PlayerController : MonoBehaviour {
    public bool playerInput = true;
    private Vector3 _movement;
    public bool debugDraws = true;
    public int speed; // in Pixels per Second...
    [TooltipAttribute("Max Distance within a context-menu snaps.")]
    public float contextDist;
	public AudioClip openChest;
	public AudioSource audioSource;

	void Start () {
	    _movement = Vector3.zero;
	}

	void Update () {
        if(playerInput) {
            _movement.z += Input.GetAxis("Vertical") * speed * Time.deltaTime;
            _movement.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            if(Input.GetButtonDown("Submit")){
                checkContextMenu();
				checkNPCDialog ();
            }
        }
        /*
		if ((Time.time - dialogStartTime) > dialogTime) {
			dialogWindow.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (0, 1000, 0);
		}
        */
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

    /**
     * Checks if there is any context menu around to open
     */
    private GameObject checkContextMenu() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Context Menu");
        GameObject closest = null;
        float closestDist = float.MaxValue;
        foreach(GameObject obj in objs) {
            if(closest == null) { // to have something to compare with
                closest = obj;
                closestDist = Vector3.Distance(transform.position, obj.transform.position);
            }

            // Get the distance
            float compareDist = Vector3.Distance(transform.position, obj.transform.position);
            if(compareDist < closestDist) { // compare and overwrite it
                closest = obj;
                closestDist = compareDist;
            }
        }
        if(debugDraws) {
            Debug.DrawLine(transform.position, closest.transform.position, Color.red, 2f, false);
        }

        MenuTrigger trigger = closest.GetComponent<MenuTrigger>();
        if(trigger != null && closestDist <= trigger.triggerDistance) { // trigger tells the trigger distance
            trigger.TriggerMenu();
            playerInput = false;
			// audioSource.PlayOneShot (openChest);
			// Debug.Log ("Chest opened");
        } else if(trigger == null) {
            Debug.Log("Trigger -> null");
        }
        return closest;
    }

    public void enablePlayercontroller() {
        playerInput = true;
    }

	private GameObject checkNPCDialog() {
		GameObject[] objs = GameObject.FindGameObjectsWithTag("NPC");
		GameObject closest = null;
		float closestDist = float.MaxValue;
		foreach(GameObject obj in objs) {
			if(closest == null) { // to have something to compare with
				closest = obj;
				closestDist = Vector3.Distance(transform.position, obj.transform.position);
			}

			// Get the distance
			float compareDist = Vector3.Distance(transform.position, obj.transform.position);
			if(compareDist < closestDist) { // compare and overwrite it
				closest = obj;
				closestDist = compareDist;
			}
		}
        /*
		if(debugDraws) {
			Debug.DrawLine(transform.position, closest.transform.position, Color.red, 2f, false);
		}
        */

		return closest;
	}
}
