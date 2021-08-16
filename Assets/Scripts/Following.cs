using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : MonoBehaviour
{

    //Player Reference
    public Transform        player;

    //RigidBody2D Reference
    private Rigidbody2D     rb;

    //Movement Reference
    private Vector2         movement;
    public float            moveSpeed = 2f;
    private bool            movingRight;
    private int             spaceRange = 1;

    // Start is called before the first frame update
    void Start()
    {   
        movingRight = transform.localRotation.y >= 0 ? true : false;
        // Debug.Log(transform.localRotation.y);
        // Debug.Log(movingRight);
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = player.position - transform.position;
        movement = direction;
    }

    private void FixedUpdate() {
        moveCharacter(movement);    
    }

    void moveCharacter(Vector2 direction){
        // Debug.Log(Mathf.Abs(transform.position.y - player.position.y));
        if(direction.x > 1 || direction.x < -1) {
            rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        } else if(Mathf.Abs(transform.position.y - player.position.y) < spaceRange) {
            GetComponent<Enemy>().Attack();
        }

        // Swap direction of sprite depending on walk direction
        if (direction.x > 0 && !movingRight) {
            transform.Rotate(new Vector3(0, 180, 0));
            movingRight = true;
        }
        else if (direction.x < 0 && movingRight){
            transform.Rotate(new Vector3(0, 180, 0));
            movingRight = false;
        }
    }
}
