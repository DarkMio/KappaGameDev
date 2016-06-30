using UnityEngine;
using System.Collections;


namespace StateManager {
    /**
     * This class is for loading all kinds of managers and instantiating them.
     */
    public class ManagerLoader : MonoBehaviour {
        public GameObject gameManager;



	    // Loads all managers and their states
	    void Awake () {
	        if (GameManager.instance == null) {
	            Instantiate(gameManager);
	        }
	    }

	    // Update is called once per frame
	    void Update () {

	    }
    }
}
