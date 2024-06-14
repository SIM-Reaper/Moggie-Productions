using System;

[Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int stackSize;

    public InventoryItem(ItemData item)
    {
        itemData = item;
        AddtoStack();
    }

    public void AddtoStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }
}