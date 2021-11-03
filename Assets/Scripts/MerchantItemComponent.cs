using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantItemComponent : MonoBehaviour
{
    [SerializeField] Image sprite;
    public Text quantity;

    public InventoryItem item;
    public MerchantComponent merchant;

    private void Start()
    {
        sprite.sprite = item.Sprite;
    }

    public void Click()
    {
        merchant.ShowInspector(item, this);
    }
}
