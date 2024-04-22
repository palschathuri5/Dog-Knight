using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Animator animator;
    public float attackRange = 2f;
    private float update;
    void Awake()
    {
        update = 0.0f;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        yield return new WaitForSeconds(5f);
    }



    // Update is called once per frame
    void Update()
    {
        // Attack every 5 seconds
        update += Time.deltaTime;
        if(update > 5.0f)
        {
            update = 0.0f;
            
            AttackPlayer();
        }
        else
        {
            animator.SetBool("Attack", false);
        }
            


    }

    void AttackPlayer()
    {
        // Find all objects with the "player" tag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject player = players[0];
        // Get the position of enemy
        Vector3 playerPosition = transform.position;


        // Calculate the distance between the player and the enemy
        float distanceToPlayer = Vector3.Distance(playerPosition, player.transform.position);
        UnityEngine.Debug.Log("Distance " + distanceToPlayer);
        // Check if the enemy is within attack range
        if (distanceToPlayer <= attackRange)
        {

            DogStats dogStats = player.GetComponent<DogStats>();
            animator.SetBool("Attack", true);


            if (dogStats != null)
            {
                dogStats.TakeDamage();
                UnityEngine.Debug.Log("Enemy hits Player");
            }
        }


    }
}
