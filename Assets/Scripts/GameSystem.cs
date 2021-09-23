using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance { get; private set; }
    public InventoryComponent Inventory { get; private set; }
    public ClockComponent Clock { get; private set; }

    [SerializeField] InventoryComponent inventory;
    [SerializeField] ClockComponent clock;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = GetComponent<GameSystem>();
            Inventory = inventory;
            Clock = clock;
        }
    }

    private void Start()
    {
        if (!Instance) Debug.Log("Pas d'instance");
        if (!Inventory) Debug.Log("Pas d'inventaire");
        if (!Clock) Debug.Log("Pas de clock");
    }
}
