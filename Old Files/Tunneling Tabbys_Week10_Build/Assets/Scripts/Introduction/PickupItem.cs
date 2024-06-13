using System.Linq;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public int pointsValue = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().AddScore(pointsValue);
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    public static PickupItem[] GetAllInactivePickupItems()
    {
        return Resources.FindObjectsOfTypeAll<PickupItem>().Where(p => !p.gameObject.activeInHierarchy).ToArray();
    }
}