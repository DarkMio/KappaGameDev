using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasePlayer : MonoBehaviour {

    private List<BaseStat> _playerStats = new List<BaseStat>();

    private List<BaseItem> _inventory = new List<BaseItem>();


	void Start () {
        for (int i = 0; i  < 10; i++)
        {
            BaseItem _item = new BaseItem();
            _inventory.Add(_item);
            Debug.Log(_inventory[i].ItemName);
            Debug.Log(_inventory[i].ItemDescription);
            Debug.Log(_inventory[i].ItemType);
            Debug.Log(_inventory[i].ItemValue);
            Debug.Log(_inventory[i].ItemStats[0].StatName);
            Debug.Log(_inventory[i].ItemStats[0].StatDescription);
            Debug.Log(_inventory[i].ItemStats[0].StatType);
        }
        Debug.Log(_inventory.Count);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public List<BaseItem> ReturnPlayerInventory()
    {
        return _inventory;
    }
}
