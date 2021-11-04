using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//======================== CHALLENGE #1
//Data Structure
// array of structures
// struct {
//     string question;
//     bool flag = false;
//     string choice1;
//     string choice2;
//     string choice3;
//     string choice4;
//     int answer;
// }

[Serializable]
public struct question
{
    public string questionText;    //Question to ask
    public string choiceA;         //Choices
    public string choiceB;
    public string choiceC;
    public string choiceD; 
    public int answer;             //Answer ranges from 1-4 (choice1, choice2, etc., respectively)
    public bool flag;              //flag is true if answer has already been answered correctly
}

public class AdaptiveQManager : MonoBehaviour
{
    public TextMeshProUGUI questionDisplay;
    public float typingSpeed;
    public GameObject ChoiceA;
    public GameObject ChoiceB;
    public GameObject ChoiceC;
    public GameObject ChoiceD;

    public question[] currentQuestions = new question[5];
    private int index, numQuestions = 5, qScore = 0;
    private static int currentStage, targetStage;
    private bool isSet = false;

    [SerializeField]
    public question[] stage1Questions = new question[5]{
        new question() { 
            questionText = "What was the name of the Italian explorer who accompanied Ferdinand Magellan?", 
            choiceA = "Hatdog", 
            choiceB = "Chimi", 
            choiceC = "Antonio Pigafetta", 
            choiceD = "Aaaaaa", 
            answer = 3,
            flag = false,
        },
        new question() { 
            questionText = "Pigafetta characterized the people as _______________.", 
            choiceA = "Hatdog", 
            choiceB = "very familiar and friendly", 
            choiceC = "Bggg", 
            choiceD = "Aaaaaa", 
            answer = 2,
            flag = false,
        },
        new question() { 
            questionText = "The name of the first King of Zuluan and Calaguan, whom Magellan had met during his travels.", 
            choiceA = "Raia Siagu", 
            choiceB = "Chimi", 
            choiceC = "Bggg", 
            choiceD = "Aaaaaa", 
            answer = 1,
            flag = false,
        },
        new question() { 
            questionText = "The date when the mass by the shore had presided.", 
            choiceA = "Hatdog", 
            choiceB = "Chimi", 
            choiceC = "Bggg", 
            choiceD = "March 31st", 
            answer = 4,
            flag = false,
        },
        new question() { 
            questionText = "When explaining the reason for the cross, Magellan said the cross would _____________.", 
            choiceA = "Hatdog", 
            choiceB = "Chimi", 
            choiceC = "be beneficial because if other Spaniards come and see the cross, they would not cause troubles", 
            choiceD = "Aaaaaa", 
            answer = 3,
            flag = false,
        },
    };

    [SerializeField]
    public question[] stage2Questions = new question[5];

    [SerializeField]
    public question[] stage3Questions = new question[5];

    [SerializeField]
    public question[] stage4Questions = new question[5];


    #region Singleton
    public static AdaptiveQManager instance;

    void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of the adaptive questioning manager found!");
            return;
        }
        instance = this;    
    }
    #endregion

    void Start(){
        //======================== CHALLENGE #2 - Randomizing set of questions
        //On start, populate currentQuestions[] array with questions that are unflagged.
        setStage(1);
    }

    void Update(){
        if(questionDisplay.text != "" && questionDisplay.text == currentQuestions[index].questionText ){
            //======================== CHALLENGE #3 - User Interface buttons 
            //Disable button-clicking while question has not been shown yet
            if(!isSet) {
                isSet = true;
                // Debug.Log(ChoiceA.GetComponent<TextMeshProUGUI>());
                ChoiceA.GetComponent<TextMeshProUGUI>().text = currentQuestions[index].choiceA;
                ChoiceB.GetComponent<TextMeshProUGUI>().text = currentQuestions[index].choiceB;
                ChoiceC.GetComponent<TextMeshProUGUI>().text = currentQuestions[index].choiceC;
                ChoiceD.GetComponent<TextMeshProUGUI>().text = currentQuestions[index].choiceD;
            }
            ToggleActive(true);
        } else {
            isSet = false;
            ToggleActive(false);
        }

        if(currentStage != targetStage) {
            currentStage = targetStage;
            PopulateQuestions();
            StartCoroutine(Type());
        }
    }

    IEnumerator Type(){
        foreach(char letter in currentQuestions[index].questionText.ToCharArray()){
            questionDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextQuestion(int value){
        CheckAnswer(value);
        questionDisplay.text = "";
        if(index < numQuestions - 1){
            index++;
            StartCoroutine(Type());
        } else {
            index = 0;
            //======================== CHALLENGE #4 - Evaluation and Calculation 
            //On the last question, check if score is the minimum required value to pass.
            //If yes, show total score and proceed to next stage
            //Else, Retry and redo level, flag correctly answered questions.
            DisplayResults();
        }
        //Lastly, empty currentQuestions[] array.
        EmptyQuestions();
        
    }

    private void DisplayResults() {
        if(qScore >= 3) {
            ScoreManager.calculateTotalScore();
            questionDisplay.text =  "Alab Points: " + ScoreManager.getTotalScore().ToString() + 
                                    "\nEnemy Points: " + ScoreManager.getEnemyScore().ToString() +
                                    "\nLife Points: " + ScoreManager.getLifeScore().ToString() +
                                    "\nStage Completion Points: " + ScoreManager.getTotalTries().ToString() + 
                                    "\nTotal Score: " + ScoreManager.getTotalScore().ToString() +
                                    "\nYour questioning score is: " + qScore.ToString() + "/5. \nPassed!";
            //Display Proceed to Next Stage button
        } else {
            questionDisplay.text = "\nYour questioning score is: " + qScore.ToString() + "/5. \nTry again!";
            //Display Retry button
        }
    }

    //Easy
    private void CheckAnswer(int val) {
        //If correct, qScore + 1
        //Crosscheck with values of answer variable inside structure array
        if(val == currentQuestions[index].answer) {
            qScore++;
            Debug.Log("Correct! You answered: " + val + ". Correct answer: " + currentQuestions[index].answer + " qScore: " + qScore);
        } else {
            Debug.Log("Wrong! You answered: " + val + ". Correct answer: " + currentQuestions[index].answer + " qScore: " + qScore);
        }
    }

    //Easy, run through structure array and copy question variable into currentQuestions[] array.
    public void PopulateQuestions() {
        //Takes questions dataset and populates them into currentQuestions[] array.
        switch (currentStage)
        {
            case 1: 
                stage1Questions.CopyTo(currentQuestions, 0);
                Debug.Log("Case 1");
                break;
            default: stage1Questions.CopyTo(currentQuestions, 0);
                Debug.Log("Default case");
                break;
                
        }
    } 

    public static void setStage(int stageVal) {
        targetStage = stageVal;
        Debug.Log("Set target stage val to : " + stageVal);
    } 

    //Easy
    private void EmptyQuestions() {
        //Empties currentQuestions[] array

    }

    private void ToggleActive(bool val) {
        ChoiceA.SetActive(val);
        ChoiceB.SetActive(val);
        ChoiceC.SetActive(val);
        ChoiceD.SetActive(val);
    }

}
