using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseComponent : MonoBehaviour
{
    [SerializeField] GameObject outline;

    private void Start()
    {
        if (outline)
            outline.gameObject.SetActive(false);
    }

    void OnMouseEnter()
    {
        if (outline)
            outline.gameObject.SetActive(true);
    }
    void OnMouseExit()
    {
        if (outline)
            outline.gameObject.SetActive(false);
    }

    public void Enter()
    {
        GameSystem.Instance.Clock.SetPause(true);
    }

    public void Exit()
    {
        GameSystem.Instance.Clock.SetPause(true);
    }
}
