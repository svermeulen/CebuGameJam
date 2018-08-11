
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public float KillzoneBuffer = 1.0f;
    public AudioSource WalkNoise;

    GameObject _ground;
    float _killLine;

    public static GameController Instance
    {
        get; private set;
    }

    public void Awake()
    {
        Instance = this;

        _ground = GameObject.Find("Ground");
    }

    void Start()
    {
        var collider = _ground.GetComponent<Collider2D>();

        _killLine = _ground.transform.position.y + collider.composite.bounds.min.y - KillzoneBuffer;
        Debug.Log(string.Format("_killLine = {0}", _killLine));
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

        WalkNoise.volume = ShouldPlayFlutterNoise() ? 1 : 0;

        CheckForDeaths();
    }

    void CheckForDeaths()
    {
        foreach (var player in GameRegistry.Instance.AllPlayers)
        {
            if (player.transform.position.y < _killLine)
            {
                player.Die();
            }
        }
    }

    bool ShouldPlayFlutterNoise()
    {
        foreach (var player in GameRegistry.Instance.AllPlayers)
        {
            if (Mathf.Abs(player.LastMove.x) > 0 && player.IsGrounded)
            {
                return true;
            }
        }

        return false;
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
