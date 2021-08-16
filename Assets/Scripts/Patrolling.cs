using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolling : MonoBehaviour
{
    public float            speed;
    public float            distance;
    public Transform        groundDetection;

    private float           temp;
    private bool            movingRight = true;

    //IgnoreCameraConfiner
    private int             layerMask = 3; 

    private void Start() {
        temp = speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance, layerMask);

        if(groundInfo.collider == false || groundInfo.collider.name == "Crate"){
            if(movingRight == true){
                transform.Rotate(new Vector3(0, 180, 0));
                movingRight = false;
            } else {
                transform.Rotate(new Vector3(0, 180, 0));
                movingRight = true;
            }
        }
    }
}
