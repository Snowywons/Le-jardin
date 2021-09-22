using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    private static GameSystem instance;
    public static GameSystem Instance
    {
        get 
        { 
            if (!instance) 
                Debug.Log("Instance is Empty"); 
            return instance;
        }

        private set 
        { 
            if (!instance) 
                instance = value; 
        }
    }

    public InventoryComponent Inventory { get; private set; }
    public ClockComponent Clock { get; private set; }

    [SerializeField] InventoryComponent inventory;
    [SerializeField] ClockComponent clock;

    private void Awake()
    {
        Instance = GetComponent<GameSystem>();
        Inventory = inventory;
        Clock = clock;
    }
}
