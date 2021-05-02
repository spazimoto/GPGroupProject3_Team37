using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueSystem : MonoBehaviour
{
    PlayerScript player;

    public Text dialogueText;
    public GameObject dialogueBox;

    public int index;

    public Text nameText;
    public string npcName;
    [TextArea(3, 10)]
    public string[] sentences;

    public bool continueDialogue = true;

    void Start()
    {
        dialogueBox.gameObject.SetActive(false);

        player = GameObject.Find("Player").GetComponent<PlayerScript>();
    }


    public void DisplayNextSentence()
    {
        GameObject playerAnim = GameObject.Find("PyrrhaTextures");
        AnimationEvents animationEvent = playerAnim.GetComponent<AnimationEvents>();

        if(index < sentences.Length - 1)
        {
            animationEvent.respawned = false;

            dialogueBox.gameObject.SetActive(true);
            index++;
            nameText.text = npcName;
            dialogueText.text = "";
            StartCoroutine(TypeSentence());
        }
        else
        {
            animationEvent.respawned = true;

            index = 0;
            dialogueBox.gameObject.SetActive(false);
        }
    }

    
    IEnumerator TypeSentence()
    {
        continueDialogue = true;
        foreach(char letter in sentences[index].ToCharArray())
        {
            dialogueText.text += letter;
            continueDialogue = false;
            yield return new WaitForSeconds(0.01f);
        }
        continueDialogue = true;
    }

}
