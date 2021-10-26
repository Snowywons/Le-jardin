using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo
{
    public bool isWet;
    public PlantType plante;
    public int age;

    public TileInfo(bool iswet, PlantType typeplante, int ageplante)
    {
        isWet = iswet;
        this.plante = typeplante;
        this.age = ageplante;
    }
}
