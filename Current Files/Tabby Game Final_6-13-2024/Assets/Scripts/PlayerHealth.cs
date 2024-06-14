using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action OnPlayerDamaged;
    public event Action OnPlayerDeath;
    
    public float maxHealth = 3;
    public float health;

    public Animator anim;

    public void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        OnPlayerDamaged?.Invoke();

        if (health <= 0)
        {
            health = 0;
            Debug.Log("You're dead");
            anim.SetBool("isDead", true);
            OnPlayerDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        health += amount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
}
