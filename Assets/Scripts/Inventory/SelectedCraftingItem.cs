using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
using UnityEngine.EventSystems;
using NodeEditorFramework.Standard;

public class SelectedCraftingItem : MonoBehaviour, IDragHandler, IPointerDownHandler
{

    private Text selectedItemText;
    public GameObject draggingIcon;
    private CraftingInterface craftingInterface;
    private IngredientNode recipe;
    private NodeCanvas canvas;
    public Button myButton;
    public List<string> itemCount;



    // Use this for initialization

    void Start()
    {
        craftingInterface = GetComponentInParent<CraftingInterface>();
        Debug.Log(craftingInterface.saveName);
        selectedItemText = GameObject.Find("SelectedItemText").GetComponent<Text>();
        BasePlayer basePlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<BasePlayer>();
        myButton = GameObject.Find("CraftButton").GetComponent<Button>();
        canvas = NodeEditorSaveManager.LoadSceneNodeCanvas("CraftingCanvas", false);

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShowSelectedItemText()
    {

        if (this.gameObject.GetComponent<Toggle>().isOn)
        {
            if (this.gameObject.name == "Empty")
            {
                selectedItemText.text = "This slot is empty.";
            }
            else
            {
                selectedItemText.text = InventoryWindow.playerInventory[System.Int32.Parse(this.gameObject.name)].ItemName + ": " + InventoryWindow.playerInventory[System.Int32.Parse(this.gameObject.name)].ItemDescription;
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!GameObject.Find("InventoryWindow").GetComponent<InventoryWindow>().dragged && this.name != "Empty")
        {
            GameObject.Find("InventoryWindow").GetComponent<InventoryWindow>().ShowDraggedItem(this.transform.name);
            string v = InventoryWindow.playerInventory[System.Int32.Parse(this.gameObject.name)].ItemName;
            InventoryWindow.itemCounter.Remove(v);
            //craftingInventory.Remove(v);
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.name = "Empty";

        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        InventoryWindow inventoryWindow = GameObject.Find("InventoryWindow").GetComponent<InventoryWindow>();
        if (inventoryWindow.dragged)
        {
            if (this.name != "Empty")
            {
                inventoryWindow.SwapItem(this.gameObject);
            }
            else
            {
                this.transform.name = inventoryWindow.AddItemToSlot(this.gameObject);
                this.transform.GetChild(0).gameObject.SetActive(true);
                string v = InventoryWindow.playerInventory[System.Int32.Parse(this.gameObject.name)].ItemName;
                itemsAdded(v);
                InventoryWindow.itemCounter.Add(v);
            }

        }
    }

    public void Craft()
    {
        Node node = craftingInterface.Retrieve();
        recipe = node as IngredientNode;
        if (recipe != null)
        {
            myButton.interactable = false;
            removeItemsFromInventory();
            InventoryWindow.playerInventory.Add(new BaseItem((Instantiate(canvas.nodes.Find(x => ((IngredientNode)x).ingredientName == recipe.ingredientName)) as IngredientNode)));
            craftingInterface.ResetHistory();
            InventoryWindow.itemCounter.Clear();
        } else
        {
            myButton.interactable = false;
            removeItemsFromInventory();
            craftingInterface.ResetHistory();
            InventoryWindow.itemCounter.Clear();
            InventoryWindow.playerInventory.Add(new BaseItem());
        }
    }

    private void itemsAdded(string v)
    {
        craftingInterface.Register(v);
        if (craftingInterface.registeredElements.Count >= 2)
        {
            myButton.interactable = true;
        }
    }

    private void removeItemsFromInventory()
    {
        foreach (string s in InventoryWindow.itemCounter)
        {
            for (int i = 0; i < InventoryWindow.playerInventory.Count; i++)
            {
                BaseItem item = InventoryWindow.playerInventory[i];
                if (item.ItemName == s)
                {
                    InventoryWindow.playerInventory.Remove(item);
                    break;
                }
            }
        }
    }
}
