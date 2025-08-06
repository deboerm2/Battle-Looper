using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    private GameObject pauseMenu;

    private void Start()
    {
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu"))
            pauseMenu = transform.Find("PauseMenu").gameObject;

        Time.timeScale = 1;
    }
    public void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void CloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

    public void PauseGame()
    {
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu"))
            {
                PauseGame();
            }
        }
    }
}
