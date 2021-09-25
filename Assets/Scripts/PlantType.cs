using UnityEngine;

[CreateAssetMenu(menuName = "Type Plante")]
public class PlantType : ScriptableObject, InventoryItem
{
    public string ID;
    public string Name;
    public Sprite Sprite;

    public int Price;

    [Header("Plant properties")]
    public int maturingTime;
    public Transform young;
    public Transform mature;

    string InventoryItem.ID => ID;
    string InventoryItem.Name => Name;
    Sprite InventoryItem.Sprite => Sprite;
}
