using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BeltStates
{
    Off,
    Left,
    Right
}

public class BeltBehaviour : MonoBehaviour
{
    public MeshRenderer Renderer;
    public float TileAnimateSpeed = 0.01f;
    public float ForceScale = 0.01f;
    public BeltStates State;
    public float Speed = 1.0f;

    List<PlayerPlatformerController> _players = new List<PlayerPlatformerController>();

    void OnCollisionEnter2D(Collision2D theCollider)
    {
        if (theCollider.collider.CompareTag("Player"))
        {
            var player = theCollider.collider.gameObject.GetComponent<PlayerPlatformerController>();
            Assert.IsNotNull(player);
            _players.Add(player);
        }
    }

    void OnCollisionExit2D(Collision2D theCollider)
    {
        if (theCollider.collider.CompareTag("Player"))
        {
            var player = theCollider.collider.gameObject.GetComponent<PlayerPlatformerController>();
            Assert.IsNotNull(player);
            _players.RemoveWithConfirm(player);
        }
    }

    public void Update()
    {
        if (State == BeltStates.Left || State == BeltStates.Right)
        {
            Renderer.material.mainTextureOffset = new Vector2(
                Renderer.material.mainTextureOffset.x + Speed * TileAnimateSpeed * (State == BeltStates.Left ? 1 : -1), 0);
        }
    }

    public void FixedUpdate()
    {
        foreach (var player in _players)
        {
            switch (State)
            {
                case BeltStates.Left:
                {
                    player.PlatformMove += -1.0f * Speed;
                    break;
                }
                case BeltStates.Right:
                {
                    player.PlatformMove += 1.0f * Speed;
                    break;
                }
            }
        }
    }
}

