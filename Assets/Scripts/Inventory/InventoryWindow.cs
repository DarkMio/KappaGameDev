using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
// Fabian, 15.04.2016

public class InventoryWindow : MonoBehaviour{

    public int startingPosX;
    public int startingPosY;
    public int slotCountPerPage;
    public int slotCountLength;
    public GameObject itemSlotPrefab;
    public ToggleGroup itemSlotToggleGroup;

    public GameObject draggedIcon;
    public BaseItem draggedItem;
    public bool dragged = false;
    private const int mousePosOffset = 10;

    private string slotName;

    private int xPos;
    private int yPos;
    private GameObject itemSlot;
    private int itemSlotCount;
    public List<GameObject> inventorySlots;

    public static List<BaseItem> playerInventory;
    public static List<string> itemCounter = new List<string>();


    // Use this for initialization


    void Start () {
        BasePlayer basePlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<BasePlayer>();
        playerInventory = basePlayerScript.ReturnPlayerInventory();
        CreateInventorySlotsInWindow();
        AddItemsFromInventory();

    }



    // Update is called once per frame
    void Update () {
        if(dragged){ // Harry, are you a wizzard? How can it be so easy to add the mouse-position?
            draggedIcon.GetComponent<RectTransform>().position = Input.mousePosition + new Vector3(0, -mousePosOffset);
        }

    }

    public void DisrespectYourSurroundings()
    {
        Debug.Log("Inventory");
        foreach (GameObject item in inventorySlots)
        {
            Destroy(item);
        }
        CreateInventorySlotsInWindow();
        AddItemsFromInventory();

    }

    public void inventoryCount()
    {
        AddItemsFromInventory();
    }


    public void ShowDraggedItem(string name) {
        slotName = name;
        dragged = true;
        draggedIcon.SetActive(true);
        draggedItem = playerInventory[int.Parse(name)];
        Image img = draggedIcon.GetComponent<Image>();
        var itemImage = ReturnItemIcon(draggedItem);
        img.sprite = itemImage;
    }

    public string AddItemToSlot(GameObject slot){
        slot.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ReturnItemIcon(playerInventory[int.Parse(slotName)]);
        draggedIcon.SetActive(false);
        draggedItem = null;
        dragged = false;
        return slotName;
    }

    private void CreateInventorySlotsInWindow()
    {
        inventorySlots = new List<GameObject>();
        xPos = startingPosX;
        yPos = startingPosY;
        for (int i = 0; i < slotCountPerPage; i++)
        {
            itemSlot = Instantiate(itemSlotPrefab);
            itemSlot.name = "Empty";
            itemSlot.GetComponent<Toggle>().group = itemSlotToggleGroup;
            inventorySlots.Add(itemSlot);
            itemSlot.transform.SetParent(this.gameObject.transform);
            itemSlot.GetComponent<RectTransform>().localPosition = new Vector3(xPos, yPos, 0);
            xPos += (int)itemSlot.GetComponent<RectTransform>().rect.width;
            itemSlotCount++;
            if(itemSlotCount % slotCountLength == 0)
            {
                itemSlotCount = 0;
                yPos -= (int)itemSlot.GetComponent<RectTransform>().rect.height;
                xPos = startingPosX;
            }
        }
    }

    public void AddItemsFromInventory()
    {
            for (int i = 0; i < playerInventory.Count; i++)
            {
                if (inventorySlots[i].name == "Empty" && !(inventorySlots[i].name == playerInventory[i].ItemName))
                {
                    inventorySlots[i].name = i.ToString();
                    //change empty slot with actual item
                    inventorySlots[i].transform.GetChild(0).gameObject.SetActive(true);
                    inventorySlots[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ReturnItemIcon(playerInventory[i]);
                }
            }
    }


    public Sprite ReturnItemIcon(BaseItem item){
        Sprite icon = new Sprite();

        if (item.ItemName == "Green Herb")
        {
            icon = Resources.Load<Sprite>("herb_green");
        }
        if (item.ItemName == "Blue Herb")
        {
            icon = Resources.Load<Sprite>("herb_blue");
        }
        if(item.ItemName == "Antidote")
        {
            icon = Resources.Load<Sprite>("flask_violet_full");
        }
        if(item.ItemName == "Trash")
        {
            icon = Resources.Load<Sprite>("trash");
        }

        return icon;
    }

    public void SwapItem(GameObject slot){
        BaseItem swapItem = playerInventory[int.Parse(slot.name)];
        slot.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ReturnItemIcon(draggedItem);
        slot.name = slotName;
        draggedItem = swapItem;
        draggedIcon.GetComponent<Image>().sprite = ReturnItemIcon(draggedItem);
        slotName = playerInventory.FindIndex(x => x == draggedItem).ToString();
        }

}
