using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryComponent : MonoBehaviour
{
    [field: SerializeField] public int Capacity { get; private set; }
    private const int MAX_CAPACITY = 8;

    [SerializeField] GameObject content;
    [SerializeField] InventorySlot slotPrefab;


    private List<Slot> slots;
    private Slot selected;
    public List<PlantType> plantes = new List<PlantType>();

    private void Start()
    {
        
        slots = new List<Slot>();

        for (int i = 0; i < Capacity; i++)
        {
            CreateNewSlot();
        }

        Add(FindObjectOfType<WateringComponent>(), 1, false);
        Add(FindObjectOfType<HarvestingComponent>(), 1, false);
        foreach (PlantType plante in plantes)
        {
            Add(new Seed(plante));
            Add(new Seed(plante));
        }

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

    public bool Add(InventoryItem item, int quantity = 1, bool outline = true)
    {
        // Security check
        if (item == null)
        {
            return true;
        }

        // Update quantity or create a new one
        var slot = FindSlot(item);
        if (slot != null)
        {
            slot.content.quantity += quantity;
            if (slot.content.item.Consumable)
                slot.prefab.quantity.text = $"{slot.content.quantity}";
            else
                slot.prefab.quantity.text = "";
            return true;
        }
        else if (!IsFull())
        {
            CreateNewItem(item, quantity, outline);
            return true;
        }
        return false;
    }
    public void Remove(InventoryItem item)
    {
        var slot = FindSlot(item);
        if(slot != null)
        {
            slot.content.quantity--;
            if(slot.content.quantity == 0)
            {
                slot.prefab.icon.sprite = null;
                slot.prefab.quantity.text = "";
                slot.content = null;
            }
            else if(item.Consumable)
            {
                slot.prefab.quantity.text = $"{slot.content.quantity}";
            }
        }
       
    }

    private void CreateNewItem(InventoryItem item, int quantity, bool outline)
    {
        Slot emptySlot = GetEmptySlot();
        if (emptySlot == null) return;

        if (item.Consumable)
            emptySlot.prefab.quantity.text = $"{quantity}";
        else
            emptySlot.prefab.quantity.text = "";

        emptySlot.prefab.icon.sprite = item.Sprite;
        emptySlot.content = new Item(item, quantity);
        emptySlot.prefab.outline.enabled = outline;

    }

    private Slot GetEmptySlot()
    {
        return slots.FirstOrDefault(s => s.content == null);
    }

    private void CreateNewSlot()
    {
        var slot = Instantiate(slotPrefab, content.transform);
        slot.name = $"Slot [{slots.Count}]";
        slot.index.text = $"{slots.Count + 1}";
        slots.Add(new Slot(slot, null, (KeyCode)((int)KeyCode.Alpha1+slots.Count)));
    }

    private bool ItemExists(InventoryItem item, out Item itemFound)
    {
        var slot = FindSlot(item);
        if(slot != null)
        {
            itemFound = slot.content;
            return true;
        }
        itemFound = null;
        return false;
    }
    private Slot FindSlot(InventoryItem item)
    {
        return slots.FirstOrDefault(x => x.content?.item.ID == item.ID);
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
        public InventorySlot prefab;
        public Item content;
        public KeyCode hotkey;

        public Slot(InventorySlot prefab, Item content, KeyCode hotkey)
        {
            this.prefab = prefab;
            this.content = content;
            this.hotkey = hotkey;
        }

    }

    private class Item
    {
        public InventoryItem item;
        public int quantity;

        public Item(InventoryItem item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
    }
}
