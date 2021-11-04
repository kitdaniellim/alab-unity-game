using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alab : MonoBehaviour
{
   [SerializeField] 
   private int alabValue = 1;

   private void OnTriggerEnter2D(Collider2D other){
       if(other.gameObject.CompareTag("Player")){
           ScoreManager.instance.updateAlabScore(alabValue);
       }
   }
}
