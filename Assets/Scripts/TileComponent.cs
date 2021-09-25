using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileComponent : MonoBehaviour
{
    public bool isWet;
    public PlantType plante;
    public int age;
    public Material wetMaterial;
    public Material dryMaterial;

    private Transform planteMature;
    public bool instantGrow; // For Debug Only

    private void Start()
    {
        if (instantGrow)
        {
            for (int i = 0; i < plante.maturingTime; i++, isWet = true)
                OnDayAdvance();
        }
    }

    public void Plant(PlantType type)
    {
        plante = type;
        age = 0;
    }
    public void Harvest()
    {
        // Security check
        if (plante == null)
        {
            Debug.Log("Error: Can't harvest. No item found.");
            return;
        }

        if (!GameSystem.Instance.Inventory.IsFull())
        {
            GameSystem.Instance.Inventory.Add(plante);
            TileReset();
        }
        else
        {
            // TO DO : Informer le joueur que son inventaire est plein.
        }
    }

    public void SetWet()
    {
        isWet = true;
        GetComponent<MeshRenderer>().material = wetMaterial;
    }

    public void OnInteract()
    {
        var inventaire = FindObjectOfType<InventoryComponent>();

        var selection = inventaire.GetSelected();
        if (selection is IUsable usable)
        {
            usable.Use(this);
        }
    }

    private void TileReset()
    {
        age = 0;
        plante = null;

        // Security check
        if (planteMature)
            Destroy(planteMature.gameObject);
    }

    public void OnDayAdvance()
    {
        Debug.Log("advance");
        
        
        if (isWet)
        {
            GetComponent<MeshRenderer>().material = dryMaterial;
            isWet = false;
            if (plante != null)
            {
                age++;
                if (age == plante.maturingTime)
                {
                    planteMature = Instantiate(plante.mature);
                    planteMature.position = new Vector3(0, 0.5f, 0);
                    planteMature.SetParent(transform, false);
                }
            }
        }

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnDayAdvance();
        }
        
    }
}
