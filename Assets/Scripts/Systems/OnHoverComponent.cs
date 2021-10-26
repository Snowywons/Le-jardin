using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnHoverComponent : MonoBehaviour
{
    [SerializeField] UnityEvent onMouseEnter = new UnityEvent();
    [SerializeField] UnityEvent onMouseExit = new UnityEvent();

    void OnMouseEnter()
    {
        if (onMouseEnter != null)
            onMouseEnter.Invoke();
    }

    void OnMouseExit()
    {
        if (onMouseExit != null)
            onMouseExit.Invoke();
    }
}
