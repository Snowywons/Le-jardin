using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseComponent : MonoBehaviour
{
    public void Enter()
    {
        GameSystem.Instance.Clock.Pause();
    }

    public void Exit()
    {
        GameSystem.Instance.Clock.Resume();
    }
}
