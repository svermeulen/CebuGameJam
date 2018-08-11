using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageBehaviour : SwitchTriggerBase
{
    public Transform PlayerPosition;
    public GameObject PlayerClonePrefab;
    public GameObject Bars;
    public bool IsReverse;

    PlayerPlatformerController _player;
    bool _hasOpened;

    public void Start()
    {
        GameRegistry.Instance.AddCage(this);

        _player = GameObject.Instantiate(
            PlayerClonePrefab, PlayerPosition.transform.position, Quaternion.identity)
            .GetComponent<PlayerPlatformerController>();

        _player.SetIsFrozen(true);
        _player.GetComponent<SpriteRenderer>().sortingOrder = -7;
    }

    public override void Trigger(bool isOn)
    {
        if (_hasOpened)
        {
            return;
        }

        Bars.SetActive(false);

        _player.SetIsFrozen(false);
        _player.SetIsReverse(IsReverse);
        _player.GetComponent<SpriteRenderer>().sortingOrder = 5;
        _hasOpened = true;
        GameRegistry.Instance.RemoveCage(this);
    }
}
