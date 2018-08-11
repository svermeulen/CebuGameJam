using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mainmanager : MonoBehaviour
{
    public GameObject SelectLevelPrefab;
    public Transform SelectLevelParent;
    public Transform MainMenuPanel;

    bool _hasInitializedLevelSelect = false;
    bool _isSelectingLevel = false;

    ///MAIN FUNCTION
    public void playbtn()
    {
        SceneManager.LoadScene(1);
    }

    void LazyInitLevelButtons()
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

            var button = GameObject.Instantiate(SelectLevelPrefab, SelectLevelParent).GetComponent<Button>();

            button.onClick.AddListener(() => SceneManager.LoadScene(sceneName));

            button.gameObject.GetComponentInChildren<Text>().text = sceneName;
        }
    }

    public void selectlevel()
    {
        LazyInitLevelButtons();
        SelectLevel(true);
    }

    void SelectLevel(bool isSelectingLevel)
    {
        if (_isSelectingLevel != isSelectingLevel)
        {
            _isSelectingLevel = isSelectingLevel;
            SelectLevelParent.gameObject.SetActive(isSelectingLevel);
            MainMenuPanel.gameObject.SetActive(!isSelectingLevel);
        }
    }

    public void Update()
    {
        if (_isSelectingLevel && Input.GetKeyDown(KeyCode.Escape))
        {
            SelectLevel(false);
        }
    }

    public void exitbtn()
    {
        Application.Quit();
        //EditorApplication.Exit(0);
    }


}
