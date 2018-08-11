
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
        Invoke("CheckForLevelEnd", 1.0f);
        SoundManager.Instance.PlayExitted();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Mainmenu");
        }
    }

    public void OnPlayerDied(
        PlayerPlatformerController player)
    {
        SoundManager.Instance.PlayDied();

        Invoke("GoToNextLevel", 1.0f);
    }

    void GoToNextLevel()
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
