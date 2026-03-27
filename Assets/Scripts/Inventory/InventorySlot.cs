using System;

[Serializable]
public class InventorySlot
{
    public InventoryItemData item;
    public int quantity;

    public bool IsEmpty
    {
        get { return item == null || quantity <= 0; }
    }

    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}
