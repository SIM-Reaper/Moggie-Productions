using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    
    //Script to determine how much damage enemy deals to player
    [SerializeField] float health, maxHealth = 100f;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount; // 3 -> 2 -> 1 -> 0 = Enemy Has Died

        animator.SetTrigger("Hurt");

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        animator.SetBool("isDead", true);

        GetComponent<LootBag>().InstantiateLoot(transform.position);

        GetComponent<Collider>().enabled = false;
        this.enabled = false;
        
        Destroy(gameObject);
        
    }
}
