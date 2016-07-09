using UnityEngine;
using System.Collections;

public class SpawnRandomDweller : MonoBehaviour {
    public GameObject dwellerPrefab;
    public float timeToSpawn;
    public float timeTillRespawn;

    private GameObject dwellerInstance;
    private float time;

	// Use this for initialization
	void Start () {
	    time = Time.timeSinceLevelLoad;
	}

	// Update is called once per frame
	void Update () {
	    var currentTime = (time + timeTillRespawn) - Time.timeSinceLevelLoad;
	    if (dwellerInstance != null) {
	        return;
	    }
	    if (currentTime < 0) {
	        Spawn();
	    }
	}

    /* Old directive to spawn / kill / debug dwellers
    void OnGUI() {
        #if UNITY_EDITOR
        if (GUI.Button(new Rect(5, 25, 60, 20), "SPAWN!")) {
            Spawn();
        }

        if (GUI.Button(new Rect(5, 45, 60, 20), "KILL!")) {
            Kill();
        }
        #endif
    }
    */

    void Spawn() {
        Destroy(dwellerInstance);
	    time = Time.timeSinceLevelLoad;

        dwellerInstance = Instantiate(dwellerPrefab);
        dwellerInstance.transform.parent = transform;
        dwellerInstance.transform.position = transform.position;
    }

    public void Kill() {
        Destroy(dwellerInstance);
        time = Time.timeSinceLevelLoad;
    }
}
