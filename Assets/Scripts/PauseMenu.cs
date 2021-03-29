using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;

    public GameObject pauseMenu;

    void Start()
    {
        Resume();
    }

    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;

        Cursor.lockState = CursorLockMode.None;
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Quit()
    {
        print("Quitting game...");
        SceneManager.LoadScene("Start");
    }
}
