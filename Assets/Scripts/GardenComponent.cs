using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenComponent : MonoBehaviour
{
    private void Start()
    {
    }

    public void Example()
    {
        GameSystem.Instance.Inventory.Add();
    }
}
