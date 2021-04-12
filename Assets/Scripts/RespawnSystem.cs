using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RespawnSystem : MonoBehaviour
{
    [SerializeField] private float minDeathHeight; //start to play falling sound
    [SerializeField] private float maxDeathHeight; //play hit sound

    Vector3 spawnPoint;

    Renderer playerRenderer;

    private float timeInvincible = 0f;
    private float cooldown = 3f;

    private bool isInvincible = false;

    public GameObject[] hearts;
    private int life;

    private bool dead;

    public AudioClip falling;
    public AudioClip hit;

    public float FlashingTime = .6f;
    public float TimeInterval = .1f;

    void Start()
    {
        life = hearts.Length;
        dead = false;

        spawnPoint = transform.position;
    }

    void Update()
    {
        print ("I have " + life + " lives.");

        PlayerScript playerScript = GetComponent<PlayerScript>();

        if(transform.position.y < minDeathHeight)
        {
            //falling sound goes here?

            if (transform.position.y < maxDeathHeight)
            {
                playerScript.controller.enabled = false;
                transform.position = spawnPoint;
                playerScript.SFXControl(hit);

                if (Time.time > timeInvincible)
                {
                    TakeDamage();
                    Cooldown();
                }
            }
        }
        else
        {
            playerScript.controller.enabled = true;
        }

        if (dead == true)
        {
            playerScript.anim.SetBool("isDead", true);
            playerScript.controller.enabled = false;
        }

        print(spawnPoint);
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Checkpoint"))
        {
            spawnPoint = collider.gameObject.GetComponent<Transform>().position;

            Destroy(collider.gameObject);
        }
    }

        void Cooldown()
    {
        timeInvincible = Time.time + cooldown;
    }

    public void TakeDamage()
    {
        if (life >= 1)
        {
            life -= 1;
            Destroy(hearts[life].gameObject);

            if (life < 1)
            {
                dead = true;
            }
        }
    }
}
