using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo
{
    public bool isWet;
    public PlantType plante;
    public int age;

    public TileInfo(bool isWet, PlantType typeplante, int ageplante)
    {
        this.isWet = isWet;
        this.plante = typeplante;
        this.age = ageplante;
    }
}
