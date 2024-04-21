using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class MoveDog : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    public Transform cam;

    public float speed = 6;
    public float gravity = -9.81f;
    public float jumpHeight = 1;
    Vector3 velocity;
    bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    public float attackRange = 2f;

    public float cooldown = 1f; //seconds
    private float lastAttackedAt = -9999f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        
        //jump
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (animator.GetBool("Death") == true && controller.isGrounded)
        {

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            return;
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            // UnityEngine.Debug.Log("Jumped!!!");
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        //walk
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        

        //Attacking Animation
        if (Input.GetButtonDown("Fire1") && controller.isGrounded)
        {
            animator.SetBool("Attack", true);
            
            if (Time.time > lastAttackedAt + cooldown)
            {
                //do the attack
                AttackEnemy();
                AttackCrate();
                lastAttackedAt = Time.time;
            }
        }
        else
        {
            animator.SetBool("Attack", false);
        }

        //Defense Animation
        if (Input.GetButtonDown("Fire2") && controller.isGrounded)
        {
            animator.SetBool("Defend", true);
        }
        else
        {
            animator.SetBool("Defend", false);
        }

        //Walking animation
        if (direction == Vector3.zero)
        {
            //Idle
            animator.SetFloat("Speed", 0);
        }
        else
        {
            animator.SetFloat("Speed", 2f);
        }


        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        
    }

    void AttackEnemy()
    {
        // Find all objects with the "enemy" tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // Get the position of the player
        Vector3 playerPosition = transform.position;

        // Loop through each enemy and apply damage if within range
        foreach (GameObject enemy in enemies)
        {
            // Calculate the distance between the player and the enemy
            float distanceToEnemy = Vector3.Distance(playerPosition, enemy.transform.position);
            UnityEngine.Debug.Log("Distance " + distanceToEnemy);
            // Check if the enemy is within attack range
            if (distanceToEnemy <= attackRange)
            {
                //UnityEngine.Debug.Log("Yes");
                // Get the EnemyHealth component from the enemy
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

                // If the enemy has EnemyHealth component, deduct health
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(5); // Reduce 5 health
                }
            }

        }
    }

    void AttackCrate(){
        //UnityEngine.Debug.Log("In Attack crate");
        float crateZone = 2.0f; //area surrounding crate where dog can break the crate
        
        //Similar to attackEnemy(), creating tag to handle multiple crate objects
        GameObject[] crates = GameObject.FindGameObjectsWithTag("Crate"); 
        foreach (GameObject crate in crates){
            float distanceToCrate = Vector3.Distance(transform.position, crate.transform.position);
             //calculating distance so we can destroy the crate when we are in the zone.
            if (distanceToCrate <= crateZone){
                ItemReplace replaceScript = crate.GetComponent<ItemReplace>(); //Calling the ItemReplace.cs script
                if(replaceScript!=null){
                    replaceScript.ReplaceBox();
                    //deleteItem();
                }
            }
        }
    }

    public void deleteItem(){
         float itemZone = 1.0f; //area surrounding crate where dog can break the crate
        
        //Similar to attackEnemy(), creating tag to handle multiple crate objects
            UnityEngine.Debug.Log("deleteItem");
            

            GameObject[] crates = GameObject.FindGameObjectsWithTag("Crate"); 
            foreach (GameObject crate in crates){
                UnityEngine.Debug.Log(crate);
                float distanceToItem = Vector3.Distance(transform.position, crate.transform.position);
             //calculating distance so we can destroy the crate when we are in the zone.
                if (distanceToItem <= itemZone){
                    UnityEngine.Debug.Log("in delete item range");
                    Destroy(crate);
                }
        }
        
    }

    
}
