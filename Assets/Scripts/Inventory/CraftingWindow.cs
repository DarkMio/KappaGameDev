using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CraftingWindow : MonoBehaviour 
{
    public int startingPosXC;
    public int startingPosYC;
    public int slotCountPerPage2;
    public int slotCountLength2;
    public GameObject itemSlotPrefab;
    public ToggleGroup itemSlotToggleGroup;
    public GameObject draggedIcon;
    public BaseItem draggedItem;
    public bool dragged = false;
    private const int mousePosOffset = 10;
    private string slotName;
    private int xPos;
    private int yPos;
    private GameObject itemSlot2;
    private int itemSlotCount2;
    public List<GameObject> inventorySlots2;
    public List<GameObject> inventorySlotsTest;

    public Button myButton;
    public Button myButton2;


    // Use this for initialization
    void Start () {
        CreateCraftingSlotsInWindow();
	
	}
	
	// Update is called once per frame 
	void Update () {
	
	}

    public void DisrespectYourSurroundings2()
    {

        Debug.Log("Crafting");
        foreach (GameObject item in inventorySlots2)
        {
            Destroy(item);
        }
        CreateCraftingSlotsInWindow();
    }
    
    public void DisrespectYourSurroundings() {
        Debug.Log("Metal tune starts here.");
        foreach (GameObject item in inventorySlots)
        {
            Destroy(item);
        }
        CreateInventorySlotsInWindow();
    }



    private void CreateCraftingSlotsInWindow()
    {
        inventorySlots2 = new List<GameObject>();
        xPos = startingPosXC;
        yPos = startingPosYC;
        for (int i = 0; i < slotCountPerPage2; i++)
        {
            itemSlot2 = Instantiate(itemSlotPrefab);
            itemSlot2.name = "Empty";
            itemSlot2.GetComponent<Toggle>().group = itemSlotToggleGroup;
            inventorySlots2.Add(itemSlot2);
            itemSlot2.transform.SetParent(this.gameObject.transform);
            itemSlot2.GetComponent<RectTransform>().localPosition = new Vector3(xPos, yPos, 0);
            xPos += (int)itemSlot2.GetComponent<RectTransform>().rect.width;
            itemSlotCount2++;
            if (itemSlotCount2 % slotCountLength2 == 0)
            {
                itemSlotCount2 = 0;
                yPos -= (int)itemSlot2.GetComponent<RectTransform>().rect.height;
                xPos = startingPosXC;
            }
        }
    }

    public Sprite ReturnItemIcon(BaseItem item)
    {
        Sprite icon = new Sprite();

        if (item.ItemName == "Green Herb")
        {
            icon = Resources.Load<Sprite>("herb_green");
        }
        if (item.ItemName == "Blue Herb")
        {
            icon = Resources.Load<Sprite>("herb_blue");
        }
        if (item.ItemName == "Antidote")
        {
            icon = Resources.Load<Sprite>("flask_violet_full");
        }

        return icon;
    }



}
