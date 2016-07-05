using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using NodeEditorFramework;
using NodeEditorFramework.Standard;

// Fabian, 15.04.2016

public class CraftingWindow : MonoBehaviour
{

    public int startingPosX;
    public int startingPosY;
    public int slotCountPerPage;
    public int slotCountLength;
    public GameObject itemSlotPrefab;
    public ToggleGroup itemSlotToggleGroup;
    public Sprite[] itemSprites;

    public GameObject draggedIcon;
    public BaseItem draggedItem;
    public bool dragged = false;
    private const int mousePosOffset = 10;

    private string slotName;

    private int xPos;
    private int yPos;
    private GameObject itemSlot;
    private int itemSlotCount;
    private List<GameObject> inventorySlots;
    private List<BaseItem> craftingInventory;
    public Button myButton;




    // Use this for initialization
    void Start()
    {
        myButton = GameObject.Find("CraftButton").GetComponent<Button>();
        itemSprites = Resources.LoadAll<Sprite>("FinalFantasy6Sheet4");
        CreateInventorySlotsInWindow();
    }



    // Update is called once per frame
    void Update()
    {
        if (dragged)
        { // Harry, are you a wizzard? How can it be so easy to add the mouse-position?
            draggedIcon.GetComponent<RectTransform>().position = Input.mousePosition + new Vector3(0, -mousePosOffset);
        }
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
            xPos += (int)itemSlot.GetComponent<RectTransform>().rect.width + 10;
            itemSlotCount++;
            if (itemSlotCount % slotCountLength == 0)
            {
                itemSlotCount = 0;
                yPos -= (int)itemSlot.GetComponent<RectTransform>().rect.height;
                xPos = startingPosX;
            }
        }
    }

    private void AddItemsFromInventory()
    {
        BasePlayer basePlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<BasePlayer>();
        craftingInventory = basePlayerScript.ReturnPlayerInventory();
        for (int i = 0; i < craftingInventory.Count; i++)
        {
            if (inventorySlots[i].name == "Empty")
            {
                inventorySlots[i].name = i.ToString();
                //change empty slot with actual item
                inventorySlots[i].transform.GetChild(0).gameObject.SetActive(true);
                inventorySlots[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ReturnItemIcon(craftingInventory[i]);
            }
        }
    }

    public string AddItemToSlot(GameObject slot)
    {
        slot.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ReturnItemIcon(craftingInventory[int.Parse(slotName)]);
        draggedIcon.SetActive(false);
        draggedItem = null;
        dragged = false;
        return slotName;
    }

    private Sprite ReturnItemIcon(BaseItem item)
    {
        Sprite icon = new Sprite();

        if (item.ItemName == "Green Herb")
        {
            icon = itemSprites[50];
        }
        if (item.ItemName == "Blue Herb")
        {
            icon = itemSprites[60];
        }
        return icon;
    }

    public void SwapItem(GameObject slot)
    {
        BaseItem swapItem = craftingInventory[int.Parse(slot.name)];
        slot.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ReturnItemIcon(draggedItem);
        slot.name = slotName;
        draggedItem = swapItem;
        draggedIcon.GetComponent<Image>().sprite = ReturnItemIcon(draggedItem);
        slotName = craftingInventory.FindIndex(x => x == draggedItem).ToString();
    }

    public void ShowDraggedItem(string name)
    {
        slotName = name;
        dragged = true;
        draggedIcon.SetActive(true);
        draggedItem = craftingInventory[int.Parse(name)];
        Image img = draggedIcon.GetComponent<Image>();
        var itemImage = ReturnItemIcon(draggedItem);
        img.sprite = itemImage;
    }
}
