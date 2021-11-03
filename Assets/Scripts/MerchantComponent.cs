using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MerchantComponent : MonoBehaviour
{
    public List<PlantType> stock;

    public EconomyComponent balance;
    public InventoryComponent playerInventory;
    public RectTransform panel;
    public MerchantItemComponent prefabItem;
    public InspectorComponent inspectorPanel;

    [Header("Tabs")]
    [SerializeField] RectTransform buyTabRect;
    [SerializeField] RectTransform sellTabRect;
    private Vector2 initButtonSize;
    [SerializeField] Vector2 selectedButtonSize;

    [HideInInspector] public bool isSelling;
    private const float sellingPriceFactor = 0.50f;

    private SaveSystemComponent savesystem;

    void Start()
    {
        savesystem = FindObjectOfType<SaveSystemComponent>();
        inspectorPanel.merchant = this;
        initButtonSize = new Vector2(buyTabRect.rect.width, buyTabRect.rect.height);
        buyTabRect.sizeDelta = selectedButtonSize;
        UpdateUI();
    }

    public bool Buy(InventoryItem item, int quantity)
    {
        var total = CalculateUnitPrice(item) * quantity;
        if (balance.Pay(total)) 
        {
            if(playerInventory.Update(item, quantity))
            {
                savesystem.Save();
                return true;
            }
            balance.Add(total);
        }
        return false;
    }

    public bool Sell(InventoryItem item, int quantity)
    {
        var total = CalculateUnitPrice(item) * quantity;
        if (balance.Add(total))
        {
            if (playerInventory.Update(item, -quantity))
            {
                var slot = GameSystem.Instance.PlayerInventory.FindSlot(item);
                if (slot != null)
                    inspectorPanel.maxAmount = slot.content.quantity;
                else
                    UpdateUI();

                savesystem.Save();
                return true;
            }

            // Rembourse le joueur si une erreur est survenue
            balance.Pay(total);
        }
        return false;
    }

    public int CalculateUnitPrice(InventoryItem item) => isSelling ? (int)(item.BasePrice * sellingPriceFactor) : item.BasePrice;

    public void ShowInspector(InventoryItem item, MerchantItemComponent merchantItem)
    {
        inspectorPanel.gameObject.SetActive(true);
        inspectorPanel.SetItem(item);
        inspectorPanel.buyButton.gameObject.SetActive(!isSelling);
        inspectorPanel.sellButton.gameObject.SetActive(isSelling);

        var slot = GameSystem.Instance.PlayerInventory.FindSlot(item);
        inspectorPanel.maxAmount = slot != null && isSelling ? slot.content.quantity : 99;
        inspectorPanel.merchantItem = merchantItem;
    }

    public void SetSellingMode()
    {
        sellTabRect.sizeDelta = selectedButtonSize;
        buyTabRect.sizeDelta = initButtonSize;

        isSelling = true;
        UpdateUI();
    }

    public void SetBuyingMode()
    {
        sellTabRect.sizeDelta = initButtonSize;
        buyTabRect.sizeDelta = selectedButtonSize;
        isSelling = false;
        UpdateUI();
    }

    public void UpdateUI()
    {
        panel.transform.Clear();

        if (!isSelling)
        {
            foreach(var item in stock)
            {
                var listItem = Instantiate(prefabItem, panel);
                listItem.merchant = this;
                listItem.item = new Seed(item);
            }
        }
        else
        {
            foreach (var savedItem in savesystem.playerInventory)
            {
                if (savedItem.Value.item.Name != "Watering Can" &&
                    savedItem.Value.item.Name != "Scythe")
                {
                    var listItem = Instantiate(prefabItem, panel);
                    listItem.merchant = this;
                    var plante = GameSystem.Instance.Plants.Where(p => p.Name == savedItem.Value.item.Name).FirstOrDefault();
                    listItem.item = savedItem.Value.item;
                    listItem.quantity.text = $"{savedItem.Value.quantity}";
                }
            }
        }

        inspectorPanel.gameObject.SetActive(false);
    }
}
