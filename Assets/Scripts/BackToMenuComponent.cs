using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenuComponent : MonoBehaviour
{
    public void BackToMenu()
    {
        FindObjectOfType<SceneNavigatorComponent>().Load(SceneNavigatorComponent.MENU);
    }
}
