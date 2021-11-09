using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : IUsable
{

    private readonly PlantType _typePlante;
    public bool Consumable => true;

    public string ID => $"seed_{_typePlante.ID}";
    string InventoryItem.Name => _typePlante.name;
    Sprite InventoryItem.Sprite => _typePlante.seedSprite;

    int InventoryItem.BasePrice => _typePlante.Price/2;

    string InventoryItem.Description => _typePlante.description;

    public bool Use(TileComponent tile)
    {
        if (tile.Plant(_typePlante)) 
        {
            GameSystem.Instance.seedSound.Play();
            return true;
        }
        return false;
    }

    public Seed(PlantType typePlante)
    {
        _typePlante = typePlante;
    }
}
