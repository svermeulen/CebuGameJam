using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    public SwitchTriggerBase[] Triggers;

    bool _isOnSwitch;

    void OnTriggerEnter2D(Collider2D theCollider)
    {
        if (theCollider.CompareTag("Player"))
        {
            _isOnSwitch = true;
        }
    }

    void OnTriggerExit2D(Collider2D theCollider)
    {
        if (theCollider.CompareTag("Player"))
        {
            _isOnSwitch = false;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FlipSwitch();
        }
    }

    void FlipSwitch()
    {
        foreach (var trigger in Triggers)
        {
            trigger.Trigger();
        }
    }
}
