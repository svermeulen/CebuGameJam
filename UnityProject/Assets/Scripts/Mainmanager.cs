using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mainmanager : MonoBehaviour
{
    GameObject _;
    bool _hasInitializedLevelSelect = false;

    ///MAIN FUNCTION
    public void playbtn()
    {
        SceneManager.LoadScene(1);
    }

    public void selectlevel()
    {
        if (_hasInitializedLevelSelect)
        {
            return;
        }

        _hasInitializedLevelSelect = true;

        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;

        string[] scenes = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            var sceneName = System.IO.Path.GetFileNameWithoutExtension(
                UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
        }
    }

    public void selectSpecificLevel(GameObject button)
    {
    }

    public void exitbtn()
    {
        Application.Quit();
        //EditorApplication.Exit(0);
    }


}
