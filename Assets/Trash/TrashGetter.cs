using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Standard;

public class TrashGetter : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    void OnGUI() {
        if (GUI.Button(new Rect(5, 85, 60, 20), "TEST")) {
            CraftingInterface c = GetComponent<CraftingInterface>();
            Debug.Log(c);
            c.Register("Penis Herb");
            c.Register("Vagina Herb");
            Node node = c.Retrieve();
            IngredientNode iNode = node as IngredientNode;
            if (iNode != null) {
                Debug.Log(iNode.ingredientName);
            }
        }
    }
}
