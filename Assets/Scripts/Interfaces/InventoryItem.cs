using UnityEngine;

public interface InventoryItem
{
    string ID { get; }
    string Name { get; }
    Sprite Sprite { get; }
    public bool Consumable { get; }
}