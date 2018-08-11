using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageBehaviour : SwitchTriggerBase
{
    public Transform PlayerPosition;
    public GameObject PlayerClonePrefab;

    PlayerPlatformerController _player;
    bool _hasOpened;

    public void Awake()
    {
        GameRegistry.Instance.AddCage(this);

        _player = GameObject.Instantiate(
            PlayerClonePrefab, PlayerPosition.transform.position, Quaternion.identity)
            .GetComponent<PlayerPlatformerController>();

        _player.IsInCage = true;
        _player.GetComponent<SpriteRenderer>().sortingOrder = -7;
    }

    public override void Trigger(bool isOn)
    {
        if (_hasOpened)
        {
            return;
        }

        _player.IsInCage = false;
        _player.GetComponent<SpriteRenderer>().sortingOrder = 5;
        _hasOpened = true;
        GameRegistry.Instance.RemoveCage(this);
    }
}
