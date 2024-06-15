using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    
    // Script to determine how much damage enemy deals to player
    [SerializeField] float health, maxHealth = 100f;

    // Optional delay for death animation
    [SerializeField] float deathDelay = 2f;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount; // 3 -> 2 -> 1 -> 0 = Enemy Has Died

        animator.SetTrigger("Hurt");

        if (health <= 0)
        {
            StartCoroutine(Die()); // Start the coroutine to handle death
        }
    }

    IEnumerator Die()
    {
        Debug.Log("Enemy died!");

        animator.SetBool("isDead", true);

        GetComponent<Collider>().enabled = false;
        this.enabled = false;

        // Wait for the specified death delay before destroying the game object
        yield return new WaitForSeconds(deathDelay);

        Destroy(gameObject);
    }
}
