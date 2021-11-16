using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;

public class DiscountComponent : MonoBehaviour
{
    [Header("Discounts")]
    [SerializeField] int minDiscountsCount = 5;
    [SerializeField] int maxDiscountsCount = 15;

    [Header("Goblin Days")]
    [SerializeField] int minGoblinDaysCount = 2;
    [SerializeField] int maxGoblinDaysCount = 6;
    [SerializeField] Sprite golbinSprite;

    public List<Discount> discountsList = new List<Discount>();

    private void Start()
    {
        Randomize();
    }

    private void Randomize()
    {
        
        SaveSystemComponent savesystem = FindObjectOfType<SaveSystemComponent>();
        var rando = new System.Random(savesystem.randomSeed);
        List<int> allDays = Enumerable.Range(1, 31).ToList();
        List<PlantType> plantTypes = savesystem.plants;

        int discountsCount = rando.Next(minDiscountsCount, maxDiscountsCount);

        for (int i = 0; i < discountsCount; i++)
        {
            int n = rando.Next(0, allDays.Count);
            int day = allDays.ElementAt(n);
            allDays.RemoveAt(n);

            PlantType plant = plantTypes.ElementAt(rando.Next(0, plantTypes.Count));

            discountsList.Add(new Discount(day, DiscountType.Plant, plant.Sprite, plant));
        }

        int goblinDaysCount = rando.Next(minGoblinDaysCount, maxGoblinDaysCount);

        for (int i = 0; i < goblinDaysCount; i++)
        {
            int n = rando.Next(0, allDays.Count);
            int day = allDays.ElementAt(n);
            allDays.RemoveAt(n);

            discountsList.Add(new Discount(day, DiscountType.Goblin, golbinSprite));
        }
    }
}

public enum DiscountType
{
    Plant,
    Goblin
}

public class Discount
{
    public int day;
    public DiscountType discountType;
    public Sprite sprite;
    public PlantType plantType;

    public Discount(int day, DiscountType discountType, Sprite sprite, PlantType plantType = null)
    {
        this.day = day;
        this.discountType = discountType;
        this.sprite = sprite;
        this.plantType = plantType;
    }
}
