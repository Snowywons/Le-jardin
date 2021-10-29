using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemComponent : MonoBehaviour
{
    public Dictionary<string, TileInfo> tiles = new Dictionary<string, TileInfo>();
    public Dictionary<int, SavedItem> playerInventory = new Dictionary<int, SavedItem>();
    public Dictionary<int, SavedItem> warehouseInventory = new Dictionary<int, SavedItem>();
    public int playerInventoryCapacity = 4;
    public int warehouseInventoryCapacity = 20;

    //Valeurs initiales
    public void Start()
    {
        playerInventory.Add( playerInventory.Count, new SavedItem { item = FindObjectOfType<WateringComponent>(), quantity = -1});
        playerInventory.Add(playerInventory.Count, new SavedItem { item = FindObjectOfType<HarvestingComponent>(), quantity = -1 });

        foreach (PlantType plante in GameSystem.Instance.Plants)
        {
            if(playerInventory.Count < playerInventoryCapacity) 
                playerInventory.Add(playerInventory.Count, new SavedItem { item = new Seed(plante), quantity = 2});
            if(warehouseInventory.Count < warehouseInventoryCapacity)
                warehouseInventory.Add(warehouseInventory.Count, new SavedItem { item = new Seed(plante), quantity = 2 });
        }
    }
}

public struct SavedItem
{
    public InventoryItem item;
    public int quantity;

    public SavedItem(InventoryItem item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
