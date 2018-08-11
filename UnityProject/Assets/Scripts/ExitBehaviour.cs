using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (Input.GetKeyDown(KeyCode.F))
        {
            ExitPlayers();
            CheckForLevelEnd();
        }
    }

    void CheckForLevelEnd()
    {
        if (GameRegistry.Instance.AllCages.IsEmpty()
            && GameRegistry.Instance.AllPlayers.IsEmpty())
        {
            Application.LoadLevel(Application.loadedLevel + 1);
        }
    }

    void ExitPlayers()
    {
        foreach (var player in _players)
        {
            player.Exit();
        }
    }
}
