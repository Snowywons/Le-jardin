using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemComponent : MonoBehaviour
{

    public Dictionary<string, TileInfo> tiles = new Dictionary<string, TileInfo>();
    public List<SavedItem> playerInventory = new List<SavedItem>();
    public List<SavedItem> warehouseInventory = new List<SavedItem>();

    //Valeurs initiales
    public void Start()
    {
        playerInventory.Add( new SavedItem { item = FindObjectOfType<WateringComponent>(), quantity = -1});
        playerInventory.Add( new SavedItem { item = FindObjectOfType<HarvestingComponent>(), quantity = -1 });

        foreach (PlantType plante in GameSystem.Instance.Plants)
        {
            playerInventory.Add(new SavedItem { item = new Seed(plante), quantity = 2});
            warehouseInventory.Add(new SavedItem { item = new Seed(plante), quantity = 2 });

        }


    }
}

public struct SavedItem
{
    public InventoryItem item;
    public int quantity;
}


