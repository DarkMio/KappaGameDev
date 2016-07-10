using UnityEngine;
using System.Collections.Generic;
using NodeEditorFramework;
using NodeEditorFramework.Standard;

public class BaseItem {
    
    private string _name;
    private string _description;
    private int _value;
    private List<BaseStat> _stats;
    private ItemAffixNew _affix;
    

    public BaseItem(IngredientNode iNode)
    {
        ItemName = iNode.ingredientName;
        ItemDescription = iNode.description;
        ItemValue = iNode.price;
    }

    public BaseItem()
    {
        ItemName = "Trash";
        ItemDescription = "One man's trash";
        ItemValue = 1; 
    }

    
    public enum ItemAffixNew{
        normal,
        weak,
        puny,
        strong,
        mighty,
        godlike,
        perfect
    }
    
    public string ItemName{
        get{
            return _name;
        }
        set{
            _name = value;
        }
    }
    public string ItemDescription{
        get{
            return _description;
        }
        set{
            _description = value;
        }
    }
    public int ItemValue{
        get{
            return _value;
        }
        set{
            _value = value;
        }
    }
    public List<BaseStat> ItemStats{
        get{
            return _stats;
        }
        set{
            _stats = value;
        }
    }
    public ItemAffixNew ItemAffix{
        get{
            return _affix;
        }
        set{
            _affix = value;
        }
    }

}
