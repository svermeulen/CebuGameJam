using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorBehaviour : SwitchTriggerBase
{
    public Transform DestinationPoint;
    public float MoveSpeed;
    public float Epsilong;

    State _state;
    Rigidbody2D _rigidBody;
    Vector3 _startPosition;
    Vector3 _goalPosition;

    public void Awake()
    {
        _startPosition = transform.position;
        _goalPosition = DestinationPoint.transform.position;

        _rigidBody = this.GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        if (_state != State.Idle)
        {
            var goalPos = _state == State.MovingToGoal ? _goalPosition : _startPosition;

            _rigidBody.velocity = (transform.position.y < goalPos.y ? Vector2.up : Vector2.down) * MoveSpeed;

            if ((transform.position - goalPos).magnitude < Epsilong)
            {
                _state = _state == State.MovingToGoal ? State.MovingToStart : State.MovingToGoal;
            }
        }
    }

    public override void Trigger(bool isOn)
    {
        if (_state == State.Idle)
        {
            _state = State.MovingToGoal;
        }
    }

    public enum State
    {
        Idle,
        MovingToStart,
        MovingToGoal,
    }
}

