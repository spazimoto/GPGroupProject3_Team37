using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    AudioSource audio;
    public AudioClip beep;
    public AudioClip boop;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    public void HoverSFX()
    {
        audio.PlayOneShot(beep);
    }

    public void ClickSFX()
    {
        audio.PlayOneShot(boop);
    }
}
