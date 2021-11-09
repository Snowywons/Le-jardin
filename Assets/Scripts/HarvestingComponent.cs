using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HarvestingComponent : MonoBehaviour, IUsable
{
    public string ID => "tool_harvesting";
    public string Name => "Scythe";
    public bool Consumable => false;

    Sprite InventoryItem.Sprite => Sprite;

    public int BasePrice => 0;

    public string Description => "";

    public Sprite Sprite;

    public AudioSource soundEffect;

    public bool Use(TileComponent tile)
    {
        soundEffect.Play();
        return tile.Harvest();
    }
}
