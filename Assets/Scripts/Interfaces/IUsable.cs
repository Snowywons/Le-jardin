using UnityEngine;

public interface IUsable : InventoryItem
{
    public bool Use(TileComponent tile);
}
