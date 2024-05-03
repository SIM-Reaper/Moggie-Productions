using UnityEngine;

public class HazardItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().ResetProgress();
        }
    }
    

}