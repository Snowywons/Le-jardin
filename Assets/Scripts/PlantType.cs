using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Type Plante")]
public class PlantType : ScriptableObject
{
    public string nom;
    public float prixBase;
    public int maturingTime;
    public Transform young;
    public Transform mature;

}
