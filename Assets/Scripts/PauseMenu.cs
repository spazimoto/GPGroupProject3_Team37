using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;

    public GameObject pauseMenu;

    public GameObject controlsMenu;

    public GameObject[] inventory;

    void Start()
    {
        Resume();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene("testscene");
        }

        InventoryCheck();

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
        controlsMenu.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Controls()
    {
        controlsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void Back()
    {
        controlsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Quit()
    {
        print("Quitting game...");
        SceneManager.LoadScene("Start");
    }

    void InventoryCheck()
    {
        if(PlayerScript.gotWheel)
        {
            inventory[0].SetActive(true);
        }
        else
        {
            inventory[0].SetActive(false);
        }
        if(PlayerScript.gotEngine)
        {
            inventory[1].SetActive(true);
        }
        else
        {
            inventory[1].SetActive(false);
        }
        if(PlayerScript.gotSail)
        {
            inventory[2].SetActive(true);
        }
        else
        {
            inventory[2].SetActive(false);
        }
    }
}
