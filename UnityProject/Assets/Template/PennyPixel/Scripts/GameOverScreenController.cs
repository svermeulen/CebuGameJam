
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreenController : MonoBehaviour
{
    public static int LastLoadedLevel
    {
        get; set;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F))
        {
            Application.LoadLevel(LastLoadedLevel);
        }
    }
}

