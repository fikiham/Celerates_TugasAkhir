using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropCookSlot : MonoBehaviour, IDropHandler
{
     

    public GameObject item; // Item yang saat ini ada di slot
    public string slotName; // Nama slot, misalnya "SlotCook1" atau "SlotCook2"

    public GameObject hasilCook; // Referensi ke GameObject HasilCook

    [SerializeField] CookUI cookUI;

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
                RemoveFromPreviousSlot(draggedItem);
                SetItemToSlot(draggedItem);
                CreateCook();
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
                // Misalnya, atur ukuran menjadi (100, 100)
                resultRectTransform.sizeDelta = new Vector2(100, 100);

                // Atur posisi menjadi (0, 0) relatif terhadap parent (HasilCook)
                resultRectTransform.anchoredPosition = Vector3.zero;

                // Atur rotasi menjadi default (0, 0, 0)
                resultRectTransform.rotation = Quaternion.identity;

                // Tambahkan listener untuk tombol hasilCook
                hasilCookButton.onClick.RemoveAllListeners();
                hasilCookButton.onClick.AddListener(() =>
                {
                    // Tambahkan item ke inventori pemain
                    Player_Inventory.Instance.AddItem(ItemPool.Instance.GetItem(recipe.result.itemName));

                    // Hapus GameObject hasil masakan dari tampilan
                    Destroy(resultItem);

                    // Hapus item dari slot masak
                    RemoveItemsFromSlots();
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