using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Respawn Delay
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

    public static void ResetGame () {
        gm.StartCoroutine(gm.RespawnPlayer());
        // gm.RespawnPlayer();
    }

    public static void GameOver () {
        // Destroy(player.gameObject);
        // gm.RespawnPlayer();
    }

}
