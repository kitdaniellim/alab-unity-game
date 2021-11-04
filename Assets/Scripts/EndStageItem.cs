using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStageItem : Interactable
{


    public override void Interact() {
        base.Interact();
        EndStage();
    }  

    private void EndStage() {


        //Show question screen
        // call AdaptiveQManager functions

        //Sample
        Debug.Log("Ending the stage ");
        GameMaster.ProceedToQuestioning(1);
    }
}
