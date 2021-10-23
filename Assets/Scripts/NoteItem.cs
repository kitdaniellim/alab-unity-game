using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteItem : Interactable
{

    public string[] noteString;

    public override void Interact() {
        base.Interact();
        OpenNote();
    }  

    private void OpenNote() {
        Debug.Log("Reading note: " + noteString[0]);
        DialogueManager.instance.EnterDialogue(noteString);
    }
}
