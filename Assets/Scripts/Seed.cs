using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : IUsable
{

    private readonly PlantType _typePlante;
    public bool Consumable => true;

    string InventoryItem.ID => $"seed_{_typePlante.ID}";
    string InventoryItem.Name => _typePlante.name;
    Sprite InventoryItem.Sprite => _typePlante.seedSprite;

    public bool Use(TileComponent tile)
    {
        return tile.Plant(_typePlante);
    }

    public Seed(PlantType typePlante)
    {
        _typePlante = typePlante;
    }
}
