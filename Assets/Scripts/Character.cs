using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;

[CreateAssetMenu(menuName = "Personnage")]
public class Character : ScriptableObject
{
    public Sprite neutral;
    public Sprite happy;
    public YarnProgram dialogue;
    public string startNode;
    public string nom;
    
}
