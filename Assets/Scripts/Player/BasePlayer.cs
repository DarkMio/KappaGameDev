using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
using NodeEditorFramework.Standard;

public class BasePlayer : MonoBehaviour {


    public BaseItem[] _inventory = new BaseItem[32];
    private NodeCanvas canvas;
    private IngredientNode iNode;



    void Start () {
         canvas = NodeEditorSaveManager.LoadSceneNodeCanvas("CraftingCanvas", false);

        _inventory[0] = GenerateItem("Green Herb");
        _inventory[1] = GenerateItem("Green Herb");
        _inventory[2] = GenerateItem("Blue Herb");
        _inventory[3] = GenerateItem("Blue Herb");

        foreach (BaseItem baseItem in _inventory) {
            Debug.Log(baseItem);
        }


    }

    BaseItem GenerateItem(string name) {
        IngredientNode iNode = Instantiate(canvas.nodes.Find(x => ((IngredientNode)x).ingredientName == name)) as IngredientNode;
        return new BaseItem(iNode);
    }

	// Update is called once per frame
	void Update () {
    }
}
