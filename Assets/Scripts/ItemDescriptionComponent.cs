using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionComponent : MonoBehaviour
{
    public Text name;
    public Text description;
    public Image sprite;
    public Button acheter;
    public Text quantity;
    public Text price;
    public Button plus;
    public Button minus;

    private InventoryItem item;
    private int amount;

    public MerchantComponent marchand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetItem(InventoryItem item)
    {
        this.item = item;
        name.text = item.Name;
        sprite.sprite = item.Sprite;
        price.text = item.BasePrice.ToString();
        description.text = item.Description;
        amount = 1;
        quantity.text = amount.ToString();

    }
    public void AddQuantity()
    {
        amount++;
        quantity.text = amount.ToString();
        price.text = (amount * item.BasePrice).ToString();
    }
    public void RemoveQuantity()
    {
        if(amount > 1)
        {
            amount--;
            quantity.text = amount.ToString();
            price.text = (amount * item.BasePrice).ToString();
        }
    }
    public void Acheter()
    {
        if(marchand.Acheter(item, amount))
        {
            amount = 1;
            quantity.text = amount.ToString();
            price.text = (amount * item.BasePrice).ToString();

        }

    }
}
