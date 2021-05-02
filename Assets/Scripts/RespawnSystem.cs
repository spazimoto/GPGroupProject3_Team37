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

    public AudioClip death;
    public AudioClip revive;

    float scaleX = 0.5f;
    float scaleY = 0.5f;
    float scaleZ = 0.5f;

    void Start()
    {
        PlayerScript playerScript = GetComponent<PlayerScript>();
        AnimationController animController = GetComponent<AnimationController>();

        life = hearts.Length;
        dead = false;
        spawnPoint = transform.position;

        playerScript.SFXControl(revive);
        animController.anim.SetTrigger("isAlive");
    }

    void Update()
    {

        PlayerScript playerScript = GetComponent<PlayerScript>();
        AnimationController animController = GetComponent<AnimationController>();

        GameObject playerAnim = GameObject.Find("PyrrhaTextures");
        AnimationEvents animationEvent = playerAnim.GetComponent<AnimationEvents>();

        if (!animationEvent.respawned)
        {
            playerScript.enabled = false;
        }
        else
        {
            playerScript.enabled = true;
        }

        if (transform.position.y < minDeathHeight)
        {
            if (transform.position.y < maxDeathHeight)
            {
                playerScript.controller.enabled = false;
                transform.position = spawnPoint;
                playerScript.SFXControl(hit);

                TakeDamage();
                /*if (Time.time > timeInvincible)
                {
                    TakeDamage();
                    Cooldown();
                }*/
            }
        }
        else
        {
            playerScript.controller.enabled = true;
        }

        if (dead == true)
        {
            animController.anim.SetBool("isDead", true);
            playerScript.enabled = false;


            //playerScript.SFXControl(death);
        }

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Checkpoint"))
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
            AnimationController animController = GetComponent<AnimationController>();

            animController.anim.SetTrigger("isHurt");
            life -= 1;
            hearts[life].GetComponent<RectTransform>().localScale = new Vector3(scaleX, scaleY, scaleZ);
            hearts[life].GetComponent<Image>().color = Color.grey;
            Destroy(hearts[life].GetComponent<Animation>());

            if (life < 1)
            {
                dead = true;
            }
        }
    }
}
