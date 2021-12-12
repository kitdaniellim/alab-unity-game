using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    
    public TextMeshProUGUI textDisplay;
    public string[] sentences = new string[5];
    public float typingSpeed;
    private int index;
    private int arrayLength;
    private bool isEntered;

    public GameObject continueButton;
    public GameObject dialogueImage;

    #region Singleton
    public static DialogueManager instance;

    void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of the dialogue manager found!");
            return;
        }
        instance = this;    
    }
    #endregion

    void Start(){
        // Player.instance.toggleMovement();
        // dialogueImage.SetActive(true);
        // StartCoroutine(Type());
    }

    void Update(){
        // currentlyDisplayedText
        if(textDisplay.text != "" && textDisplay.text == sentences[index] ){
            continueButton.SetActive(true);
        }

        if (Input.GetKeyDown("f")) {
            NextSentence();
        }
    }

    //Executed when OpenNote function inside Note class is run, displays string content from note onscreen
    public void EnterDialogue(string[] content){
        // Player.instance.toggleMovement();
        if(!isEntered) {
            isEntered = true;
            dialogueImage.SetActive(true);
            content.CopyTo(sentences, 0);
            arrayLength = content.Length;
            StartCoroutine(Type());
        }
    }

    void EndDialogue(){
        isEntered = false;
        continueButton.SetActive(false);
        dialogueImage.SetActive(false);
        // Player.instance.toggleMovement();
    }

    IEnumerator Type(){
        foreach(char letter in sentences[index].ToCharArray()){
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence(){
        continueButton.SetActive(false);
        textDisplay.text = "";
        if(index < arrayLength - 1){
            index++;
            StartCoroutine(Type());
        } else {
            index = 0;
            EndDialogue();
        }
    }

}
