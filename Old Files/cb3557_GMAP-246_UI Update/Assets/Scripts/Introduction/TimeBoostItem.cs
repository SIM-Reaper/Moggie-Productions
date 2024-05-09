using System.Linq;
using UnityEngine;

public class TimeBoostItem : MonoBehaviour
{
    public float timeBoost = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().timeLeft += timeBoost;
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
    
    public static TimeBoostItem[] GetAllInactiveTimeItems()
    {
        return Resources.FindObjectsOfTypeAll<TimeBoostItem>().Where(p => !p.gameObject.activeInHierarchy).ToArray();
    }
}