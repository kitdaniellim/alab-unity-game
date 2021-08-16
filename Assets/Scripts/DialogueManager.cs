using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;

    public GameObject continueButton;
    public GameObject dialogueImage;
    private GameObject player;


    void Start(){
        player = GameObject.FindWithTag("Player");
        player.GetComponent<Player>().toggleMovement();
        dialogueImage.SetActive(true);
        StartCoroutine(Type());
    }

    void Update(){
        if(textDisplay.text == sentences[index]){
            continueButton.SetActive(true);
        }

        if (Input.GetKeyDown("e")) {
            NextSentence();
        }
    }

    IEnumerator Type(){
        foreach(char letter in sentences[index].ToCharArray()){
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence(){
        continueButton.SetActive(false);
        if(index < sentences.Length - 1){
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } else {
            textDisplay.text = "";
            continueButton.SetActive(false);
            dialogueImage.SetActive(false);
            player.GetComponent<Player>().toggleMovement();
        }
    }

}
