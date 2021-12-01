using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitComponent : MonoBehaviour
{
    SaveSystemComponent savesystem;
    public void Awake()
    {
        savesystem = FindObjectOfType<SaveSystemComponent>();
    }
    
    public void Quit()
    {
        Application.Quit();
    }
    public void SaveAndQuit()
    {
        savesystem.Save();
        Application.Quit();
    }
}
