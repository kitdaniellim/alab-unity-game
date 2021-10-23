using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    //Private Variables
    private Animator                m_animator;
    private Rigidbody2D             m_body2d;
    private Sensor_Player           m_groundSensor;
    private bool                    m_grounded = false;
    // private bool                    m_combatIdle = false;
    public bool                     m_isDead = false;
    private bool                    m_outOfBounds = false;
    private bool                    m_movementDisabled = false;
    private float                   nextAttackTime = 0f;
    private int                     currentHealth;
    private bool                    canInteract = false;
    private GameObject              interactableObj;

    //Player Stats
    [SerializeField] int            maxHearts = 3;
    [SerializeField] float          m_speed = 4.0f;
    [SerializeField] float          m_jumpForce = 12f;
    [SerializeField] Transform      attackPoint;
    [SerializeField] float          attackRange = 0.5f;
    [SerializeField] float          attackRate = 2f;
    [SerializeField] int            attackDamage = 40;
    [SerializeField] int            fallBoundary = 300;
    [SerializeField] LayerMask      enemyLayers;

    //Audio
    public AudioSource              slashSound;
    public AudioSource              hurtSound;
    public AudioSource              jumpSound;
    public AudioSource              deathSound;

    //Images
    public Image[]                  hearts;
    public Sprite                   fullHeart;
    public Sprite                   emptyHeart;


    #region Singleton
    public static Player instance;

    void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of player found!");
            return;
        }
        instance = this;    
    }
    #endregion

    // Use this for initialization
    void Start () {
        currentHealth = maxHearts;
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Player>();
        
        //Default Combat Idle Animation
        m_animator.SetInteger("AnimState", 1);

        Physics2D.IgnoreLayerCollision(6, 10, true);
        Physics2D.IgnoreLayerCollision(6, 11, true);
    }
	
	// Update is called once per frame
	void Update () {

        //Hearts UI
        for(int x = 0; x < hearts.Length; x++) {
            if(x < currentHealth) {
                hearts[x].sprite = fullHeart;
            } else {
                hearts[x].sprite = emptyHeart;
            }

            if(x < maxHearts) {
                hearts[x].enabled = true;
            } else {
                hearts[x].enabled = false;
            }
        } 

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if(m_grounded && !m_groundSensor.State()) {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        //Player Out-of-Bounds
        if(transform.position.y <= fallBoundary){
            if(!m_outOfBounds) {
                Debug.Log("You fell!");
                m_outOfBounds = true;
                TakeDamage(1);
            }
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        if(!m_movementDisabled) {
            // Swap direction of sprite depending on walk direction
            if (inputX > 0)
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            else if (inputX < 0)
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            
            // Move
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

            

            

            // -- Handle Animations --
            //Death
            // if (Input.GetKeyDown("e")) {
            //     if(!m_isDead)
            //         m_animator.SetTrigger("Death");
            //     else
            //         m_animator.SetTrigger("Recover");

            //     m_isDead = !m_isDead;
            // }

            
                
            //Hurt
            // else if (Input.GetKeyDown("q"))
            //     m_animator.SetTrigger("Hurt");

            // if (Input.GetKeyDown("q"))
            //     m_animator.SetTrigger("Hurt");
            
            //Attack
            if(Input.GetMouseButtonDown(0)) {
                if(Time.time >= nextAttackTime){
                    slashSound.Play();
                    m_animator.SetTrigger("Attack");
                    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

                    foreach(Collider2D enemy in hitEnemies) {
                        Debug.Log("We hit " + enemy.name);
                        enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
                    }
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }

            //Change between idle and combat idle
            // else if (Input.GetKeyDown("f"))
            //     m_combatIdle = !m_combatIdle;

            //Interact with Object
            else if (Input.GetKeyDown("e") && canInteract) {
                interactableObj.GetComponent<Interactable>().Interact();
            }

            //Jump
            else if (Input.GetKeyDown("space") && m_grounded) {
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
            }

            //Run
            else if (Mathf.Abs(inputX) > Mathf.Epsilon)
                m_animator.SetInteger("AnimState", 2);

            //Combat Idle
            // else if (m_combatIdle)
            //     m_animator.SetInteger("AnimState", 1);

            //Idle
            else
                m_animator.SetInteger("AnimState", 0);
        }
    }

    public void TakeDamage(int damage) {
        if(!m_isDead) {
            currentHealth -= damage; 
            if(currentHealth <= 0){
                Die();
            } else {
                //Hurt animation
                hurtSound.Play();
                if(m_outOfBounds) {
                    m_movementDisabled = true;
                    GameMaster.ResetGame();
                    StartCoroutine(resetState());
                } else {
                    m_animator.SetTrigger("Hurt");
                }
            }
        }
    }

    private void Die() {
        Debug.Log("GAME OVER");

        //Die animation
        deathSound.Play();
        m_animator.SetTrigger("Death");
        m_isDead = true;
        m_movementDisabled = true;


        //Game Over TODO then reset game after clicking retry
    }

    private void Retry() {
        currentHealth = maxHearts;
        GameMaster.ResetGame();
        StartCoroutine(resetState());
    }

    private IEnumerator resetState() {
        yield return new WaitForSeconds(2f);
        Debug.Log ("Recovering...");
        m_animator.SetTrigger("Death");
        m_animator.SetTrigger("Recover");
        
        
        yield return new WaitForSeconds(2f);
        Debug.Log ("Movement restored.");
        m_movementDisabled = false;
        m_outOfBounds = false;
        m_isDead = false;
    }

    private void OnDrawGizmosSelected() {
        if(attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    } 

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Alab")){
            Destroy(other.gameObject);
        }

        if(other.gameObject.CompareTag("AttackPoint")){
            m_animator.SetTrigger("Hurt");
        }
        if(other.gameObject.CompareTag("Interactable")){
            canInteract = true;
            interactableObj = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Interactable")){
            canInteract = false;
            interactableObj = null;
        }
    }

    public void toggleMovement() {
        m_movementDisabled = !m_movementDisabled;
    }

}
