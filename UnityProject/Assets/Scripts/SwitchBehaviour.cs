using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    public SwitchTriggerBase[] Triggers;
    public Animator Animator;

    bool _isOverSwitch;
    bool _isOn;

    void OnTriggerEnter2D(Collider2D theCollider)
    {
        if (theCollider.CompareTag("Player"))
        {
            _isOverSwitch = true;
        }
    }

    void OnTriggerExit2D(Collider2D theCollider)
    {
        if (theCollider.CompareTag("Player"))
        {
            _isOverSwitch = false;
        }
    }

    public void Update()
    {
        if (_isOverSwitch && Input.GetKeyDown(KeyCode.F))
        {
            _isOn = !_isOn;
            Animator.SetBool("IsOn", _isOn);

            this.Invoke("TriggerTriggers", 0.5f);
        }
    }

    void TriggerTriggers()
    {
        foreach (var trigger in Triggers)
        {
            trigger.Trigger(_isOn);
        }
    }
}
