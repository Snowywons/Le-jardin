using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public enum InventoryMode
{
    Player,
    Warehouse
}

public class InventoryComponent : MonoBehaviour
{
    [Header("Options")]
    public InventoryMode mode = InventoryMode.Player;
    [SerializeField] bool hotkey;
    [SerializeField] bool showSlotIndex;

    [Header("Properties")]
    [SerializeField] int capacityMin = 9;
    [SerializeField] int capacityMax = 9;
    public int Capacity { get; private set; }

    [Header("References")]
    private GameObject canvasContent;
    [SerializeField] string canvasContentName;
    [SerializeField] ItemSlot slotPrefab;

    public List<Slot> slots;
    private Slot selected;
    private SaveSystemComponent saveSystem;

    private GameObject recycleSlotContainer;

    private void Start()
    {
        saveSystem = FindObjectOfType<SaveSystemComponent>();

        recycleSlotContainer = new GameObject();
        recycleSlotContainer.name = $"{mode} Recycle Slot Container";
        recycleSlotContainer.transform.SetParent(transform);

        slots = new List<Slot>();
        Capacity = capacityMin;

        InitSlots();

        if (mode.Equals(InventoryMode.Player)) InitPlayerInventory();
        if (mode.Equals(InventoryMode.Warehouse)) InitWarehouseInventory();

        UpdateUI();
    }

    private void Update()
    {
        if (hotkey)
        {
            foreach (Slot slot in slots)
            {
                if (Input.GetKeyDown(slot.hotkey))
                {
                    SlotSelect(slot);
                    break;
                }
            }
        }
    }

    private void InitSlots()
    {
        for (int i = 0; i < Capacity; i++)
            CreateNewSlot();
    }

    public void UpdateUI()
    {
        if (!FindCanvasContent() || slots == null)
            return;

        RecycleAll();

        foreach (Slot slot in slots)
        {
            slot.prefab.transform.SetParent(canvasContent.transform);
            slot.prefab.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
            slot.prefab.GetComponent<RectTransform>().transform.localScale = Vector3.one;
        }
    }

    private bool FindCanvasContent()
    {
        var allCanvasContent = (CanvasContentComponent[]) FindObjectsOfType(typeof(CanvasContentComponent), true);
        var found = allCanvasContent.Where(o => o.name == canvasContentName).FirstOrDefault();
        canvasContent = found ? found.gameObject : null;
        return canvasContent != null;
    }

    private void InitPlayerInventory()
    {
        foreach (SavedItem item in saveSystem.playerInventory)
        {
            Add(item.item, item.quantity, true);
        }

        SlotSelect();
    }

    private void InitWarehouseInventory()
    {
        foreach(SavedItem item in saveSystem.warehouseInventory)
        {
            Add(item.item, item.quantity, true);
        }

    }

    private void SlotSelect(Slot slot = null)
    {
        if (slot == null)
        {
            selected = slots.FirstOrDefault();
            selected.prefab.border.gameObject.SetActive(true);
            return;
        }

        if (selected != null)
            selected.prefab.border.gameObject.SetActive(false);

        selected = slot;
        selected.prefab.border.gameObject.SetActive(true);
    }

    public InventoryItem GetSelected()
    {
        return selected?.content?.item;
    }

    public bool Update(InventoryItem item, int quantity)
    {
        if (item == null) return false;

        // Mettre à jour la quantité ou ajouter l'item à un nouveau slot
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
            Add(item, quantity, true);
            return true;
        }

        return false;
    }

    private void Add(InventoryItem item, int quantity = 1, bool outline = true)
    {
        Slot emptySlot = FindSlot();
        if (emptySlot == null) return;

        emptySlot.prefab.quantity.text = quantity > -1 ? $"{quantity}" : "";
        emptySlot.prefab.icon.sprite = item.Sprite;
        emptySlot.prefab.icon.enabled = true;
        emptySlot.content = new Item(item, quantity);
        emptySlot.prefab.outline.enabled = outline;
    }

    public void Remove(InventoryItem item, bool all = false)
    {
        var slot = FindSlot(item);
        if (slot != null)
        {
            slot.content.quantity -= all ? slot.content.quantity : 1;
            slot.prefab.quantity.text = $"{slot.content.quantity}";

            if (slot.content.quantity <= 0)
            {
                slot.prefab.icon.sprite = null;
                slot.prefab.icon.enabled = false;
                slot.prefab.quantity.text = "";
                slot.content = null;
            }
        }  
    }

    private void CreateNewSlot(bool addToList = true)
    {
        ItemSlot slot = Instantiate(slotPrefab, recycleSlotContainer.transform);
        slot.name = $"Slot [{slots.Count}]";
        slot.index.text = showSlotIndex ? $"{slots.Count + 1}" : "";
        slot.icon.enabled = false;
        slot.border.gameObject.SetActive(false);

        if (addToList)
            slots.Add(new Slot(slot, null, (KeyCode)((int)KeyCode.Alpha1+slots.Count)));
    }

    public void CopyToNewSlot(Slot copyFrom)
    {
        Slot emptySlot = FindSlot();
        emptySlot.prefab.icon.sprite = copyFrom.prefab.icon.sprite;
        emptySlot.prefab.icon.enabled = copyFrom.prefab.icon.enabled;
        emptySlot.content = copyFrom.content;
        emptySlot.prefab.outline.enabled = copyFrom.prefab.outline.enabled;
        emptySlot.prefab.quantity.text = copyFrom.prefab.quantity.text;

        copyFrom.Clear();
    }

    public Slot FindSlot(InventoryItem item) => slots.FirstOrDefault(x => x.content?.item.ID == item.ID);

    public Slot FindSlot(int index) => slots[index];

    public Slot FindSlot() => slots.FirstOrDefault(s => s.content == null);

    public bool IsFull() => slots.All(x => x.content != null);

    public void Expand()
    {
        if (Capacity < capacityMax)
        {
            Capacity++;
            CreateNewSlot();
            UpdateUI();
        }
    }

    public void ClearAll()
    {
        slots.Clear();
    }

    public void RecycleAll()
    {
        if (slots == null) return;

        //Déplace tous les slots vers le conteneur de récupération de slots
        foreach (var s in slots)
            s.prefab.transform.SetParent(recycleSlotContainer.transform);
    }

    public void Sort()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].content == null)
            {
                for (int j = i + 1; j < slots.Count; j++)
                {
                    if (slots[j].content != null)
                    {
                        slots[i].Copy(slots[j]);
                        slots[j].Clear();
                        break;
                    }
                }
            }
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        // Raffraichissement du UI sur un changement de scène
        UpdateUI();
    }
}

public class Slot
{
    public ItemSlot prefab;
    public Item content;
    public KeyCode hotkey;

    public Slot(ItemSlot prefab, Item content, KeyCode hotkey)
    {
        this.prefab = prefab;
        this.prefab.slot = this;
        this.content = content;
        this.hotkey = hotkey;
    }

    public void Update()
    {
        prefab.quantity.text = content.quantity.ToString();
    }

    public void Clear()
    {
        prefab.icon.sprite = null;
        prefab.icon.enabled = false;
        prefab.quantity.text = "";
        content = null;
    }

    public void Copy(Slot slot)
    {
        prefab.icon.sprite = slot.prefab.icon.sprite;
        prefab.icon.enabled = slot.prefab.icon.enabled;
        prefab.quantity.text = slot.prefab.quantity.text;
        content = slot.content;
    }
}

public class Item
{
    public InventoryItem item;
    public int quantity;

    public Item(InventoryItem item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
