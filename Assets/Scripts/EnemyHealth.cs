using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 15; // Maximum health of the enemy
    private int currentHealth; // Current health of the enemy

    void Start()
    {
        currentHealth = maxHealth; // Set current health to maximum health when the enemy spawns
    }

    // Method to reduce the enemy's health
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce current health by the amount of damage

        // Check if the enemy is dead
        if (currentHealth <= 0)
        {
            Die(); // Call the Die method if the enemy's health reaches or falls below zero
        }
    }

    // Method to handle enemy death
    void Die()
    {
        // Perform any death-related actions here, such as playing death animations, spawning particle effects, etc.
        Destroy(gameObject); // Destroy the enemy GameObject when it dies
    }
}
