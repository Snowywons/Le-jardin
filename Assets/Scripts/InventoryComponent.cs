using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryComponent : MonoBehaviour
{
    [field: SerializeField] public int Capacity { get; private set; }
    private const int MAX_CAPACITY = 8;

    [SerializeField] GameObject content;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] GameObject itemPrefab;
    private static List<Item> items;
    private static List<Slot> slots;

    private void Start()
    {
        items = new List<Item>();
        slots = new List<Slot>();

        for (int i = 0; i < Capacity; i++)
        {
            CreateNewSlot();
        }
    }

    public void Add(InventoryItem item)
    {
        // Security check
        if (item == null)
        {
            Debug.Log("Error: Can't add to inventory. No item found");
            return;
        }

        // Update quantity or create a new one
        if (ItemExists(item, out Item itemFound))
            itemFound.item.GetComponentInChildren<Text>().text = $"{++itemFound.quantity}";
        else if (!IsFull())
            CreateNewItem(item);
    }

    private void CreateNewItem(InventoryItem item)
    {
        Slot emptySlot = GetEmptySlot();

        GameObject newItem = Instantiate(itemPrefab, emptySlot.prefab.transform);
        newItem.GetComponent<Image>().sprite = item.Sprite;
        newItem.GetComponentInChildren<Text>().text = $"{item.Quantity}";

        items.Add(new Item(item.ID, newItem, 1));

        emptySlot.MarkAsFilled();
    }

    private Slot GetEmptySlot()
    {
        return slots.FirstOrDefault(s => s.filled == false);
    }

    private void CreateNewSlot()
    {
        GameObject slot = Instantiate(slotPrefab, content.transform);
        slot.name = $"Slot [{slots.Count}]";
        slots.Add(new Slot(slot, false));
    }

    private bool ItemExists(InventoryItem item, out Item itemFound)
    {
        itemFound = items.FirstOrDefault(i => i.id == item.ID);
        return itemFound != null;
    }

    public void Expand()
    {
        if (Capacity < MAX_CAPACITY)
        {
            CreateNewSlot();
            Capacity++;
        }
        else
        {
            Debug.Log($"Error: Can't expand more. You reached the maximum capacity ({MAX_CAPACITY}).");
        }
    }

    public bool IsFull() => items.Count >= Capacity;

    private class Slot
    {
        public GameObject prefab;
        public bool filled;

        public Slot(GameObject prefab, bool filled)
        {
            this.prefab = prefab;
            this.filled = filled;
        }

        public void MarkAsFilled()
        {
            filled = true;
        }
    }

    private class Item
    {
        public int id;
        public GameObject item;
        public int quantity;

        public Item(int id, GameObject item, int quantity)
        {
            this.id = id;
            this.item = item;
            this.quantity = quantity;
        }
    }
}
