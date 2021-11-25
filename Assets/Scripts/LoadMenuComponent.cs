using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenuComponent : MonoBehaviour
{
    public List<Button> boutons;
    public List<Button> boutonsDelete;
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
            UpdateButtons(i +1);
        }
    }

    public void UpdateButtons(int slot)
    {
        bool gameExists = savesystem.GameExists(slot);
        Text texte = boutons[slot -1].GetComponentInChildren<Text>();
        texte.text = gameExists ? $"Partie #{slot}" : "Sauvegarde vide";
        boutonsDelete[slot-1].gameObject.SetActive(gameExists);
    }
}
