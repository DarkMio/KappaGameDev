using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
using NodeEditorFramework.Standard;

public class BaseCrate : MonoBehaviour
{


    private List<BaseItem> _inventory = new List<BaseItem>();
    private NodeCanvas canvas;
    private IngredientNode iNode;



    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public List<BaseItem> ReturnCrateInventory()
    {
        return _inventory;
    }
}
