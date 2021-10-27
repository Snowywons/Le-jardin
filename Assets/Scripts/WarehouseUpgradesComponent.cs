using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseUpgradesComponent : MonoBehaviour
{
    [Header("Farmable zone")]
    [SerializeField] List<Upgrade> farmableZoneUpgrades;
    [SerializeField] Text farmableZoneLevelText;
    [SerializeField] Text farmableZonePriceText;
    [SerializeField] Button farmableZoneBuyButton;

    [Header("Watering can")]
    [SerializeField] List<Upgrade> wateringCanUpgrades;
    [SerializeField] Text wateringCanLevelText;
    [SerializeField] Text wateringCanPriceText;
    [SerializeField] Button wateringCanBuyButton;

    [Header("Inventory Slot")]
    [SerializeField] List<Upgrade> inventorySlotUpgrades;
    [SerializeField] Text inventorySlotLevelText;
    [SerializeField] Text inventorySlotPriceText;
    [SerializeField] Button inventorySlotBuyButton;

    [Header("Balance")]
    [SerializeField] Text balanceText;

    private GameSystem gs;
    private EconomyComponent eco;

    private void Start()
    {
        gs = GameSystem.Instance;
        eco = gs.GetComponent<EconomyComponent>();

        Init();
    }

    private void Init()
    {
        if (farmableZoneUpgrades != null && farmableZoneUpgrades.Count > 0)
        {
            UpdateUI(gs.farmableZoneLevel, farmableZoneUpgrades, farmableZoneLevelText, farmableZonePriceText, farmableZoneBuyButton);
        }

        balanceText.text = eco.Balance.ToString();
    }

    public void BuyFarmableZoneUpgrade()
    {
        int nextLevel = gs.farmableZoneLevel + 1;

        // Vérifie s'il y a des upgrades d'achetables
        if (nextLevel > farmableZoneUpgrades.Count) return;

        // Effectue l'achat
        if (eco.SafePay(farmableZoneUpgrades[nextLevel - 1].price))
        {
            UpdateUI(nextLevel, farmableZoneUpgrades, farmableZoneLevelText, farmableZonePriceText, farmableZoneBuyButton);
            gs.farmableZoneLevel++;
            gs.farmableZoneCount++;
        }
    }

    public void BuyWateringCanUpgrade()
    {
        int nextLevel = gs.wateringCanLevel + 1;

        // Vérifie s'il y a des upgrades d'achetables
        if (nextLevel > wateringCanUpgrades.Count) return;

        // Effectue l'achat
        if (eco.SafePay(wateringCanUpgrades[nextLevel - 1].price))
        {
            UpdateUI(nextLevel, wateringCanUpgrades, wateringCanLevelText, wateringCanPriceText, wateringCanBuyButton);
            gs.wateringCanLevel++;
        }
    }

    public void BuyInventorySlotUpgrade()
    {
        int nextLevel = gs.inventorySlotLevel + 1;

        // Vérifie s'il y a des upgrades d'achetables
        if (nextLevel > inventorySlotUpgrades.Count) return;

        // Effectue l'achat
        if (eco.SafePay(inventorySlotUpgrades[nextLevel - 1].price))
        {
            UpdateUI(nextLevel, inventorySlotUpgrades, inventorySlotLevelText, inventorySlotPriceText, inventorySlotBuyButton);
            gs.wateringCanLevel++;
        }
    }

    private void UpdateUI(int nextLevel, List<Upgrade> upgrades, Text levelText, Text priceText, Button buyButton)
    {
        bool isMaxLevel = nextLevel < upgrades.Count;
        levelText.text = isMaxLevel ? $"{nextLevel + 1}" : "MAX";
        priceText.text = isMaxLevel ? $"{farmableZoneUpgrades[nextLevel].price}$" : "";
        buyButton.interactable = isMaxLevel;
        balanceText.text = eco.Balance.ToString();
    }

    [Serializable]
    private class Upgrade
    {
        public int price;
    }
}