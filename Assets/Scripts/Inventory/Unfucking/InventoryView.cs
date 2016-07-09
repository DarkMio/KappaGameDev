using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour {

    public Sprite[] itemSprites;

    public List<GameObject> inventorySlots;
    public GameObject itemSlotPrefab;
    public ToggleGroup itemSlotToggleGroup;
    private BasePlayer playerEntity;
    private BaseItem[] playerInventory;
    private BaseItem[] cachedInventory;

    public int startingPosX;
    public int startingPosY;
    public int slotCountPerPage;
    public int slotCountLength;
    private int itemSlotCount;

    private SelectedItemView mouseFollowing;
    private Vector3 mouseFollowingOriginalPosition; // original transform.
    private bool shouldMove;
    private bool acquiredLock = false;

    void Start () {
        itemSprites = Resources.LoadAll<Sprite>("FinalFantasy6Sheet4");

        playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<BasePlayer>();
        playerInventory = playerEntity._inventory;
        cachedInventory = new BaseItem[playerInventory.Length];
        BuildInventorySlots();
    }

    void BuildInventorySlots() {
        playerInventory.CopyTo(cachedInventory, 0);
        foreach (GameObject item in inventorySlots) {
            Destroy(item);
        }
        inventorySlots = new List<GameObject>();
        var xPos = startingPosX;
        var yPos = startingPosY;
        for (int i = 0; i < slotCountPerPage; i++)
        {
            var itemSlot = Instantiate(itemSlotPrefab);
            var itemScript = itemSlot.GetComponent<SelectedItemView>();
            itemScript.spritesheet = itemSprites; // defer the spritesheet downwards.
            itemScript.inventoryView = this;
            if (i < playerInventory.Length) {
                // Attach the game object to it.
                itemScript.assignedObject = playerInventory[i];
                Debug.Log("Assigned! " + playerInventory[i] + " | " + itemScript.assignedObject);

            }
            itemSlot.name += " " + i;
            itemSlot.transform.SetParent(this.gameObject.transform);
            itemSlot.GetComponent<RectTransform>().localPosition = new Vector3(xPos, yPos, 0);
            xPos += (int)itemSlot.GetComponent<RectTransform>().rect.width;

            // Add it to the lsit, now we can work easily through them.
            inventorySlots.Add(itemSlot);

            if(inventorySlots.Count % slotCountLength == 0)
            {
                itemSlotCount = 0;
                yPos -= (int)itemSlot.GetComponent<RectTransform>().rect.height;
                xPos = startingPosX;
            }
        }
    }

    public void Clicked(SelectedItemView clickedObject) {
        if (mouseFollowing == null) { // enabling swapping
            Debug.Log("REAQUIRING!");
            mouseFollowing = clickedObject;
            mouseFollowingOriginalPosition = clickedObject.transform.position;
            acquiredLock = false;
        } else { // disabling swapping
            mouseFollowing.transform.position = clickedObject.transform.position;
            clickedObject.transform.position = mouseFollowingOriginalPosition;

            // and now swap them in list.
            int indexClicked = inventorySlots.FindIndex(a => a.GetComponent<SelectedItemView>() == clickedObject);
            int indexFollowing = inventorySlots.FindIndex(a => a.GetComponent<SelectedItemView>() == mouseFollowing);
            playerInventory[indexClicked] = mouseFollowing.assignedObject;
            playerInventory[indexFollowing] = clickedObject.assignedObject;

            cachedInventory[indexClicked] = mouseFollowing.assignedObject;
            cachedInventory[indexFollowing] = clickedObject.assignedObject;

            mouseFollowing = null;
        }

    }

	void OnGUI () {
	    if (GUI.Button(new Rect(5, 100, 65, 20), "Rebuild UI.")) {
	        BuildInventorySlots();
	    }

	    if (!playerInventory.SequenceEqual(cachedInventory)) {
	        BuildInventorySlots();
	    }


	    if (mouseFollowing != null) {
	        Vector3 position = Input.mousePosition + new Vector3(0, -35, 0);
	        if (!acquiredLock) { // this can be cleaned up and refactored:
	            mouseFollowing.transform.position = Vector3.MoveTowards(mouseFollowing.transform.position,
	                position, 5f);
	            float magnitude = (mouseFollowing.transform.position - position).magnitude;
	            if (magnitude < 0.5f) {
	                acquiredLock = true;
	            }
	        } else {
	            mouseFollowing.transform.position = position;
	        }
	    }
	}
}
