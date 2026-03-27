using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int hotbarSize = 8;
    [SerializeField] private int inventoryRows = 3;
    [SerializeField] private int inventoryColumns = 8;

    private readonly List<InventorySlot> slots = new List<InventorySlot>();
    private InventoryUI inventoryUI;

    public event Action InventoryChanged;
    public event Action<int> SelectedSlotChanged;

    public IReadOnlyList<InventorySlot> Slots
    {
        get { return slots; }
    }

    public int HotbarSize
    {
        get { return hotbarSize; }
    }

    public int TotalSlotCount
    {
        get { return hotbarSize + (inventoryRows * inventoryColumns); }
    }

    public bool IsInventoryOpen { get; private set; }
    public int SelectedSlotIndex { get; private set; }


    private void Awake()
    {
        EnsureSlotCapacity();

        inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI == null)
        {
            GameObject inventoryUiObject = new GameObject("RuntimeInventoryUI");
            inventoryUI = inventoryUiObject.AddComponent<InventoryUI>();
        }

        inventoryUI.Initialize(this);
        SetSelectedSlot(0);
    }

    public bool AddItem(InventoryItemData itemData, int quantity = 1)
    {
        if (itemData == null || quantity <= 0)
        {
            return false;
        }

        int remaining = quantity;

        for (int i = 0; i < slots.Count; i++)
        {
            InventorySlot slot = slots[i];

            if (slot.item != itemData || slot.quantity >= itemData.maxStackSize)
            {
                continue;
            }

            int spaceLeft = itemData.maxStackSize - slot.quantity;
            int amountToAdd = Mathf.Min(spaceLeft, remaining);

            slot.quantity += amountToAdd;
            remaining -= amountToAdd;

            if (remaining <= 0)
            {
                NotifyInventoryChanged();
                return true;
            }
        }

        for (int i = 0; i < slots.Count; i++)
        {
            InventorySlot slot = slots[i];

            if (!slot.IsEmpty)
            {
                continue;
            }

            int amountToAdd = Mathf.Min(itemData.maxStackSize, remaining);
            slot.item = itemData;
            slot.quantity = amountToAdd;
            remaining -= amountToAdd;

            if (remaining <= 0)
            {
                NotifyInventoryChanged();
                return true;
            }
        }

        NotifyInventoryChanged();
        return false;
    }

    public bool RemoveFromSlot(int slotIndex, int quantity = 1)
    {
        if (!IsValidSlot(slotIndex) || quantity <= 0)
        {
            return false;
        }

        InventorySlot slot = slots[slotIndex];
        if (slot.IsEmpty)
        {
            return false;
        }

        slot.quantity -= quantity;

        if (slot.quantity <= 0)
        {
            slot.Clear();
        }

        NotifyInventoryChanged();
        return true;
    }

    public InventorySlot GetSelectedSlot()
    {
        if (!IsValidSlot(SelectedSlotIndex))
        {
            return null;
        }

        return slots[SelectedSlotIndex];
    }

    public void ToggleInventory()
    {
        SetInventoryOpen(!IsInventoryOpen);
    }

    public void SetInventoryOpen(bool isOpen)
    {
        if (IsInventoryOpen == isOpen)
        {
            return;
        }

        IsInventoryOpen = isOpen;
        inventoryUI.SetInventoryVisible(isOpen);
        NotifyInventoryChanged();
    }

    public void SetSelectedSlot(int slotIndex)
    {
        if (!IsValidSlot(slotIndex))
        {
            return;
        }

        SelectedSlotIndex = slotIndex;
        SelectedSlotChanged?.Invoke(SelectedSlotIndex);
        NotifyInventoryChanged();
    }

    public void DropOneFromSlot(int slotIndex)
    {
        if (!IsValidSlot(slotIndex))
        {
            return;
        }

        InventorySlot slot = slots[slotIndex];
        if (slot.IsEmpty)
        {
            return;
        }

        InventoryItemData itemToDrop = slot.item;
        RemoveFromSlot(slotIndex, 1);

        Vector3 dropPosition = transform.position + Vector3.right * 0.75f;
        WorldItemPickup.SpawnDroppedItem(itemToDrop, dropPosition, 1);
    }

    private void EnsureSlotCapacity()
    {
        while (slots.Count < TotalSlotCount)
        {
            slots.Add(new InventorySlot());
        }
    }

    private bool IsValidSlot(int slotIndex)
    {
        return slotIndex >= 0 && slotIndex < slots.Count;
    }

    private void NotifyInventoryChanged()
    {
        if (InventoryChanged != null)
        {
            InventoryChanged.Invoke();
        }
    }
}
