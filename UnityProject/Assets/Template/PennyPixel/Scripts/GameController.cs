
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public pauseMenu PauseMenu;
    public float KillzoneBuffer = 1.0f;
    public AudioSource WalkNoise;
    public Text LevelText;
    public GameObject GameOverPanel;

    GameObject _ground;
    float _killLine;
    bool _waitingToRestart;

    public static GameController Instance
    {
        get; private set;
    }

    public void Awake()
    {
        Instance = this;

        _ground = GameObject.Find("Ground");

        LevelText.text = string.Format("Level: {0}", Application.loadedLevelName);
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
            PauseMenu.pauseBtn();
        }

        WalkNoise.volume = ShouldPlayFlutterNoise() ? 1 : 0;

        CheckForDeaths();

        if (_waitingToRestart)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }

    void CheckForDeaths()
    {
        foreach (var player in GameRegistry.Instance.AllPlayers)
        {
            if (player.transform.position.y < _killLine)
            {
                player.Die(false);
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

    public void OnPlayerDied(PlayerPlatformerController deadPlayer)
    {
        SoundManager.Instance.PlayDied();

        _waitingToRestart = true;

        Invoke("ShowEndScreen", 0.25f);
    }

    void ShowEndScreen()
    {
        GameOverPanel.SetActive(true);
    }

    void CheckForLevelEnd()
    {
        if (GameRegistry.Instance.AllPlayers.IsEmpty())
        {
            Application.LoadLevel(Application.loadedLevel + 1);
        }
    }
}
