using System;
using UnityEngine;
using System.Collections;

public class NavigationSampleScript : MonoBehaviour {
    private NavMeshAgent agent;
    private float time;

    public float timeTillResearch;
    public float movementLength;

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
	    time = Time.timeSinceLevelLoad;
	    if (Math.Abs(timeTillResearch) < 0.1) {
	        timeTillResearch = 3f;
	    }

	    if (Math.Abs(movementLength) < 0.1) {
	        movementLength = 32f;
	    }

	    SearchTarget();
	}

	// Update is called once per frame
	void Update () {
	    float currentTime = (time + timeTillResearch) - Time.timeSinceLevelLoad;
	    Debug.Log(time + timeTillResearch - Time.timeSinceLevelLoad);
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
