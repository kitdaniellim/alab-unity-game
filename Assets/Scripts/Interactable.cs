using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    //Private Variables
    private float           distance;
    
    //Public References
    public float            radius = 3f;
    public Transform        player;
    
    

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);
        if(distance <= radius) {
            //Pop up trigger
            transform.GetChild(0).gameObject.SetActive(true);
        } else {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public virtual void Interact() {
        //This function is meant to be overridden
        Debug.Log("Interacting with " + transform.name);
        //Hide the Pop up after pressing "E"
        transform.GetChild(0).gameObject.SetActive(false);
    }


    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
