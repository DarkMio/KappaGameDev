using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
using NodeEditorFramework.Standard;

public class BasePlayer : MonoBehaviour
{


    private List<BaseItem> _inventory = new List<BaseItem>();
    private NodeCanvas canvas;
    private IngredientNode iNode;



    void Start()
    {
        canvas = NodeEditorSaveManager.LoadSceneNodeCanvas("CraftingCanvas", false);
        Debug.Log(canvas);
        IngredientNode iNode = Instantiate(canvas.nodes.Find(x => ((IngredientNode)x).ingredientName == "Green Herb")) as IngredientNode;
        BaseItem item_ = new BaseItem(iNode);
        _inventory.Add(item_);
        iNode = Instantiate(canvas.nodes.Find(x => ((IngredientNode)x).ingredientName == "Blue Herb")) as IngredientNode;
        item_ = new BaseItem(iNode);
        _inventory.Add(item_);
        iNode = Instantiate(canvas.nodes.Find(x => ((IngredientNode)x).ingredientName == "Green Herb")) as IngredientNode;
        item_ = new BaseItem(iNode);
        _inventory.Add(item_);
        iNode = Instantiate(canvas.nodes.Find(x => ((IngredientNode)x).ingredientName == "Blue Herb")) as IngredientNode;
        item_ = new BaseItem(iNode);
        _inventory.Add(item_);

        Debug.Log(_inventory);
        foreach (BaseItem item in _inventory) {
            Debug.Log(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public List<BaseItem> ReturnPlayerInventory()
    {
        return _inventory;
    }
}
