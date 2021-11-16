using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MerchantComponent : MonoBehaviour
{
    private enum Market
    {
        Public,
        Goblin
    }

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
    private const float sellingPriceFactor = 0.50f; // Normal Day. Revendu à 50% de son prix de base

    private const float sellingDiscountFactor = 0.65f; // Discount Day. Revendu à 65% de son prix de base
    private const float buyingDiscountFactor = 0.75f; // Discount Day. Acheté à 75% de son prix de base
    private const float goblinSellingDiscountFactor = 0.90f; // Goblin Discount Day. Revendu à 90% de son prix de base

    private SaveSystemComponent savesystem;
    private DiscountComponent discountComponent;

    private Market market;

    void Start()
    {
        savesystem = FindObjectOfType<SaveSystemComponent>();
        discountComponent = FindObjectOfType<DiscountComponent>();
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

    public int CalculateUnitPrice(InventoryItem item)
    {
        int day = GameSystem.Instance.Clock.GetDay() - 1;
        var discount = discountComponent.discountsList.Where(d => d.day == day).FirstOrDefault();

        // S'il s'agit d'une journée spéciale
        if (discount != null)
        {
            // Si le joueur est en mode VENTE
            if (isSelling)
            {
                // Si c'est une journée spéciale de PLANTE
                if (discount.discountType.Equals(DiscountType.Plant))
                {
                    // Si c'est la plante en rabais, alors il y a un discount sur la vente
                    if (discount.plantType != null &&
                        (discount.plantType.ID == item.ID || "seed_" + discount.plantType.ID == item.ID))
                    {
                        return (int)(item.BasePrice * sellingDiscountFactor);
                    }
                    // Si ce n'est pas la plante en rabais, alors aucun discount sur la vente
                    else
                    {
                        return (int)(item.BasePrice * sellingPriceFactor);
                    }
                }
                // Si c'est une journée spéciale du GOBLIN
                else if (market.Equals(Market.Goblin))
                {
                    return (int)(item.BasePrice * goblinSellingDiscountFactor);
                }
            }
            // Si le joueur est en mode ACHAT
            else
            {
                // Si c'est la plante en rabais, alors il y a un discount sur l'achat
                if (discount.plantType != null &&
                    (discount.plantType.ID == item.ID || "seed_" + discount.plantType.ID == item.ID))
                {
                    return (int)(item.BasePrice * buyingDiscountFactor);
                }
                // Si ce n'est pas la plante en rabais, alors aucun discount sur l'achat
                else
                {
                    return item.BasePrice;
                }
            }
        }

        // S'il ne s'agit PAS d'une journée spéciale
        return isSelling ? (int)(item.BasePrice * sellingPriceFactor) : item.BasePrice;
    }

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
                    listItem.item = savedItem.Value.item;
                    listItem.quantity.text = $"{savedItem.Value.quantity}";
                }
            }
        }

        inspectorPanel.gameObject.SetActive(false);
    }

    public void PublicMarketSelected()
    {
        market = Market.Public;
    }

    public void GoblinMarketSelected()
    {
        market = Market.Goblin;
    }
}
