using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantComponent : MonoBehaviour
{
    public List<PlantType> stock;

    public EconomyComponent balance;
    public InventoryComponent playerInventory;
    public RectTransform panel;
    public MerchantItemComponent prefabItem;

    public ItemDescriptionComponent infoPanel;

    // Start is called before the first frame update
    void Start()
    {
        infoPanel.marchand = this;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool Acheter(InventoryItem item, int quantity)
    {
        var total = item.BasePrice * quantity;
        if (balance.Pay(total)) 
        {
            if(playerInventory.Update(item, quantity))
                return true;
            balance.Add(total);
        }
        return false;
    }
    public void Ajouter()
    {

    }
    public void Enlever()
    {

    }

    public void OuvrirDescription(InventoryItem item)
    {
        infoPanel.gameObject.SetActive(true);
        infoPanel.SetItem(item);
        
    }

    public void UpdateUI()
    {
        foreach(var item in stock)
        {
            var listItem = Instantiate(prefabItem, panel);
            listItem.merchant = this;
            listItem.item = new Seed(item);
        }
        infoPanel.gameObject.SetActive(false);
    }
}
