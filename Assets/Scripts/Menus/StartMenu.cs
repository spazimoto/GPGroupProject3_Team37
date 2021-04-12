using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    AudioSource audio;
    public AudioClip select;

    void Awake()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }
    public void Play()
    {
        audio.PlayOneShot(select);
        SceneManager.LoadScene("Ezra's Market (Hub Level)");
    }

    public void Controls()
    {
        audio.PlayOneShot(select);
        SceneManager.LoadScene("Controls");
    }

    public void Options()
    {
        audio.PlayOneShot(select);
        SceneManager.LoadScene("Credits");
    }
    public void Quit()
    {
        audio.PlayOneShot(select);
        Invoke("Application.Quit()", 1);
    }
}
