
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
        GameOverScreenController.LastLoadedLevel = Application.loadedLevel;
        SceneManager.LoadScene("GameOverScreen");
    }

    void CheckForLevelEnd()
    {
        if (GameRegistry.Instance.AllPlayers.IsEmpty())
        {
            Application.LoadLevel(Application.loadedLevel + 1);
        }
    }
}
