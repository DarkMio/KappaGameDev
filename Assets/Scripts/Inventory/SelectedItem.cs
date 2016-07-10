﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SelectedItem : MonoBehaviour, IDragHandler, IPointerDownHandler {

    private Text selectedItemText;
    private Text selectedItemValue;

    public GameObject draggingIcon;

	// Use this for initialization
	void Start () {
        selectedItemText = GameObject.Find("SelectedItemText").GetComponent<Text>();
        selectedItemValue = GameObject.Find("SelectedItemValue").GetComponent<Text>();

    }

    // Update is called once per frame
    void Update () {

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
                selectedItemText.text = InventoryWindow.playerInventory[int.Parse(this.gameObject.name)].ItemAffix.ToString() + " " + InventoryWindow.playerInventory[int.Parse(this.gameObject.name)].ItemName + ": " + InventoryWindow.playerInventory[int.Parse(this.gameObject.name)].ItemDescription;
            }
        }
    }

    public void ShowSelectedItemValue()
    {
        if (this.gameObject.GetComponent<Toggle>().isOn)
        {
            if (this.gameObject.name == "Empty")
            {
                selectedItemValue.text = null;
            }
            else
            {
                selectedItemValue.text = "Value: " + InventoryWindow.playerInventory[int.Parse(this.gameObject.name)].ItemValue;
            }
        }
    }

    public void OnDrag(PointerEventData eventData){
        if (!GameObject.Find("InventoryWindow").GetComponent<InventoryWindow>().dragged && this.name !="Empty"){
            GameObject.Find("InventoryWindow").GetComponent<InventoryWindow>().ShowDraggedItem(this.transform.name);
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.name = "Empty";
        }
    }


    public void OnPointerDown(PointerEventData eventData){
        InventoryWindow inventoryWindow = GameObject.Find("InventoryWindow").GetComponent<InventoryWindow>();
        if (inventoryWindow.dragged){
            if(this.name != "Empty"){

                inventoryWindow.SwapItem(this.gameObject);
            } else {
            this.transform.name = inventoryWindow.AddItemToSlot(this.gameObject);
            this.transform.GetChild(0).gameObject.SetActive(true);
            }

        }
    }
}
