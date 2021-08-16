using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_old : MonoBehaviour
{
    [SerializeField]
    private Transform groundCheckTransform = null;

    [SerializeField]
    private LayerMask playerMask;

    private bool jumpKeyWasPressed;

    private float horizontalInput;

    private Rigidbody2D rigidbodyComponent;

    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyWasPressed = true;
            Console
                .WriteLine(Physics2D
                    .OverlapCircleAll(groundCheckTransform.position,
                    0.1f,
                    playerMask)
                    .Length);
        }
        horizontalInput = Input.GetAxis("Horizontal");
    }

    // FixedUpdate is called once every physics update
    private void FixedUpdate()
    {
        rigidbodyComponent.velocity =
            new Vector2(horizontalInput * 18, rigidbodyComponent.velocity.y);

        if (
            Physics2D
                .OverlapCircleAll(groundCheckTransform.position,
                0.12f,
                playerMask)
                .Length ==
            0
        )
        {
            return;
        }

        if (jumpKeyWasPressed)
        {
            rigidbodyComponent.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
            jumpKeyWasPressed = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Alab")){
            Destroy(other.gameObject);
        }
    }
}
