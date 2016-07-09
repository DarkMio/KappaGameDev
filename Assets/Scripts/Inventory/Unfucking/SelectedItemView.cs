using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectedItemView : MonoBehaviour {

    public Text selectedItemText;
    public Image draggingIcon;
    public BaseItem assignedObject; // this object should get rid of inventoryLinkNumber.
    public Sprite[] spritesheet;
    public InventoryView inventoryView;

	// Use this for initialization
	void Start () {
	    selectedItemText = GameObject.Find("SelectedItemText").GetComponent<Text>();
	    draggingIcon = this.transform.GetChild(0).gameObject.GetComponent<Image>();
	    GetComponent<Button>().onClick.AddListener(() => inventoryView.Clicked(this));
	}

    private void ButtonAction() {
        new UnityEvent();
        Debug.Log("Hello my lady, hello my darling.");
    }

    public void Receive(SelectedItemView item) {
        draggingIcon = item.draggingIcon;

    }

	// Update is called once per frame
	void Update () {
	    if (assignedObject != null) {
	        draggingIcon.sprite = ReturnItemIcon();
	        draggingIcon.color = new Color(1f, 1f, 1f, 1f);
	    }
	}

    public void ShowSelectedItemText() {
        if (assignedObject == null) {
            selectedItemText.text = "This slot is empty.";
        } else {
            selectedItemText.text = assignedObject.ItemName + " : " + assignedObject.ItemDescription;
        }
    }

    void OnClicked() {

    }

    public Sprite ReturnItemIcon(){
        if (spritesheet == null) {
            return null;
        }

        Sprite icon = new Sprite();
        if (assignedObject.ItemName == "Green Herb") {
            icon = spritesheet[50];
        }
        if (assignedObject.ItemName == "Blue Herb") {
            icon = spritesheet[60];
        }
        if(assignedObject.ItemName == "Antidote") {
            icon = spritesheet[40];
        }

        return icon;
    }
}
