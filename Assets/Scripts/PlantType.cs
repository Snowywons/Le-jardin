using UnityEngine;

[CreateAssetMenu(menuName = "Type Plante")]
public class PlantType : ScriptableObject, InventoryItem
{
    [field: SerializeField] public int ID { get; set; }
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public int Quantity { get; set; }
    [field: SerializeField] public int Price { get; set; }
    [field: SerializeField] public Sprite Sprite { get; set; }

    [Header("Plante properties")]
    public int maturingTime;
    public Transform young;
    public Transform mature;
}
