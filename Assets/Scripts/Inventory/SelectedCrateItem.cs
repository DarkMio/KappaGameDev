﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SelectedCrateItem : MonoBehaviour, IDragHandler, IPointerDownHandler {

    private Text selectedItemText;
    public GameObject draggingIcon;

	// Use this for initialization
	void Start () {
        selectedItemText = GameObject.Find("SelectedCrateItemText").GetComponent<Text>();
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
                selectedItemText.text = CrateWindow.crateInventory[System.Int32.Parse(this.gameObject.name)].ItemName + ": " + CrateWindow.crateInventory[System.Int32.Parse(this.gameObject.name)].ItemDescription;
            }
        }
    }

    public void OnDrag(PointerEventData eventData){
        if (!GameObject.Find("CrateWindow").GetComponent<CrateWindow>().dragged && this.name !="Empty"){
            GameObject.Find("CrateWindow").GetComponent<CrateWindow>().ShowDraggedItem(this.transform.name);
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.name = "Empty";
        }
    }


    public void OnPointerDown(PointerEventData eventData){
        CrateWindow crateWindow = GameObject.Find("CrateWindow").GetComponent<CrateWindow>();
        if (crateWindow.dragged){
            if(this.name != "Empty"){

                crateWindow.SwapItem(this.gameObject);
            } else {
            this.transform.name = crateWindow.AddItemToSlot(this.gameObject);
            this.transform.GetChild(0).gameObject.SetActive(true);
            }

        }
    }
}
