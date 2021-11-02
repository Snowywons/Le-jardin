using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class MerchantItemComponent : MonoBehaviour
{
    private Image sprite;
    public InventoryItem item;
    public MerchantComponent merchant;

    private void Start()
    {
        sprite = GetComponent<Image>();
        sprite.sprite = item.Sprite;
    }

    public void Click()
    {
        merchant.OuvrirDescription(item);
    }
}
