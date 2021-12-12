using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random=UnityEngine.Random;
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
    public int id;                 //Question unique identifier
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
    [SerializeField]
    private TextMeshProUGUI qScoreDisplay;

    public GameObject resultsPass;
    public GameObject resultsFail;
    public float typingSpeed;
    public GameObject ChoiceA;
    public GameObject ChoiceB;
    public GameObject ChoiceC;
    public GameObject ChoiceD;

    public question[] currentQuestions = new question[5];
    private int index, numQuestions = 5, qScore = 0;
    public static int currentStage, targetStage, nextStage  = 1;
    private int passingScore = 3;
    private int maxQuestionsPerStage = 5;
    private bool isSet = false, didPass = false;

    [SerializeField]
    public question[] stage1Questions = new question[]{
        new question() { 
            id = 1,
            questionText = "What was the name of the Italian explorer who accompanied Ferdinand Magellan?", 
            choiceA = "Jorge Salcedo", 
            choiceB = "Ferdinand Magellan", 
            choiceC = "Antonio Pigafetta", 
            choiceD = "Lapu-lapu", 
            answer = 3,
            flag = false,
        },
        new question() {
            id = 2, 
            questionText = "Pigafetta characterized the people as _______________.", 
            choiceA = "weird and awkward", 
            choiceB = "very familiar and friendly", 
            choiceC = "thick and muscley", 
            choiceD = "animals and savages", 
            answer = 2,
            flag = false,
        },
        new question() {
            id = 3, 
            questionText = "The name of the first King of Zuluan and Calaguan, whom Magellan had met during his travels.", 
            choiceA = "Raia Siagu", 
            choiceB = "Raia Judaoh", 
            choiceC = "King Fernandez", 
            choiceD = "Raia Bibimbap", 
            answer = 1,
            flag = false,
        },
        new question() {
            id = 4, 
            questionText = "The date when the mass by the shore had presided.", 
            choiceA = "April 19th", 
            choiceB = "December 25th", 
            choiceC = "January 21st", 
            choiceD = "March 31st", 
            answer = 4,
            flag = false,
        },
        new question() {
            id = 5, 
            questionText = "When explaining the reason for the cross, Magellan said the cross would _____________.", 
            choiceA = "call upon the divine judgement of God", 
            choiceB = "make it rain lumpia and lechon the next day", 
            choiceC = "be beneficial because if other Spaniards come and see the cross, they would not cause troubles", 
            choiceD = "be great for tourism", 
            answer = 3,
            flag = false,
        },
        new question() {
            id = 6, 
            questionText = "The name of the king who offered to guide Magellan and his crew to the other islands.", 
            choiceA = "Raia Calambu", 
            choiceB = "Raia Alejandro", 
            choiceC = "Raia Kiko", 
            choiceD = "Raia Carlos", 
            answer = 1,
            flag = false,
        },
        new question() {
            id = 7, 
            questionText = "The first signs of gold were found here on ___________.", 
            choiceA = "Hiroko Island", 
            choiceB = "Humunu Island", 
            choiceC = "Bear Island", 
            choiceD = "Wakwak Shore", 
            answer = 2,
            flag = false,
        },
        new question() {
            id = 8, 
            questionText = "The town which Pigafetta referred to as 'The Watering Place of Good Signs'.", 
            choiceA = "Mactan", 
            choiceB = "Cebu", 
            choiceC = "Siargao Island", 
            choiceD = "Humunu Island", 
            answer = 4,
            flag = false,
        },
        new question() {
            id = 9, 
            questionText = "This king offered to pilot Magellan going to Cebu.", 
            choiceA = "King Buena", 
            choiceB = "King Carlos", 
            choiceC = "King Calambu", 
            choiceD = "King Aldana", 
            answer = 3,
            flag = false,
        },
        new question() {
            id = 10, 
            questionText = "The Ladrones Islands were also referred by Pigafetta as the __________.", 
            choiceA = "Fishmaker's Island", 
            choiceB = "Island of Alluring Ladies", 
            choiceC = "Island of the Thieves", 
            choiceD = "Land of Coconut Trees and Milk", 
            answer = 3,
            flag = false,
        },
    };

    [SerializeField]
    public question[] stage2Questions = new question[]{
        new question() { 
            id = 1,
            questionText = "Magellan and his men reached the port of Cebu on ________.", 
            choiceA = "May 25th", 
            choiceB = "April 7th", 
            choiceC = "December 1st", 
            choiceD = "February 3rd", 
            answer = 2,
            flag = false,
        },
        new question() { 
            id = 2,
            questionText = "The king of ___ demanded that Megallan pay tribute as it was customary.", 
            choiceA = "Cebu", 
            choiceB = "Siargao", 
            choiceC = "Leyte", 
            choiceD = "Bohol", 
            answer = 1,
            flag = false,
        },
        new question() { 
            id = 3,
            questionText = "After a few days, the King and Magellan met in ___________.", 
            choiceA = "the king's backyard", 
            choiceB = "the Altar of the Gods", 
            choiceC = "front of Magellan's cross", 
            choiceD = "an open space", 
            answer = 4,
            flag = false,
        },
        new question() { 
            id = 4,
            questionText = "The king and Magellan offered each other _________ to signify friendship.", 
            choiceA = "chickens", 
            choiceB = "two cows", 
            choiceC = "blood", 
            choiceD = "three Bulbul (bird) feathers", 
            answer = 3,
            flag = false,
        },
        new question() { 
            id = 5,
            questionText = "When the king demanded that Magellan pay tribute as it was customary, Magellan __________.", 
            choiceA = "ignored the king", 
            choiceB = "punched his face", 
            choiceC = "cried", 
            choiceD = "refused", 
            answer = 4,
            flag = false,
        },
        new question() { 
            id = 6,
            questionText = "The reason why the king wanted Magellan to pay tribute.", 
            choiceA = "the king was greedy", 
            choiceB = "it was customary", 
            choiceC = "the king wanted power", 
            choiceD = "no particular reason", 
            answer = 2,
            flag = false,
        },
        new question() { 
            id = 7,
            questionText = "Magellan explained that his king was the emperor of a great empire _____________________.", 
            choiceA = "while the king of Cebu is even greater", 
            choiceB = "and he should kneel", 
            choiceC = "but the king of Cebu is my new king", 
            choiceD = "and it would do them better to make friends with them than forge enmity", 
            answer = 4,
            flag = false,
        },
    };

    [SerializeField]
    public question[] stage3Questions = new question[]{
        new question() { 
            id = 1,
            questionText = "To show how to be a good Christian, Magellan encouraged the king to ___________.", 
            choiceA = "burn all of the idols and worshiping the cross instead", 
            choiceB = "worship both dolls and Jesus", 
            choiceC = "keep worshipping the dolls", 
            choiceD = "sacrifice his people", 
            answer = 1,
            flag = false,
        },
        new question() { 
            id = 2,
            questionText = "When asked to become a Christian, Rajah Humabon ____________.", 
            choiceA = "was outraged and waged war with Magellan", 
            choiceB = "ignored the request", 
            choiceC = "refused", 
            choiceD = "agreed and was baptized as a Christian", 
            answer = 4,
            flag = false,
        },
        new question() { 
            id = 3,
            questionText = "The name that was given to Rajah Humabon by Magellan after his baptismal.", 
            choiceA = "Mang Tomas", 
            choiceB = "Prinsipio Manuel", 
            choiceC = "Don Carlos", 
            choiceD = "Don Juan", 
            answer = 3,
            flag = false,
        },
        new question() { 
            id = 4,
            questionText = "The punishment for obeying neither the king nor Magellan was ____________.", 
            choiceA = "the burning down of villages", 
            choiceB = "Malaria", 
            choiceC = "enslavement", 
            choiceD = "none", 
            answer = 1,
            flag = false,
        },
        new question() { 
            id = 5,
            questionText = "After _______ days, Pigafetta recounted that all of the island’s inhabitants were already baptized.", 
            choiceA = "120", 
            choiceB = "365", 
            choiceC = "15", 
            choiceD = "8", 
            answer = 4,
            flag = false,
        },
        new question() { 
            id = 6,
            questionText = "When the queen came to the mass one day, Magellan gave her an image of ___________ made by Pigafetta himself.", 
            choiceA = "Magellan", 
            choiceB = "the Infant Jesus", 
            choiceC = "a cross", 
            choiceD = "her husband, King Humabon ", 
            answer = 2,
            flag = false,
        },
        new question() { 
            id = 7,
            questionText = "When Magellan reiterated that all of the newly baptized Christians need to burn their idols, the natives __________.", 
            choiceA = "migrated away to avoid the new religion", 
            choiceB = "ignored Magellan", 
            choiceC = "gave excuses", 
            choiceD = "committed mass suicide", 
            answer = 3,
            flag = false,
        },
        new question() { 
            id = 8,
            questionText = "The natives claimed that they needed the idols ___________.", 
            choiceA = "to heal a sick man", 
            choiceB = "to produce more crops", 
            choiceC = "to make rain", 
            choiceD = "for display", 
            answer = 1,
            flag = false,
        },
        new question() { 
            id = 9,
            questionText = "Magellan insisted that the natives should put their faith in ____________.", 
            choiceA = "Pigafetta", 
            choiceB = "Ferdinand Magellan", 
            choiceC = "Jesus Christ", 
            choiceD = "the King of Spain", 
            answer = 3,
            flag = false,
        },
        new question() { 
            id = 10,
            questionText = "After baptizing the sick man, Pigafetta reclaimed that the man _____________.", 
            choiceA = "became bald", 
            choiceB = "was no other than Lapulapu", 
            choiceC = "went off to become a great King", 
            choiceD = "was able to speak again", 
            answer = 4,
            flag = false,
        },
    };

    [SerializeField]
    public question[] stage4Questions = new question[]{
        new question() { 
            id = 1,
            questionText = "This man came to Magellan and asked for a boat full of men to fight the chieftain named Lapu-lapu.", 
            choiceA = "Zula", 
            choiceB = "Basilio", 
            choiceC = "Pigafetta", 
            choiceD = "Don Carlos", 
            answer = 1,
            flag = false,
        },
        new question() { 
            id = 2,
            questionText = "In reply to the request to fight Lapu-lapu, Magellan __________.", 
            choiceA = "gave the man what he requested", 
            choiceB = "tortured the native for information on the surrounding islands", 
            choiceC = "refused the request and had the man thrown off the boat", 
            choiceD = "offered 3 boats instead and expressed the desire to go to Mactan himself to fight Lapulapu", 
            answer = 4,
            flag = false,
        },
        new question() { 
            id = 3,
            questionText = "When Magellan's forces arrived in Mactan, _____________________________.", 
            choiceA = "the natives forces greatly outnumbered theirs ", 
            choiceB = "it was a swift victory for Magellan", 
            choiceC = "the battle was even for both sides", 
            choiceD = "they saw the native forces and quickly ran away, leaving Magellan behind", 
            answer = 1,
            flag = false,
        },
        new question() { 
            id = 4,
            questionText = "The captain divided his men into _______.", 
            choiceA = "five bands", 
            choiceB = "ten groups", 
            choiceC = "eight teams", 
            choiceD = "two bands", 
            answer = 4,
            flag = false,
        },
        new question() { 
            id = 5,
            questionText = "Magellan was wounded on his right leg with ____________.", 
            choiceA = "a blunt blade", 
            choiceB = "a poisoned arrow", 
            choiceC = "a bull's horn", 
            choiceD = "a sharp wooden bark splintered from one of the native's shields", 
            answer = 2,
            flag = false,
        },
        new question() { 
            id = 6,
            questionText = "Seeing that the bodies of Magellan's troops were protected with armor, the natives __________.", 
            choiceA = "gave up and ran away", 
            choiceB = "aimed for their legs instead", 
            choiceC = "threw their bodies at them", 
            choiceD = "penetrated their armor regardless with sheer brute force", 
            answer = 2,
            flag = false,
        },
        new question() { 
            id = 7,
            questionText = "The name of the battle.", 
            choiceA = "Battle of Irlington", 
            choiceB = "Battle of Palawan", 
            choiceC = "Battle of Arcane", 
            choiceD = "Battle of Mactan", 
            answer = 4,
            flag = false,
        },
        new question() { 
            id = 8,
            questionText = "After leaving Cebu, the remaining fleet _____________.", 
            choiceA = "went back to Cebu a few days later to avenge their comrades", 
            choiceB = "hit an iceberg and sunk to the depths of the Pacific Ocean", 
            choiceC = "continued their journey around the world", 
            choiceD = "messaged King Humabon for help", 
            answer = 3,
            flag = false,
        },
    };


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
        //For debugging purposes, this allows us to play the questioning scene withot going thru the first stage, delete after debugging questioning script
        // SetStage(1);
    }

    void Update(){

        //targetStage variable is used as a temporary holder to track changes (current and previous value) of the stage. 
        //Once a change of stage is detected, the PopulateQuestions() is then called for that specific stage,
        //since there are different sets of questions for each stage
        if(currentStage != targetStage) {
            currentStage = targetStage;
            nextStage = currentStage + 1;
            PopulateQuestions();
            StartCoroutine(Type());
        }

        

        if(questionDisplay.text != "" && questionDisplay.text == currentQuestions[index].questionText ){
            //======================== CHALLENGE #3 - User Interface buttons - Done
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
    }

    //======================== CHALLENGE #4 - POPULATE QUESTIONS - Done
    //Populate currentQuestions[] with questions to be displayed 
    //Questions must be unflagged (haven't been answered correctly before)
    //Calls PickUnflaggedQuestions() based on current stage
    private void PopulateQuestions() {
        EmptyCurrentQuestionsArray();   //Empty questions array prior
        CheckFlags();
        Debug.Log("Populating with Stage " + currentStage + " questions.");
        switch (currentStage)
        {
            case 1: 
                Debug.Log("CASE 1");
                PickUnflaggedQuestions(stage1Questions);
                // stage1Questions.CopyTo(currentQuestions, 0);
                break;
            case 2: 
                Debug.Log("CASE 2");
                PickUnflaggedQuestions(stage2Questions);
                break;
            case 3: 
                Debug.Log("CASE 3");
                PickUnflaggedQuestions(stage3Questions);
                break;
            case 4: 
                Debug.Log("CASE 4");
                PickUnflaggedQuestions(stage4Questions);
                break;
            
            default: 
                Debug.Log("Default case");
                break;
                
        }
    } 



    //Takes unflagged questions dataset for associated stage and populates them into the currentQuestions[] array.
    private void PickUnflaggedQuestions(question[] stageQuestions) {
        int i, randomNum = 0; 
        bool found = false;

        //STEP 1: Filter stage questions from flagged and unflagged questions. Uses the FilterUnflaggedQUestions()
        List<question> unflaggedQuestions = FilterUnflaggedQuestions(stageQuestions);

        //STEP 2: Pluck 5 questions from the filtered set.
        for(i = 0; i < maxQuestionsPerStage; i++){

            //Keeps looking until a random unique integer between 0 and unflaggedQuestions.Count  is found
            //DEV NOTE: gets more inefficient as the number of questions increases and the number of flagged questions increases
            while(!found) {
                randomNum = Random.Range(0, unflaggedQuestions.Count);
                Debug.Log("Random num value: " + randomNum);
                found = isDuplicate(unflaggedQuestions[randomNum].id);
            }

            if(found) {
                Debug.Log("Unflagged question inserted at index " + i);
                currentQuestions[i] = unflaggedQuestions[randomNum];
            } else {
                // Debug.Log("unflagged question wasnt found???");
            }
            found = false; //Reset boolean for while loop
        }
    } 

    //Returns a list of unflagged questions
    private List<question> FilterUnflaggedQuestions(question[] stageQuestions) {
        int i;
        List<question> filtered = new List<question>();  
        for(i = 0; i < stageQuestions.Length; i++) {
            if(stageQuestions[i].flag == false) {
                filtered.Add(stageQuestions[i]);
            }
        }

        Debug.Log("Returning filtered unflagged questions... count = " + filtered.Count);
        return filtered;
    }

    //If the number of unflagged questions is less than 5 (max questions per stage), flags are reset to false. 
    //This assumes that the number of questions per stage is 5 or more,
    //otherwise it does not work. Ex. if stage1Questions only has 3 questions total, 

    //Resets all flag values to false for current stage questions
    private void CheckFlags() {
        int i, x;
        Debug.Log("Checking flags");
        switch (currentStage)
        {
            case 1: 
                for(i = 0, x = 0; i < stage1Questions.Length; i++) {
                    if(stage1Questions[i].flag == false) {
                        x++;
                    }
                }
                Debug.Log("A total of " + x + " unflagged questions found.");
                if(x < 5) {
                    Debug.Log("Resetting flags.");
                    for(i = 0; i < stage1Questions.Length; i++) {
                        if(stage1Questions[i].flag == true) {
                            stage1Questions[i].flag = false;
                        }
                    }
                }
                break;
            case 2: 
                for(i = 0, x = 0; i < stage2Questions.Length; i++) {
                    if(stage2Questions[i].flag == false) {
                        x++;
                    }
                }
                Debug.Log("A total of " + x + " unflagged questions found.");
                if(x < 5) {
                    Debug.Log("Resetting flags");
                    for(i = 0; i < stage2Questions.Length; i++) {
                        if(stage2Questions[i].flag == true) {
                            stage2Questions[i].flag = false;
                        }
                    }
                }
                break;
            case 3: 
                for(i = 0, x = 0; i < stage3Questions.Length; i++) {
                    if(stage3Questions[i].flag == false) {
                        x++;
                    }
                }
                Debug.Log("A total of " + x + " unflagged questions found.");
                if(x < 5) {
                    Debug.Log("Resetting flags");
                    for(i = 0; i < stage3Questions.Length; i++) {
                        if(stage3Questions[i].flag == true) {
                            stage3Questions[i].flag = false;
                        }
                    }
                }
                break;
            case 4: 
                for(i = 0, x = 0; i < stage4Questions.Length; i++) {
                    if(stage4Questions[i].flag == false) {
                        x++;
                    }
                }
                Debug.Log("A total of " + x + " unflagged questions found.");
                if(x < 5) {
                    Debug.Log("Resetting flags");
                    for(i = 0; i < stage4Questions.Length; i++) {
                        if(stage4Questions[i].flag == true) {
                            stage4Questions[i].flag = false;
                        }
                    }
                }
                break;
            
            default: 
                Debug.Log("Default case");
                break;
                
        }
    }

    //Checks if question is already inside the currentQuestions[] using the question ID
    private bool isDuplicate(int quid){
        bool ret = false;
        for(int i = 0; !ret && i < currentQuestions.Length; i++) {
            if(quid == currentQuestions[i].id) {
                ret = true;
            }
        }

        return !ret;
    }

    //Function to display question text
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
            //======================== CHALLENGE #4 - Evaluation and Calculation - Done
            //On the last question, check if score is the minimum required value to pass.
            //If yes, show total score and proceed to next stage
            //Else, Retry and redo level, flag correctly answered questions.
            DisplayResults();
        }
    }

    //Checks if answer was correct
    private void CheckAnswer(int val) {
        //If correct, qScore++
        //Crosschecks using the currentQuestions[]
        Debug.Log("Checking answer for stage " + currentStage + " questions.");

        if(val == currentQuestions[index].answer) {
            qScore++;
            Debug.Log("CORRECT! You answered: " + val + ". Correct answer: " + currentQuestions[index].answer + " qScore: " + qScore);
            UpdateQScore();
            UpdateQuestionFlag(currentQuestions[index].id);
        } else {
            Debug.Log("Wrong! You answered: " + val + ". Correct answer: " + currentQuestions[index].answer + " qScore: " + qScore);
        }
    }

    public void UpdateQScore(){
        qScoreDisplay.text = "Question Score: " + qScore.ToString() + "/5";
    }

    //Using the question ID and current stage, updates original question set. 
    //Sets flag value for the associated question to true.
    private void UpdateQuestionFlag(int quid) {
        Debug.Log("Updating flag value for Question " + quid + " at Stage " + currentStage + " .");
        switch (currentStage)
        {
            case 1: 
                Debug.Log("Case 1");
                stage1Questions[quid - 1].flag = true;
                break;
            
            default: 
                Debug.Log("Default case, nothing happened");
                break;
                
        }
    }

    private void EmptyCurrentQuestionsArray() {
        Array.Clear(currentQuestions, 0, currentQuestions.Length);
    }


    private void DisplayResults() {
        if(qScore >= passingScore) {
            didPass = true;
            ScoreManager.calculateTotalScore();
            questionDisplay.text =  "Alab Points: " + ScoreManager.getTotalScore().ToString() + 
                                    "\nEnemy Points: " + ScoreManager.getEnemyScore().ToString() +
                                    "\nLife Points: " + ScoreManager.getLifeScore().ToString() +
                                    "\nStage Completion Points: " + ScoreManager.getTotalTries().ToString() + 
                                    "\nTotal Score: " + ScoreManager.getTotalScore().ToString() +
                                    "\nYour questioning score is: " + qScore.ToString() + "/5. \nPassed!";
            PlayerPrefs.SetInt("Highscore" + targetStage.ToString(), ScoreManager.getTotalScore());
            // Debug.Log("THE HIGHSCOOOOOORE HAS BEEN RECORDED");
            // Debug.Log(PlayerPrefs.GetInt("Highscore1", 0));


            //Display Proceed to Next Stage button
        } else {
            questionDisplay.text = "\nYour questioning score is: " + qScore.ToString() + "/5. \nTry again!";
            //Display Retry button
        }
        
        if(didPass) {
            //Displaysbutton to proceed to next stage
            resultsPass.SetActive(true);
            // didPass = false;
        } else {
            //Displays try again button
            resultsFail.SetActive(true);    
        }
    }

    public static void SetStage(int stageVal) {
        targetStage = stageVal;
        Debug.Log("Set target stage val to : " + stageVal);
    } 

    public void Proceed() {
        if(currentStage != 4) {
            Debug.Log("Proceeding to next Stage.. " + nextStage);
            SceneManager.LoadScene("Stage " + nextStage);
        } else {
            Debug.Log("Thank you for playing the game!");
        }
    } 

    public void TryAgain() {
        //Reset values and stage before questioning.
        currentStage--;
        nextStage--;
        SetStage(currentStage);
        SceneManager.LoadScene("Stage " + nextStage);
        
    } 

    private void ToggleActive(bool val) {
        ChoiceA.SetActive(val);
        ChoiceB.SetActive(val);
        ChoiceC.SetActive(val);
        ChoiceD.SetActive(val);
    }

}
