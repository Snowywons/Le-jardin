using UnityEngine;

[CreateAssetMenu(menuName = "Type Plante")]
public class PlantType : ScriptableObject, InventoryItem
{
    public string ID;
    public string Name;
    public Sprite Sprite;
    public Sprite seedSprite;
    public string description;

    public int Price;
    public int scorePoints;

    [Header("Plant properties")]
    public int maturingTime;
    public Transform young;
    public Transform mature;
    public Transform seed;

    string InventoryItem.ID => ID;
    string InventoryItem.Name => Name;
    Sprite InventoryItem.Sprite => Sprite;
    bool InventoryItem.Consumable => true;

    int InventoryItem.BasePrice => Price;

    string InventoryItem.Description => description;
}
