using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HarvestingComponent : MonoBehaviour, IUsable
{
    public string ID => "tool_harvesting";
    public string Name => "Scythe";
    public bool Consumable => false;

    Sprite InventoryItem.Sprite => Sprite;

    public Sprite Sprite;

    public void Use(TileComponent tile)
    {
        tile.Harvest();
    }
}
