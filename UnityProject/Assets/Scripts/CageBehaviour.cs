using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageBehaviour : SwitchTriggerBase
{
    public GameObject PlayerClonePrefab;

    bool _hasOpened;

    public void Awake()
    {
        GameRegistry.Instance.AddCage(this);
    }

    public override void Trigger()
    {
        if (_hasOpened)
        {
            return;
        }

        _hasOpened = true;
        GameObject.Instantiate(PlayerClonePrefab, transform.position, Quaternion.identity);
        GameRegistry.Instance.RemoveCage(this);
    }
}
