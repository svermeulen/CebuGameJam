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
    public void retryBtn()
    {
        SceneManager.LoadScene(1);
    }
    public void exitBtn()
    {
        SceneManager.LoadScene(0);
    }
}
