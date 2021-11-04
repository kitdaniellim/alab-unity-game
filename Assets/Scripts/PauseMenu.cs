using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Functions simply call Game Master functions
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(GameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }
        
    }

    public void Resume () {
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        GameMaster.ResumeGame();
    }

    void Pause() {
        GameIsPaused = true;
        pauseMenuUI.SetActive(true);
        GameMaster.PauseGame();
    }

    public void LoadMenu() {
        Resume(); //Not sure if right to resume
        GameMaster.ReturnToMainMenu();
    }

    public void Quit() {
        GameMaster.QuitGame();
    }
}
