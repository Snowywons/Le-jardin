using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WateringComponent : MonoBehaviour, IUsable
{
    public string ID => "tool_watering";
    public string Name => "Watering Can";

    public Sprite Sprite;
    public bool Consumable => false;

    Sprite InventoryItem.Sprite => Sprite;

    public int BasePrice => 0;

    public string Description => "";

    public AudioSource soundEffect;

    public bool Use(TileComponent tile)
    {
        if (TileComponent.tiles.Count > 0)
        {
            foreach (var t in TileComponent.tiles)
                t.SetWet();
        }
        else
        {
            tile.SetWet();
        }

        soundEffect.Play();
        return true;
    }
}
