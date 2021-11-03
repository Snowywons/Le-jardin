using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class EconomyComponent : MonoBehaviour
{
    private SaveSystemComponent savesystem;
    [Header("References")]
    [SerializeField] string balancePanelName;

    private GameObject balancePanel;
    private List<Text> numbers;

    private const int maxBalance = 999999;

    public int Balance => savesystem.money;

    private void Start()
    {
        savesystem = FindObjectOfType<SaveSystemComponent>();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (!FindBalancePanel() || numbers == null) return;

        int balance = Balance;
        int i = 0;
        int numbersCount = numbers.Count;
        while (balance > 0 && i < numbersCount)
        {
            numbers[(numbersCount - 1) - i++].text = (balance % 10).ToString();
            balance /= 10;
        }
    }

    private bool FindBalancePanel()
    {
        if (balancePanel) 
            return true;

        balancePanel = GameObject.Find(balancePanelName);
        if (!balancePanel) 
            return false;

        numbers = new List<Text>();

        foreach (Transform obj in balancePanel.transform)
        {
            Text text = obj.GetComponent<Text>();
            if (text)
            {
                text.text = "";
                numbers.Add(text);
            }
        }

        return balancePanel != null;
    }

    public bool Add(int amount)
    {
        if (amount < 0)
            return false;

        savesystem.money = Math.Min(savesystem.money + amount, maxBalance);
        UpdateUI();

        return true;
    }

   
    // Permet d'effectuer un paiement seulement si notre solde contient la somme.
    public bool Pay(int amount)
    {
        if (amount < 0 || Balance - amount < 0)
            return false;

        savesystem.money -= amount;
        UpdateUI();

        return true;
    }
}
