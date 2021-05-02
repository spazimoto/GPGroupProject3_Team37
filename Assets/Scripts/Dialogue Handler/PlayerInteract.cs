using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    PlayerScript player;
    public AudioClip greet;

    public GameObject levelSelect;

    void Awake()
    {
        player = GetComponent<PlayerScript>();
        levelSelect.gameObject.SetActive(false);
    }

    void Update()
    {
        DialogueHandler();
    }

    private void DialogueHandler()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        DialogueSystem dialogueTrigger = FindObjectOfType<DialogueSystem>();

        GameObject playerAnim = GameObject.Find("PyrrhaTextures");
        AnimationEvents animationEvent = playerAnim.GetComponent<AnimationEvents>();

        GameObject crosshair = GameObject.Find("Crosshair");

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 15)) //origin, direction, returns a boolean if hit = true
        {
            if(raycastHit.collider.tag == "NPC")
            {
                crosshair.GetComponent<Image>().color = Color.red;

                dialogueTrigger = raycastHit.transform.gameObject.GetComponent<DialogueSystem>();

                if(dialogueTrigger.continueDialogue)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        player.SFXControl(greet);
                        dialogueTrigger.DisplayNextSentence();
                    }
                }
            }
            else
            {
                crosshair.GetComponent<Image>().color = Color.white;
            }

            if (raycastHit.collider.tag == "Taxi")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    levelSelect.gameObject.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    animationEvent.respawned = false;
                }

                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    levelSelect.gameObject.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    animationEvent.respawned = true;
                }
            }
            
        }
    }

    public void LoadMesa()
    {
        SceneManager.LoadScene("Ancestral Mesa (Desert Level)");
    }

    public void LoadOasis()
    {
        SceneManager.LoadScene("OasisLevel");
    }

    public void LoadFactory()
    {
        SceneManager.LoadScene("Mystical Jungle");
    }
}
