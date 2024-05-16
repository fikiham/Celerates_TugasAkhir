using UnityEngine;
using UnityEngine.EventSystems;

public class CookInventory : MonoBehaviour, IDropHandler
{
    public DropCookSlot slotCook1; // Slot Cook1
    public DropCookSlot slotCook2; // Slot Cook2

    public void OnDrop(PointerEventData eventData)
    {
        DragCook dragCook = eventData.pointerDrag.GetComponent<DragCook>();
        if (dragCook != null)
        {
            // Mendapatkan RectTransform dari slot tempat item di-drop
            RectTransform slotRectTransform = GetComponent<RectTransform>();
            // Mendapatkan RectTransform dari item yang di-drop
            RectTransform itemRectTransform = dragCook.GetComponent<RectTransform>();

            // Memindahkan posisi item ke posisi slot
            itemRectTransform.SetParent(transform); // Mengatur parent menjadi slot tempat item di-drop
            itemRectTransform.anchoredPosition = Vector3.zero; // Meletakkan item di tengah-tengah slot

            // Mengosongkan SlotCook1 atau SlotCook2 jika item di-drop ke inventory
            if (slotCook1 != null && slotCook1.item == dragCook.gameObject)
            {
                slotCook1.item = null;
            }

            if (slotCook2 != null && slotCook2.item == dragCook.gameObject)
            {
                slotCook2.item = null;
            }
        }
    }
}
