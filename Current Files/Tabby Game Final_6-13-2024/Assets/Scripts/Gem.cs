using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour, ICollectible
{
    public static event System.Action<ItemData> OnGemCollected;
    public ItemData gemData;

    public AudioClip pickupSound;

    void Update () 
    {

		transform.RotateAround( transform.position, Vector3.up, 100*Time.deltaTime);
	
	}

    public void Collect()
    {
        Debug.Log($"Collecting gem: {gameObject.name}");
        if (gemData != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            OnGemCollected?.Invoke(gemData);
        }
        Destroy(gameObject); // Ensure that the GameObject is destroyed after collecting the gem
    }

}
