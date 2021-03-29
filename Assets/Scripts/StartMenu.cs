using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    public void Play()
    {
        print("Playing game...");
        SceneManager.LoadScene("Ancestral Mesa (Desert Level)");
    }

    public void Controls()
    {
        print("Opening controls...");
        SceneManager.LoadScene("Controls");
    }

    public void Options()
    {
        print("Loading options...");
    }
    public void Quit()
    {
        print("Quitting game...");
        Application.Quit();
    }
}
