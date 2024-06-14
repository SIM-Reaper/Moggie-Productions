using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropper : MonoBehaviour
{
    [System.Serializable]
    public class LootItem
    {
        public GameObject itemPrefab; // Reference to the item prefab
        [Range(0f, 1f)]
        public float dropChance;
    }

    public List<LootItem> lootTable = new List<LootItem>();
    public Transform dropPosition;
    public Vector3 lootScale = new Vector3(0.5f, 0.5f, 0.5f); // Adjust the scale as needed
    public float collectDelay = 0.5f; // Delay before loot can be collected

    public void DropLoot()
    {
        foreach (var loot in lootTable)
        {
            if (Random.value <= loot.dropChance)
            {
                GameObject lootObject = Instantiate(loot.itemPrefab, dropPosition.position, Quaternion.identity);
                lootObject.transform.localScale = lootScale;

                // Ensure the Loot script is attached and initialized
                var lootScript = lootObject.GetComponent<Loot>();
                if (lootScript != null)
                {
                    lootScript.Initialize(lootObject.GetComponent<Gem>().gemData, lootScale, collectDelay);
                }
                else
                {
                    Debug.LogWarning("Loot script not found on the instantiated prefab.");
                }
            }
            
        }
    }

    private void OnDestroy()
    {
        DropLoot();
    }
}
