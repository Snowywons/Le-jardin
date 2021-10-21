using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;


public class DialogueStarter : MonoBehaviour
{
    public Image spriteBox;
    public Character personnage;
    public DialogueRunner runner;


    private void Start()
    {
        runner.Add(personnage.dialogue);
    }
    public void StartDialogue()
    {
        runner.StartDialogue(personnage.startNode);
    }

    [YarnCommand("setSprite")]
    public void SetSprite(string emotion)
    {
        switch (emotion)
        {
            case "neutral":
                spriteBox.sprite = personnage.neutral;
                break;
            case "happy":
                spriteBox.sprite = personnage.happy;
                break;
            default:
                Debug.Log("Pas de sprite");
                break;
        }
    }
}
