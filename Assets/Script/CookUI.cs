using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CookUI : MonoBehaviour
{

 [System.Serializable]
    public class CookRecipe
    {
        public Item result;
        public List<Item> ingredients;
        public List<int> ingredientsCount;
    }

    public List<CookRecipe> recipes;

    public DropCookSlot slotCook1;
    public DropCookSlot slotCook2;
    public GameObject inventorySlots;
    public bool isCookUIPanelOpen = false;

    List<Item> Items;
    [Header("Inventory Slot")]
    [SerializeField] Transform itemSlotContainer;
    [SerializeField] Transform itemSlotTemplate;

    


    private void Start(){
        //  SetRecipeDescription(recipes[0], CanCraft(recipes[0]));
        //  LogRecipesToConsole();

        
    }

    private void Update()
    {
        // Close CookUI
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCook();
        }
    }

    public void OpenCook()
    {
        GameController.Instance.ShowPersistentUI(false);
        gameObject.SetActive(true);
        isCookUIPanelOpen = true;

        RefreshSlots();
    }

    public void RefreshSlots()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate)
                continue;
            Destroy(child.gameObject);
        }

        foreach (Item item in Player_Inventory.Instance.itemList)
        {
            Transform theItem = Instantiate(itemSlotTemplate, itemSlotContainer);
            theItem.name = item.itemName;
            theItem.gameObject.SetActive(true);
            theItem.GetChild(0).GetComponent<Image>().sprite = item.sprite;
            theItem.GetChild(1).GetComponent<TMP_Text>().text = item.stackCount.ToString();

            theItem.GetComponent<DragCook>().itemName = item.itemName;

        }
    }

    public void CloseCook()
    {
        GameController.Instance.ShowPersistentUI(true);
        gameObject.SetActive(false);
        isCookUIPanelOpen = false;
    }

   
     void LogRecipesToConsole()
    {
        // mengambil total recipes 
        int recipeCount = recipes.Count;
        Debug.Log($"Total Recipes: {recipeCount}");

        foreach (var recipe in recipes)
        {
            // mengambil nama item yang di hasilkan 
            string recipeDetails = $"Recipe: {recipe.result.itemName}\nIngredients:";
            // mengecek jumlah item dari sebuah resep 
            for (int i = 0; i < recipe.ingredients.Count; i++)
            {
                // menapilkan ingredients dan ingredients Count
                recipeDetails += $"\n- {recipe.ingredients[i].itemName} x{recipe.ingredientsCount[i]}";
            }
            Debug.Log(recipeDetails);
        }
    }



}