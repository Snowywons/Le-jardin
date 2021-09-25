using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WateringComponent : MonoBehaviour, IUsable
{
    public string ID => "tool_watering";
    public string Name => "Watering Can";

    Sprite InventoryItem.Sprite => Sprite;

    public Sprite Sprite;

    public void Use(TileComponent tile)
    {
        tile.SetWet();
    }
}
