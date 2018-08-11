
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance
    {
        get; private set;
    }

    public void Awake()
    {
        Instance = this;
    }

    public void OnPlayerExited(
        PlayerPlatformerController player)
    {
        CheckForLevelEnd();
    }

    public void OnPlayerDied(
        PlayerPlatformerController player)
    {
        SceneManager.LoadScene("GameOverScreen");
    }

    void CheckForLevelEnd()
    {
        if (GameRegistry.Instance.AllPlayers.IsEmpty())
        {
            Debug.Log("Completed Level!!");
            Application.LoadLevel(Application.loadedLevel + 1);
        }
    }
}
