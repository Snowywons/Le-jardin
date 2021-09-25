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


    private static List<Slot> slots;
    private Slot selected;

    private void Start()
    {
        slots = new List<Slot>();

        DeleteExistingSlots();

        for (int i = 0; i < Capacity; i++)
        {
            CreateNewSlot();
        }
        Add(FindObjectOfType<WateringComponent>());
        Add(FindObjectOfType<HarvestingComponent>());
    }
    private void Update()
    {
        foreach(Slot slot in slots)
        {
            if (Input.GetKeyDown(slot.hotkey))
            {
                selected = slot;
                Debug.Log(slot.hotkey);
                break;
            }
        }
    }

    public InventoryItem GetSelected()
    {
        return selected?.content?.item;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == SceneNavigatorComponent.WAREHOUSE)
        {

        }

        if (level == SceneNavigatorComponent.WORLD)
        {

        }
    }

    private void DeleteExistingSlots()
    {
        content.transform.Clear();
    }

    public void Add(InventoryItem item, int quantity = 1)
    {
        // Security check
        if (item == null)
        {
            Debug.Log("Error: Can't add to inventory. No item found");
            return;
        }

        // Update quantity or create a new one
        if (ItemExists(item, out Item itemFound))
            itemFound.icon.GetComponentInChildren<Text>().text = $"{itemFound.quantity  + quantity}";
        else if (!IsFull())
            CreateNewItem(item, quantity);
    }

    private void CreateNewItem(InventoryItem item, int quantity)
    {
        Slot emptySlot = GetEmptySlot();
        if (emptySlot == null) return;

        GameObject newItem = Instantiate(itemPrefab, emptySlot.prefab.transform);
        newItem.GetComponent<Image>().sprite = item.Sprite;
        newItem.GetComponentInChildren<Text>().text = $"{quantity}";

        emptySlot.content = new Item(item, newItem, quantity);

    }

    private Slot GetEmptySlot()
    {
        return slots.FirstOrDefault(s => s.content == null);
    }

    private void CreateNewSlot()
    {
        GameObject slot = Instantiate(slotPrefab, content.transform);
        slot.name = $"Slot [{slots.Count}]";
        slot.GetComponentInChildren<Text>().text = $"{slots.Count + 1}";
        slots.Add(new Slot(slot, null, (KeyCode)((int)KeyCode.Alpha1+slots.Count)));
    }


    private bool ItemExists(InventoryItem item, out Item itemFound)
    {
        foreach(Slot slot in slots)
        {
            if(slot.content != null && slot.content.item.ID == item.ID)
            {
                itemFound = slot.content;
                return true;
            }
        }
        itemFound = null;
        return false;
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

    public bool IsFull() => slots.All(x => x.content != null);

    private class Slot
    {
        public GameObject prefab;
        public Item content;
        public KeyCode hotkey;

        public Slot(GameObject prefab, Item content, KeyCode hotkey)
        {
            this.prefab = prefab;
            this.content = content;
            this.hotkey = hotkey;
        }

    }

    private class Item
    {
        public InventoryItem item;
        public GameObject icon;
        public int quantity;

        public Item(InventoryItem item, GameObject icon, int quantity)
        {
            this.item = item;
            this.icon = icon;
            this.quantity = quantity;
        }
    }
}
