using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : MonoBehaviour
{
  	public AudioSource hopSound;

	void Update()
	{
		if(Input.GetKey(KeyCode.Space))		{
		
			hopSound.enabled = true;
		}
		else
		{
			hopSound.enabled = false;
		}
	}
}
