using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenuComponent : MonoBehaviour
{
    public List<Button> boutons;
    private SaveSystemComponent savesystem;

    private void Start()
    {
        savesystem = FindObjectOfType<SaveSystemComponent>();
        SetButtonText();
    }

    private void SetButtonText()
    {
        for(int i = 0; i < boutons.Count; ++i)
        {
            Text texte = boutons[i].GetComponentInChildren<Text>();
            texte.text = savesystem.GameExists(i + 1) ? $"Partie #{i + 1}" : "Sauvegarde vide";
        }
    }
}
