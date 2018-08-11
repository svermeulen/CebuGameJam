using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltGroup : SwitchTriggerBase
{
    public BeltBehaviour[] Belts;
    public BeltStates OnState;
    public BeltStates OffState;

    public void Awake()
    {
        SyncBelts(OffState);
    }

    public override void Trigger(bool isOn)
    {
        SyncBelts(isOn ? OnState : OffState);
    }

    void SyncBelts(BeltStates state)
    {
        foreach (var belt in Belts)
        {
            belt.State = state;
        }
    }
}

