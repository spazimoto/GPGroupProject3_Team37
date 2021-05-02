using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationEvents : MonoBehaviour
{
    Animator animator;

    public bool respawned = false;

    AudioSource audio;
    public AudioClip step1;
    public AudioClip step2;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void RespawnIn()
    {
        respawned = true;
    }

    public void FirstStep()
    {
        CharacterController controller = GameObject.Find("Player").GetComponent<CharacterController>();

        if (controller.isGrounded)
        {
            audio.PlayOneShot(step1);
        }
    }

    public void SecondStep()
    {
        CharacterController controller = GameObject.Find("Player").GetComponent<CharacterController>();

        if(controller.isGrounded)
        {
            audio.PlayOneShot(step2);
        }
    }
}
