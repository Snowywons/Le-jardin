using UnityEngine;

public interface InventoryItem
{
    int ID { get; set; }
    string Name { get; set; }
    int Quantity { get; set; }
    int Price { get; set; }
    Sprite Sprite { get; set; }
}

