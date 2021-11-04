using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI alabText;
    [SerializeField]
    private float timeStart;
    [SerializeField]
    private TextMeshProUGUI timeText;

    private static int alabScore;
    private static int enemyScore;
    private static int lifeScore = 3;
    private static int totalTries;
    private static int totalScore;

    #region Singleton
    public static ScoreManager instance;

    void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of the score manager found!");
            return;
        }
        instance = this;    
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        timeText.text = timeStart.ToString();
        if(instance == null) {
            instance = this;
        }
    }

    void Update()
    {
        timeStart += Time.deltaTime;
        timeText.text = Mathf.Round(timeStart).ToString();
    }

    public void updateAlabScore(int coinValue){
        alabScore += coinValue;
        alabText.text = "X" + alabScore.ToString();
    }

    public static void updateEnemyScore () {
        enemyScore++;
        Debug.Log("Current enemy score: " + enemyScore);
    }

    public static void updateLifeScore () {
        lifeScore--;
        if(lifeScore == 0){
            lifeScore = 3;
        }
        Debug.Log("Current life score: " + lifeScore);
    }

    public static void updateTries () {
        totalTries++;
        Debug.Log("Current number of tries: " + totalTries);
    }

    public static void calculateTotalScore () {
        alabScore *= 20;
        enemyScore *= 50;
        lifeScore *= 200;
        totalTries = 2000 - (totalTries <= 5 ? totalTries*100 : 5*100);
        totalScore = alabScore + enemyScore + lifeScore + totalTries;
    }

    public static int getAlabScore () {
        return alabScore;
    }

    public static int getEnemyScore () {
        return enemyScore;
    }

    public static int getLifeScore () {
        return lifeScore;
    }

    public static int getTotalTries () {
        return totalTries;
    }   

    public static int getTotalScore () {
        return totalScore;
    }



}
