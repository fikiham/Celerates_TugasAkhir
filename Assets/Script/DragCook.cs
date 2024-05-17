using UnityEngine;
using UnityEngine.EventSystems;

public class DragCook : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    [SerializeField] CookUI cookUI;
    [SerializeField] Transform inventoryContainer;
    public string itemName;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Periksa apakah canvasGroup sudah ada, jika belum tambahkan secara dinamis
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // Mengurangi opacity saat item di-drag
        canvasGroup.blocksRaycasts = false; // Menonaktifkan raycast agar tidak menghalangi input
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f; // Mengembalikan opacity ke nilai awal
        canvasGroup.blocksRaycasts = true; // Mengaktifkan kembali raycast
        // Mengembalikan item ke posisi awal jika tidak berhasil di-drop
        rectTransform.parent = cookUI.transform;
        rectTransform.parent = inventoryContainer;
        //if (rectTransform.parent == inventoryContainer)
        //    cookUI.RefreshSlots();
    }
}
