using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EconomyComponent : MonoBehaviour
{
    [Header("Initial values")]
    [SerializeField] int startBalance;
    [SerializeField] int minBalance;
    [SerializeField] int maxBalance;

    [Header("References")]
    [SerializeField] string balancePanelName;

    private GameObject balancePanel;
    private List<Text> numbers;

    public int Balance { get; private set; }

    private void Start()
    {
        Balance = startBalance;
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

        Balance += amount;

        if (Balance > maxBalance)
            Balance = maxBalance;

        return true;
    }

    // Permet d'effectuer un paiement même si notre solde ne contient pas la somme.
    public bool UnsafePay(int amount)
    {
        if (amount < 0)
            return false;

        Balance -= amount;

        if (Balance < minBalance)
            Balance = minBalance;

        return true;
    }

    // Permet d'effectuer un paiement seulement si notre solde contient la somme.
    public bool SafePay(int amount)
    {
        if (amount < 0 || Balance - amount < 0)
            return false;

        Balance -= amount;

        return true;
    }

    public void Clear()
    {
        Balance = 0;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        UpdateUI();
    }
}
