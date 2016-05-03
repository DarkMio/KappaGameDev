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

    private int xPos;
    private int yPos;
    private GameObject itemSlot;
    private int itemSlotCount;
    private List<GameObject> inventorySlots;

    private List<BaseItem> playerInventory;

	// Use this for initialization
	void Start () {
        CreateInventorySlotsInWindow();
        AddItemsFromInventory();
    }
	
	// Update is called once per frame
	void Update () {
	
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
            }
        }
    }
}
