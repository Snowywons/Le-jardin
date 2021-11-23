using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilePopupComponent : MonoBehaviour
{
    public Image imageContainer;

    public void UpdateImage(PlantType plante, int age)
    {
        gameObject.SetActive(true);

        if (plante == null)
            gameObject.SetActive(false);
        else if (age > plante.maturingTime)
            imageContainer.sprite = plante.seedSprite;
        else if (age == plante.maturingTime)
            imageContainer.sprite = plante.Sprite;
        else
            gameObject.SetActive(false);
    }
}
