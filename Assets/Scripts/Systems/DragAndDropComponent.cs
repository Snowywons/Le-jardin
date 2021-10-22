using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropComponent : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    [SerializeField] string canvasName;

    private RectTransform canvasRectTransform;
    private ItemSlot inventorySlot;
    private RectTransform slotRectTransform;
    private CanvasGroup slotCanvasGroup;

    private bool isDraggable;

    private void Start()
    {
        slotRectTransform = GetComponent<RectTransform>();
        slotCanvasGroup = GetComponent<CanvasGroup>();
        inventorySlot = GetComponent<ItemSlot>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!FindCanvas()) return;

        // On pr�sume que l'item est draggable
        isDraggable = true;

        // Conserve une r�f�rence sur son ancien canvasContent avant de le d�placer
        inventorySlot.oldCanvasContent = transform.parent;

        // Si l'item ne contient aucun content (un slot vide)
        if (inventorySlot.slot.content == null)
        {
            isDraggable = false;
            return;
        }

        // On assigne � l'item un nouveau parent afin de pouvoir le d�placer
        gameObject.transform.SetParent(canvas.transform);
        slotCanvasGroup.alpha = 0.5f;
        slotCanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!FindCanvas() || !isDraggable) return;

        slotRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        slotCanvasGroup.alpha = 1f;
        slotCanvasGroup.blocksRaycasts = true;

        // V�rifie si l'item est en dehors de l'�cran
        if (slotRectTransform.anchoredPosition.x < 36 || slotRectTransform.anchoredPosition.x > canvasRectTransform.rect.width - 36 ||
            slotRectTransform.anchoredPosition.y > 36 || slotRectTransform.anchoredPosition.y < -canvasRectTransform.rect.height + 36)
        {
            gameObject.transform.SetParent(inventorySlot.oldCanvasContent);
        }
    }

    private bool FindCanvas()
    {
        GameObject obj = GameObject.Find(canvasName);
        if (obj == null) return false;

        canvas = obj.GetComponent<Canvas>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        return canvas != null;
    }
}
