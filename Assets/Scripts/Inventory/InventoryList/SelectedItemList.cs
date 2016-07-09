using UnityEngine;
using UnityEngine.UI;

public class SelectedItemList : MonoBehaviour
{

    private Text selectedItemListText;

    // Use this for initialization
    void Start()
    {
        selectedItemListText = GameObject.Find("SelectedItemListText").GetComponent<Text>();
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
                selectedItemListText.text = InventoryWindow.playerInventory[System.Int32.Parse(this.gameObject.name)].ItemName + ": " + InventoryWindow.playerInventory[System.Int32.Parse(this.gameObject.name)].ItemDescription;
            }
        }
    }
}
