using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public static event Action<List<InventoryItem>> OnInventoryChange;

    public List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    private const int requiredItemCount = 7; // Define the required item count

    private void OnEnable()
    {
        Gem.OnGemCollected += Add;
    }

    private void OnDisable()
    {
        Gem.OnGemCollected -= Add;
    }

    public void Add(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.AddtoStack();
            Debug.Log($"{item.itemData.displayName} total stack is now {item.stackSize}.");
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
            Debug.Log($"Added {itemData.displayName} to the inventory for the first time.");
        }

        OnInventoryChange?.Invoke(inventory);

        int totalItemCount = CalculateTotalItemCount();
        Debug.Log($"Current Inventory Count: {totalItemCount}, Required Item Count: {requiredItemCount}");

        // Check if required item count is reached
        if (totalItemCount >= requiredItemCount)
        {
            Debug.Log("Required item count reached! Showing win screen.");
            WinManager.Instance.ShowWinScreen();
        }
    }

    private int CalculateTotalItemCount()
    {
        int totalCount = 0;
        foreach (var item in inventory)
        {
            totalCount += item.stackSize;
        }
        return totalCount;
    }

    public void Remove(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.RemoveFromStack();
            if (item.stackSize == 0)
            {
                inventory.Remove(item);
                itemDictionary.Remove(itemData);
            }
            OnInventoryChange?.Invoke(inventory);
        }
    }
}
