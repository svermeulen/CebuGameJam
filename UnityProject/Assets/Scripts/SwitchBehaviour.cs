using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    public SwitchTriggerBase[] Triggers;
    public Animator Animator;

    bool _isOn;

    List<PlayerPlatformerController> _players = new List<PlayerPlatformerController>();

    void OnTriggerEnter2D(Collider2D theCollider)
    {
        if (theCollider.CompareTag("Player"))
        {
            var player = theCollider.gameObject.GetComponent<PlayerPlatformerController>();
            Assert.IsNotNull(player);
            _players.Add(player);
        }
    }

    void OnTriggerExit2D(Collider2D theCollider)
    {
        if (theCollider.CompareTag("Player"))
        {
            var player = theCollider.gameObject.GetComponent<PlayerPlatformerController>();
            Assert.IsNotNull(player);
            _players.RemoveWithConfirm(player);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !_players.IsEmpty())
        {
            _isOn = !_isOn;
            Animator.SetBool("IsOn", _isOn);
            SoundManager.Instance.PlaySwitch();

            foreach (var trigger in Triggers)
            {
                trigger.Trigger(_isOn);
            }
        }
    }
}
