using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WateringComponent : MonoBehaviour, IUsable
{
    public string ID => "tool_watering";
    public string Name => "Watering Can";
    public bool Consumable => false;

    Sprite InventoryItem.Sprite => Sprite;

    public Sprite Sprite;

    public bool Use(TileComponent tile)
    {
        tile.SetWet();
        return true;
    }
}
