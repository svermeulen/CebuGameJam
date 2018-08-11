using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mainmanager : MonoBehaviour


{



    ///MAIN FUNCTION
    public void playbtn()
    {
        SceneManager.LoadScene(1);

    }

    public void exitbtn()
    {
        Application.Quit();
        //EditorApplication.Exit(0);
    }


}
