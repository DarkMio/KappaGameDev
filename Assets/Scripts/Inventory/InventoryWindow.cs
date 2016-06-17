using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
// Fabian, 15.04.2016

public class InventoryWindow : MonoBehaviour {

    public int startingPosX;
    public int startingPosY;
    public int slotCountPerPage;
    public int slotCountLength;
    public GameObject itemSlotPrefab;
    public ToggleGroup itemSlotToggleGroup;
    public Sprite [] itemSprites;

    public GameObject draggedIcon;
    public BaseItem draggedItem;
    public bool dragged = false;
    private const int mousePosOffset = 20;
    
    private string slotName;

    private int xPos;
    private int yPos;
    private GameObject itemSlot;
    private int itemSlotCount;
    private List<GameObject> inventorySlots;

    private List<BaseItem> playerInventory;

	// Use this for initialization
	void Start () {
        itemSprites =  Resources.LoadAll<Sprite>("FinalFantasy6Sheet4");
        CreateInventorySlotsInWindow();
        AddItemsFromInventory();
    }
    
    
	
	// Update is called once per frame
	void Update () {
        // TODO Fix mouse movement
        if(dragged){
            Vector3 mousePosition = (Input.mousePosition - GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>().localPosition);
            draggedIcon.GetComponent<RectTransform>().localPosition = new Vector3(mousePosition.x + mousePosOffset, mousePosition.y - mousePosOffset, mousePosition.z);
        }
	
	}
    
    
    public void ShowDraggedItem(string name){
        slotName = name;
        dragged = true;
        draggedIcon.SetActive(true);
        draggedItem = playerInventory[int.Parse(name)];
        draggedIcon.GetComponent<Image>().sprite = ReturnItemIcon(draggedItem);
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
            itemSlot = (GameObject)Instantiate(itemSlotPrefab);
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

    private void AddItemsFromInventory()
    {
        BasePlayer basePlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<BasePlayer>();
        playerInventory = basePlayerScript.ReturnPlayerInventory();
        for (int i = 0; i < playerInventory.Count; i++)
        {
            if (inventorySlots[i].name == "Empty")
            {
                inventorySlots[i].name = i.ToString();
                //change empty slot with actual item
                inventorySlots[i].transform.GetChild(0).gameObject.SetActive(true);
                inventorySlots[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ReturnItemIcon(playerInventory[i]);
            }
        }
    }
    
    private Sprite ReturnItemIcon(BaseItem item){
        Sprite icon = new Sprite();
/*      
        if(item.ItemType == BaseItem.ItemTypes.Ingredient){
            icon = itemSprites[70];
        } else if (item.ItemType == BaseItem.ItemTypes.Potion) {
             icon = itemSprites[10];
        } else if (item.ItemType == BaseItem.ItemTypes.Junk){
            icon = itemSprites[40];
        }
*/
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
