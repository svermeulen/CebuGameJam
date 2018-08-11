using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltBehaviour : MonoBehaviour
{
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

    public void FixedUpdate()
    {
        // TODO
        //foreach (var player in _players)
        //{
            //player.Exit();
        //}
    }
}

