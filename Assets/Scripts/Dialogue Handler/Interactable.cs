using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    //this script handles the trigger for dialogue
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueSystem>().StartDialogue(dialogue);
    }
}
