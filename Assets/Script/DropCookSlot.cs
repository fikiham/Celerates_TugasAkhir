using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropCookSlot : MonoBehaviour, IDropHandler
{
     

    public GameObject item; // Item yang saat ini ada di slot
    public string slotName; // Nama slot, misalnya "SlotCook1" atau "SlotCook2"

    public GameObject hasilCook; // Referensi ke GameObject HasilCook

    [SerializeField] CookUI cookUI;
    [SerializeField] CookIngredients cookIngredients;

    private Button hasilCookButton;

    private void Start()
    {
        // Menetapkan inventorySlots dari CookUI jika belum dilakukan
        if (cookUI == null || cookUI.inventorySlots == null)
        {
            Debug.LogError("CookUI or inventorySlots in CookUI is not assigned!");
        }

        // Mendapatkan komponen Button dari hasilCook
        hasilCookButton = hasilCook.GetComponent<Button>();
        if (hasilCookButton == null)
        {
            Debug.LogError("Button component on hasilCook is not assigned!");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedItem = eventData.pointerDrag;
        DragCook dragCook = draggedItem.GetComponent<DragCook>();

        if (dragCook != null)
        {
            if (!item)
            {
                cookIngredients.DestroyRecipeInHasilCook();
                RemoveFromPreviousSlot(draggedItem);
                SetItemToSlot(draggedItem);
                CreateCook();
                Debug.Log("Item di pindahkan");

            }
            else
            {
                dragCook.ResetPosition();
            }
        }
    }

    private void RemoveFromPreviousSlot(GameObject draggedItem)
    {
        DropCookSlot[] slots = transform.parent.GetComponentsInChildren<DropCookSlot>();
        foreach (DropCookSlot slot in slots)
        {
            if (slot.item == draggedItem)
            {
                slot.item = null;
            }
        }
    }

    private void SetItemToSlot(GameObject draggedItem)
    {
        RectTransform itemRectTransform = draggedItem.GetComponent<RectTransform>();
        itemRectTransform.SetParent(transform);
        itemRectTransform.anchoredPosition = Vector3.zero;
        item = draggedItem;
    }

   


  private void CreateCook()
    {
        bool recipeFound = false;

        if (cookUI.slotCook1.item != null && cookUI.slotCook2.item != null)
        {
            Debug.Log(cookUI.slotCook1.item);
            DragCook item1 = cookUI.slotCook1.item.GetComponent<DragCook>();
            DragCook item2 = cookUI.slotCook2.item.GetComponent<DragCook>();

            foreach (var recipe in cookUI.recipes)
            {
                if ((item1.itemName == recipe.ingredients[0].itemName && item2.itemName == recipe.ingredients[1].itemName) ||
                    (item2.itemName == recipe.ingredients[0].itemName && item1.itemName == recipe.ingredients[1].itemName))
                {
                    recipeFound = true;

                    // Implement logic to create the dish here
                    string resultDetails = $"Result: {recipe.result.itemName}";
                    Debug.Log("memunculkan " + resultDetails);

                    // Buat GameObject baru untuk hasil masakan
                    GameObject resultItem = new GameObject(recipe.result.itemName);

                    // Tambahkan komponen Image dan atur sprite-nya
                    Image imageComponent = resultItem.AddComponent<Image>();
                    imageComponent.sprite = recipe.result.sprite;

                    // Tetapkan HasilCook sebagai parent GameObject baru
                    resultItem.transform.SetParent(hasilCook.transform, false); // false untuk menjaga posisi dan rotasi

                    // Setelah ditetapkan sebagai anak, akses RectTransform dari GameObject baru
                    RectTransform resultRectTransform = resultItem.GetComponent<RectTransform>();

                    // Sesuaikan ukuran, posisi, dan rotasi sesuai kebutuhan
                    resultRectTransform.sizeDelta = new Vector2(100, 100);
                    resultRectTransform.anchoredPosition = Vector3.zero;
                    resultRectTransform.rotation = Quaternion.identity;

                    // Tambahkan listener untuk tombol hasilCook
                    hasilCookButton.onClick.RemoveAllListeners();
                    hasilCookButton.onClick.AddListener(() =>
                    {
                        // Pastikan item yang dihasilkan tidak null sebelum menambahkannya ke inventori
                        Item resultItem = ItemPool.Instance.GetItem(recipe.result.itemName);
                        if (resultItem != null)
                        {
                            Player_Inventory.Instance.AddItem(resultItem);
                            // Player_Inventory.Instance.AddItem(ItemPool.Instance.GetItem("Pedang Ren"));

                            // Hapus GameObject hasil masakan dari tampilan
                            Destroy(resultItem);

                            // Hapus item dari slot masak
                            RemoveItemsFromSlots();
                        }
                        else
                        {
                            Debug.LogError("Item hasil masakan tidak ditemukan di ItemPool!");
                        }
                    });

                    // Jika resep ditemukan, keluar dari loop
                    break;
                }
            }
        }

        // Jika tidak ada resep yang cocok, panggil cancelCook
        if (!recipeFound)
        {
            cancelCook();
        }
    }




    

    public void cancelCook()
    {
        if (hasilCook.transform.childCount > 0)
        {
            Destroy(hasilCook.transform.GetChild(0).gameObject);
        }
        hasilCookButton.onClick.RemoveAllListeners();
    }

    private void RemoveItemsFromSlots()
    {
        if (cookUI.slotCook1.item != null)
        {
            Player_Inventory.Instance.RemoveItem(ItemPool.Instance.GetItem(cookUI.slotCook1.item.GetComponent<DragCook>().itemName));
            Destroy(cookUI.slotCook1.item);
            cookUI.slotCook1.item = null;
        }

        if (cookUI.slotCook2.item != null)
        {
            Player_Inventory.Instance.RemoveItem(ItemPool.Instance.GetItem(cookUI.slotCook2.item.GetComponent<DragCook>().itemName));
            Destroy(cookUI.slotCook2.item);
            cookUI.slotCook2.item = null;
        }

        cookUI.RefreshSlots();
       CreateCook();
    }

    public void ReturnToInventory()
    {
        if (item != null)
        {
            item.transform.SetParent(cookUI.inventorySlots.transform);
            item.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            item = null;

            CreateCook();
        }
    }
}