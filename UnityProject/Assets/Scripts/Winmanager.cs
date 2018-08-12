using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Winmanager : MonoBehaviour {



    public void playgainBtn()
    {
        SceneManager.LoadScene(1);
    }
    public void exitBtn()
    {
        SceneManager.LoadScene(0);
    }

}
