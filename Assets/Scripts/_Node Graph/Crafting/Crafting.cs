using UnityEngine;
using System.Collections;
using NodeEditorFramework.Standard;
using NodeEditorFramework;

public class Crafting : MonoBehaviour
{
    // Use this for initialization
    void OnGUI()
    {
        if (GUI.Button(new Rect(5, 85, 60, 20), "TEST"))
        {
            CraftingInterface c = GetComponent<CraftingInterface>();
            Debug.Log(c);
            c.Register("Green Herb");
            c.Register("Blue Herb");
            Node node = c.Retrieve();
            IngredientNode recipe = node as IngredientNode;
            if (recipe != null)
            {
                Debug.Log(recipe.ingredientName);
            }
        }
    }
}
