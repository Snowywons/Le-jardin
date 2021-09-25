using UnityEngine;

public interface IUsable : InventoryItem
{
    public void Use(TileComponent tile);
}
