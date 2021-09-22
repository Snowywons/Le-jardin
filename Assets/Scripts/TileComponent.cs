using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileComponent : MonoBehaviour
{
    public bool isWet;
    public PlantType plante;
    public int age;


    public void Plant(PlantType type)
    {
        plante = type;
        age = 0;
    }
    public PlantType Harvest()
    {
        var temp = plante;
        plante = null;
        return temp;
    }

    public void OnDayAdvance()
    {
        Debug.Log("advance");
        if (plante == null) return;
        if (isWet)
        {
            age++;
            if (age == plante.maturingTime)
            {
                var planteMature = Instantiate(plante.mature);
                planteMature.position = new Vector3(0, 0.5f, 0);
                planteMature.SetParent(transform, false);
            }
            isWet = false;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnDayAdvance();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            isWet = true;
        }
    }
}
