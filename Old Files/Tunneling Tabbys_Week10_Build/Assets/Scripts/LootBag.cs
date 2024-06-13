using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();

    List<Loot> GetDroppedItems()
    {
        float randomNumber = Random.Range(1, 101); // 1-100
        List<Loot> possibleItems = new List<Loot>();
        foreach (Loot item in lootList)
        {
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
                return possibleItems;
            }
        }
        Debug.Log("No loot dropped");
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        List<Loot> droppedItems = GetDroppedItems();
        if (droppedItems != null && droppedItems.Count > 0)
        {
            foreach (Loot droppedItem in droppedItems)
            {
                GameObject lootGameObject = Instantiate(droppedItem.lootObject, spawnPosition, Quaternion.identity);

                // Apply force to the loot object
                Rigidbody rb = lootGameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    float dropForce = 300f;
                    Vector3 dropDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                    rb.AddForce(dropDirection * dropForce, ForceMode.Impulse);
                }
                
                // Optionally set the Rigidbody to be kinematic to stop further movement
                StartCoroutine(MakeKinematicAfterDelay(rb, 0.5f));
            }
        }
    }

    private IEnumerator MakeKinematicAfterDelay(Rigidbody rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }
}
