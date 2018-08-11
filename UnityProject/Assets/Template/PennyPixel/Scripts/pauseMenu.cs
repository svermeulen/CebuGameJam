using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class pauseMenu : MonoBehaviour
{

    public GameObject pauseUI;
    //public GameObject continueUI;


    //MAIN FUNCTION

    public void pauseBtn()
    {
        pauseUI.SetActive(true);
    }

    public void continueBtn()
    {
        pauseUI.SetActive(false);
    }

    public void skipLevel()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    public void retryBtn()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void exitBtn()
    {
        SceneManager.LoadScene(0);
    }
}
