using System;
using UnityEngine;
using System.Collections;

public class NavigationSampleScript : MonoBehaviour {
    private NavMeshAgent agent;
    private float time;

    public float timeTillResearch;
    public float movementLength;
    public Transform door;
    public bool die;

    public Transform entry;

    private DialogueInterface dialogue;

    private enum SearchEnum {
        UP,
        UP_RIGHT,
        RIGHT,
        DOWN_RIGHT,
        DOWN,
        DOWN_LEFT,
        LEFT,
        UP_LEFT,
    }

    // Use this for initialization
	void Start () {
	    agent = this.GetComponent<NavMeshAgent>();
	    agent.updateRotation = false;
	    time = Time.timeSinceLevelLoad;
	    if (Math.Abs(timeTillResearch) < 0.1) {
	        timeTillResearch = 3f;
	    }

	    if (Math.Abs(movementLength) < 0.1) {
	        movementLength = 32f;
	    }

	    door = GameObject.FindGameObjectWithTag("Door").transform;

	    SearchTarget();

	    dialogue = GetComponent<DialogueInterface>();

	    entry = GameObject.FindGameObjectWithTag("Nav Entry").transform;
	    agent.destination = entry.transform.position;
	    time = Time.timeSinceLevelLoad + 2f;
	}

	// Update is called once per frame
	void Update () {
	    if (dialogue != null && dialogue.isExhausted) {
	        die = true;
	    }
	    if (die) { // if it's time to die, go there and die pls.
	        agent.destination = door.transform.position;
	        Vector2 distance = new Vector2(transform.position.x - door.transform.position.x,
	            transform.position.z - door.transform.position.z);
	        if (distance.magnitude < 5) {
	            var thing = GetComponentInParent<SpawnRandomDweller>();
	            if (thing != null) {
	                thing.Kill();
	            }
	        }
	    }
	    float currentTime = (time + timeTillResearch) - Time.timeSinceLevelLoad;
	    if (currentTime < 0) {
	        time = Time.timeSinceLevelLoad;
	        SearchTarget();
	    }

	}

    void SearchTarget() {
        // Get all enums
        System.Array a = System.Enum.GetValues(typeof(SearchEnum));
        // Select random enum
        SearchEnum value = (SearchEnum) a.GetValue(UnityEngine.Random.Range(0, a.Length));
        // Get the appropiate direction.
        Vector2 direction = GetDirection(value) * movementLength;

        Vector3 basePosition = this.transform.position;
        Vector3 target = basePosition + new Vector3(direction.x, 0, direction.y);

        agent.destination = target;
    }

    Vector2 GetDirection(SearchEnum e) {
        switch (e) {
            case SearchEnum.UP:
                return Vector2.up;
            case SearchEnum.UP_RIGHT:
                return Vector2.up + Vector2.right;
            case SearchEnum.RIGHT:
                return Vector2.right;
            case SearchEnum.DOWN_RIGHT:
                return Vector2.right + Vector2.down;
            case SearchEnum.DOWN:
                return Vector2.down;
            case SearchEnum.DOWN_LEFT:
                return Vector2.down + Vector2.left;
            case SearchEnum.LEFT:
                return Vector2.left;
            case SearchEnum.UP_LEFT:
                return Vector2.up + Vector2.left;
            default:
                return Vector2.zero;
        }
    }
}
