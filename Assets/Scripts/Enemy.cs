using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private bool                isBehaviourChanged = false;
    private int                 aggressionRange = 1;


    //Public References
    public Animator             anim;
    public HealthBar            healthBar;

    //Enemy Stats
    int                         currentHealth;
    public int                  maxHealth = 100;
    public Transform            attackPoint;
    public float                attackRange = 0.5f;
    public float                attackRate = 1f;
    public int                  attackDamage = 1;
    float                       nextAttackTime = 0f;
    public LayerMask            playerLayers;

    //Position Reference
    private Vector2             _lastPosition;
    private GameObject          player;

    //Audio
    public AudioSource          slashSound;
    public AudioSource          hurtSound;
    public AudioSource          deathSound;

    // Start is called before the first frame update
    void Start()
    {
        _lastPosition = transform.position;
        currentHealth = maxHealth;
        healthBar.setHealth(currentHealth, maxHealth);
        // player = GameObject.FindWithTag("Player");
        player = GameObject.Find("Player");
    }

    private void Update() {
        // if(System.Math.Round(_lastPosition.x, 6) == System.Math.Round(transform.position.x, 6)) {
        //     anim.SetBool("IsRunning", false);
        // } else {
        //     anim.SetBool("IsRunning", true);
        // }

        if(_lastPosition.x == transform.position.x) {
            anim.SetBool("IsRunning", false);
        } else {
            anim.SetBool("IsRunning", true);
        }

        _lastPosition = transform.position;
        // Debug.Log(player.transform.position);
        // Debug.Log(player.transform.position.x);
        // Debug.Log(transform.position.x);
        // Debug.Log(Mathf.Abs(transform.position.x - player.transform.position.x));
        if(Mathf.Abs(transform.position.x - player.transform.position.x) < aggressionRange ){
            // Debug.Log("HEY, PLAYER IS NEAR!!");
            if(!isBehaviourChanged){
                Debug.Log("changing behaviour");
                ChangeBehaviour();
            }
        }
    }

    public void TakeDamage(int damage){
        //Hurt animation
        currentHealth -= damage;
        healthBar.setHealth(currentHealth, maxHealth);
         
        anim.SetTrigger("Hurt");
        
        if(currentHealth <= 0){
            Die();
        } else {
            hurtSound.Play();
        }
    }

    private void ChangeBehaviour(){
        Debug.Log("behaviour changed ");
        GetComponent<Patrolling>().enabled = false;
        GetComponent<Following>().enabled = true;
        isBehaviourChanged = true;
    }


    private void Die()
    {
        Debug.Log("Enemy died!");
        disableEnemy();

        //Die animation
        deathSound.Play();
        anim.SetBool("IsDead", true);

        
    }

    public void Attack() {
        if(Time.time >= nextAttackTime){
            slashSound.Play();
            anim.SetTrigger("Attack");
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
            foreach(Collider2D hit in hitPlayers) {
                Debug.Log("You got hit!");
                player.GetComponent<Player>().TakeDamage(attackDamage);
            }
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    private void OnDrawGizmosSelected() {
        if(attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    } 

    private void disableEnemy() {
        Destroy(GetComponent<Rigidbody2D>());
        GetComponent<Following>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    // private void OnCollisionStay2D(Collision2D other) {
    //     if(other.gameObject.CompareTag("Player")){
    //         anim.SetBool("IsRunning", false);
    //         Attack();
    //         if(!isBehaviourChanged){
    //             ChangeBehaviour();
    //         }
    //     }
    // }

    // private void OnCollisionExit2D(Collision2D other)
    // {
    //     if(other.gameObject.CompareTag("Player")){
    //         anim.SetBool("IsRunning", true);
    //     }
    // }

    // private void OnCollisionExit2D(Collision2D other)
    // {
    //     if(other.gameObject.CompareTag("Player")){
    //         anim.SetBool("IsRunning", true);
    //     }
    // }

    // private void OnCollisionStay(Collision other)
    // {
    //     if(other.gameObject.CompareTag("Player")){
    //         anim.SetTrigger("Attack");
    //     }
    // }
}
