using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectorComponent : MonoBehaviour
{
    public Text itemName;
    public Image itemSprite;
    public Text itemDescription;
    public Text itemQuantity;
    public Text totalPrice;
    public Button addButton;
    public Button removeButton;
    public Button buyButton;
    public Button sellButton;

    private InventoryItem item;
    private int amount;
    [HideInInspector] public int maxAmount;

    public MerchantComponent merchant;
    [HideInInspector] public MerchantItemComponent merchantItem;
    
    public void SetItem(InventoryItem item)
    {
        this.item = item;
        itemName.text = item.Name;
        itemSprite.sprite = item.Sprite;
        totalPrice.text = $"{ merchant.CalculateUnitPrice(item)}$";
        itemDescription.text = item.Description;
        amount = 1;
        itemQuantity.text = $"{amount}";
    }

    public void AddQuantity()
    {
        if (++amount <= maxAmount)
        {
            itemQuantity.text = $"{amount}";
            totalPrice.text = $"{amount * merchant.CalculateUnitPrice(item)}$";
        }
        else
        {
            amount--;
        }
    }

    public void RemoveQuantity()
    {
        if(amount > 1)
        {
            amount--;
            itemQuantity.text = $"{amount}";
            totalPrice.text = $"{amount * merchant.CalculateUnitPrice(item)}$";
        }
    }

    public void Buy()
    {
        if(merchant.Buy(item, amount))
        {
            amount = 1;
            itemQuantity.text = $"{amount}";
            totalPrice.text = $"{amount * merchant.CalculateUnitPrice(item)}$";
        }
    }

    public void Sell()
    {
        if (merchant.Sell(item, amount))
        {
            amount = 1;
            itemQuantity.text = $"{amount}";
            totalPrice.text = $"{amount * merchant.CalculateUnitPrice(item)}$";

            if (merchantItem)
                merchantItem.quantity.text = $"{maxAmount}";
        }
    }
}
