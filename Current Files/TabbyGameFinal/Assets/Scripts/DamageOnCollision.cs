using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    public float collisionDamage = 20;

    // Use OnTriggerEnter instead of OnCollisionEnter
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(collisionDamage);
        }
    }
}
