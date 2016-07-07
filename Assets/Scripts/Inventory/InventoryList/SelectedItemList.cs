using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SelectedItemList : MonoBehaviour
{

    private Text selectedItemListText;
    private List<BaseItem> playerInventory;

    // Use this for initialization
    void Start()
    {
        selectedItemListText = GameObject.Find("SelectedItemListText").GetComponent<Text>();
        BasePlayer basePlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<BasePlayer>();
        // playerInventory = basePlayerScript.ReturnPlayerInventory();
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
                selectedItemListText.text = "This slot is empty.";
            }
            else
            {
                selectedItemListText.text = playerInventory[System.Int32.Parse(this.gameObject.name)].ItemName + ": " + playerInventory[System.Int32.Parse(this.gameObject.name)].ItemDescription;
            }
        }
    }
}
