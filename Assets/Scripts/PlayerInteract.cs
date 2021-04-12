using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    PlayerScript player;

    public AudioClip greet;

    public GameObject levelSelect;

    void Awake()
    {
        player = gameObject.GetComponent<PlayerScript>();
        levelSelect.gameObject.SetActive(false);
    }

    void Update()
    {
        DialogueHandler();
    }
    private void DialogueHandler()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Interactable dialogueTrigger;
        DialogueSystem continueTrigger = FindObjectOfType<DialogueSystem>();

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 15)) //origin, direction, returns a boolean if hit = true
        {
            if(raycastHit.collider.tag == "NPC")
            {
                dialogueTrigger = raycastHit.transform.gameObject.GetComponent<Interactable>();

                if(Input.GetMouseButtonDown(0))
                {
                    player.SFXControl(greet);
                    
                    dialogueTrigger.TriggerDialogue();
                }
                if(Input.GetMouseButtonDown(1))
                {
                    continueTrigger.DisplayNextSentence();
                }
            }

            if (raycastHit.collider.tag == "Taxi")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    levelSelect.gameObject.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    player.enabled = false;
                }

                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    levelSelect.gameObject.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    player.enabled = true;
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
