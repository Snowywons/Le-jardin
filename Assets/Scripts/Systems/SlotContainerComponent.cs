using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class SlotContainerComponent : MonoBehaviour, IDropHandler
{
    [SerializeField] GameObject content;

    private const string INVENTORY_CANVAS_CONTENT_NAME = "Inventory Content";
    private const string WAREHOUSE_CANVAS_CONTENT_NAME = "Warehouse Content";

    InventoryComponent invFrom;
    InventoryComponent invTo;

    public void OnDrop(PointerEventData eventData)
    {
        // Vérifie qu'il y a un item
        GameObject item = eventData.pointerDrag;
        if (item == null) return;

        // Vérifie que l'item contient un ItemSlot
        ItemSlot itemSlot = item.GetComponent<ItemSlot>();
        if (itemSlot == null) return;

        // On commit le changement
        Commit(item);

        // On réassigne l'ancien canvasContent à l'item après son relâchement
        item.transform.SetParent(itemSlot.oldCanvasContent);
    }

    public bool Commit(GameObject droppedItem)
    {
        // Vérifie si l'item contient le composant 'ItemSlot'
        ItemSlot inventorySlot = droppedItem.GetComponent<ItemSlot>();
        if (inventorySlot == null) return false;

        // Vérifie si l'item contient un content (n'est pas vide)
        Item item = inventorySlot.slot.content;
        if (item == null) return false;

        // On assigne l'inventaire du destinateur et du destinataire selon l'ancien canvasContent de l'item
        if (inventorySlot.oldCanvasContent.name == INVENTORY_CANVAS_CONTENT_NAME)
        {
            invFrom = GameSystem.Instance.PlayerInventory;
            invTo = GameSystem.Instance.WarehouseInventory;
        }
        else
        {
            invFrom = GameSystem.Instance.WarehouseInventory;
            invTo = GameSystem.Instance.PlayerInventory;
        }

        // Si l'item déposé est déjà dans l'inventaire (un swap)
        if (content == null || inventorySlot.oldCanvasContent == content.transform)
        {
            // Si l'inventaire n'est pas plein, alors on prend prend le premier slot de libre et on y insert l'item
            if (!invFrom.IsFull())
                invFrom.CopyToNewSlot(inventorySlot.slot);
            
            invFrom.Sort();
            invFrom.UpdateUI();

            return false;
        }

        //Si un slot contient déjà l'item, alors on ajoute la quantité
        Slot slot = invTo.FindSlot(item.item);
        if (slot != null)
        {
            slot.content.quantity += item.quantity;
            slot.Update();
            invFrom.Remove(item.item, true);
            invFrom.Sort();
            invFrom.UpdateUI();
            return true;
        }
        else
        {
            //Si aucun slot contient l'item, alors on ajoute l'item dans un slot libre
            if (!invTo.IsFull())
            {
                invTo.Update(item.item, item.quantity);
                invFrom.Remove(item.item, true);
                invFrom.Sort();
                invFrom.UpdateUI();
                return true;
            }
            else
            {
                invFrom.UpdateUI();
                return false;
            }
        }
    }
}
