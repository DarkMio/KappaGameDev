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
    private List<BaseItem> playerInventory;
    public GameObject draggingIcon;
    private CraftingInterface craftingInterface;
    private IngredientNode recipe;
    private NodeCanvas canvas;
    public Button myButton;
    private InventoryWindow test;

    // Use this for initialization
    void Start()
    {
        craftingInterface = GetComponentInParent<CraftingInterface>();
        Debug.Log(craftingInterface.saveName);
        selectedItemText = GameObject.Find("SelectedItemText").GetComponent<Text>();
        BasePlayer basePlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<BasePlayer>();
        playerInventory = basePlayerScript.ReturnPlayerInventory();
        myButton = GameObject.Find("CraftButton").GetComponent<Button>();

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
                selectedItemText.text = playerInventory[System.Int32.Parse(this.gameObject.name)].ItemName + ": " + playerInventory[System.Int32.Parse(this.gameObject.name)].ItemDescription;
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!GameObject.Find("InventoryWindow").GetComponent<InventoryWindow>().dragged && this.name != "Empty")
        {
            GameObject.Find("InventoryWindow").GetComponent<InventoryWindow>().ShowDraggedItem(this.transform.name);
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
                itemsAdded(playerInventory[System.Int32.Parse(this.gameObject.name)].ItemName);
            }

        }
    }

    public void Craft()
    {
        Node node = craftingInterface.Retrieve();
        recipe = node as IngredientNode;
        if (recipe != null)
        {
            Debug.Log(recipe.ingredientName);
            myButton.interactable = false;
            playerInventory.Add(new BaseItem("Antidote"));
            //playerInventory.Add(new BaseItem((Instantiate(canvas.nodes.Find(x => ((IngredientNode)x).ingredientName == recipe.ingredientName)) as IngredientNode)));
        } else
        {
            Debug.Log("Hahaha here is your trash");
        }
    }

    private void itemsAdded(string v)
    {
        craftingInterface.Register(v);
        if (craftingInterface.registeredElements.Count >= 2)
        {
            myButton.interactable = true;
            myButton.onClick.AddListener(() => Craft());
        }
    }
}
