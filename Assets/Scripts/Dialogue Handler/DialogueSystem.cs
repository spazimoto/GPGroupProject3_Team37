using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueSystem : MonoBehaviour
{
    PlayerScript player;
    public Text nameText;
    public Text dialogueText;
    public GameObject dialogueBox;

    private Queue<string> sentences; //FIFO-- first in, first out

    void Start()
    {
        sentences = new Queue<string>();

        dialogueBox.gameObject.SetActive(false);

        player = GameObject.Find("Player").GetComponent<PlayerScript>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueBox.gameObject.SetActive(true);
        
        player.enabled = false;

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }


    void EndDialogue()
    {

        player.enabled = true;

        dialogueBox.gameObject.SetActive(false);

        Debug.Log("End of conversation.");
    }
}
