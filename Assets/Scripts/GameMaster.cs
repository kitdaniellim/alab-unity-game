using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;
    // public Transform playerPrefab;
    public Transform playerReference;
    public Transform spawnPoint;
    public float spawnDelay = 1.5f;
    

    // Start is called before the first frame update
    void Start()
    {  
        if(gm == null) {
            // gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
            gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        }
    }

    public IEnumerator RespawnPlayer() {

        yield return new WaitForSeconds(spawnDelay);
        //Reposition player to respawn
        playerReference.transform.position = spawnPoint.position;
    }

    // public void RespawnPlayer() {
        //Creating a clone prefab instance
        // Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);

        //Reposition player to respawn
        // playerReference.transform.position = spawnPoint.position;
    // }

    public static void RestartGame () {
        gm.StartCoroutine(gm.RespawnPlayer());
        // gm.RespawnPlayer();
    }

    public static void ResumeGame () {
        Debug.Log("Game resumed.");
        Time.timeScale = 1f;
    }

    public static void PauseGame () {
        Debug.Log("Game paused.");
        Time.timeScale = 0f;
    }

    public static void SaveGame () {
        Debug.Log("Game saved.");
       //Insert saving logic
    }

    public static void QuitGame () {
        Debug.Log("Quitting game.");
        Application.Quit();
    }

    public static void ReturnToMainMenu () {
        Debug.Log("Returning to main menu.");
        SceneManager.LoadScene("MainMenu");
        AudioManager.StopMusic();
    }

    public static void EndGame () {
        //Ends game and restarts current stage
        Debug.Log("GAME OVER - restarting whole game");
        ScoreManager.updateTries();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void ProceedToQuestioning (int currentStage) {
        //Ends game and restarts current stage
        Debug.Log("Questioning Time!");
        SceneManager.LoadScene("Questioning");
        AdaptiveQManager.SetStage(currentStage);
    }


}
