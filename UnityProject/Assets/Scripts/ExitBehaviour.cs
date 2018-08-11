using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class ExitBehaviour : MonoBehaviour
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && GameRegistry.Instance.AllCages.IsEmpty())
        {
            foreach (var player in _players.ToList())
            {
                player.Exit();
            }
        }
    }
}
