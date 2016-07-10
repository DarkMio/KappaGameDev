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
        myButton = GameObject.Find("CraftButton").GetComponent<Button>();
        canvas = NodeEditorSaveManager.LoadSceneNodeCanvas("CraftingCanvas", false);

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (!GameObject.Find("InventoryWindow").GetComponent<InventoryWindow>().dragged && this.name != "Empty")
        {
            GameObject.Find("InventoryWindow").GetComponent<InventoryWindow>().ShowDraggedItem(this.transform.name);
            string v = InventoryWindow.playerInventory[System.Int32.Parse(this.gameObject.name)].ItemName;
            InventoryWindow.itemCounter.Remove(v);
            craftingInterface.Remove(v);
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.name = "Empty";
            if (craftingInterface.registeredElements.Count < 2)
            {
                myButton.interactable = false;
            }

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
            BaseItem product = new BaseItem((Instantiate(canvas.nodes.Find(x => ((IngredientNode)x).ingredientName == recipe.ingredientName)) as IngredientNode));
            generateItemAffix(product);
            InventoryWindow.playerInventory.Add(product);
            craftingInterface.ResetHistory();
            InventoryWindow.itemCounter.Clear();
        }
        else
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

    private void generateItemAffix(BaseItem item)
    {
        int i = Random.Range(1, 10);
        switch (i)
        {
            case 1:
                item.ItemAffix = BaseItem.ItemAffixNew.puny;
                item.ItemValue = item.ItemValue * 4;
                break;

            case 2:
            case 3:
                item.ItemAffix = BaseItem.ItemAffixNew.weak;
                item.ItemValue = item.ItemValue * 5;
                break;

            case 4:
            case 5:
            case 6:
                item.ItemAffix = BaseItem.ItemAffixNew.normal;
                item.ItemValue = item.ItemValue * 6;
                break;

            case 7:
                item.ItemAffix = BaseItem.ItemAffixNew.strong;
                item.ItemValue = item.ItemValue * 7;
                break;

            case 8:
                item.ItemAffix = BaseItem.ItemAffixNew.mighty;
                item.ItemValue = item.ItemValue * 8;
                break;

            case 9:
                item.ItemAffix = BaseItem.ItemAffixNew.godlike;
                item.ItemValue = item.ItemValue * 9;
                break;

            case 10:
                item.ItemAffix = BaseItem.ItemAffixNew.perfect;
                item.ItemValue = item.ItemValue * 10;
                break;
        }
        }
}
