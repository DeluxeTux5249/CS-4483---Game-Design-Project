using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Hotbar slots stay visible, extra rows only show when the inventory is open.
    [SerializeField] private int hotbarSize = 8;
    [SerializeField] private int inventoryRows = 3;
    [SerializeField] private int inventoryColumns = 8;

    // main inventory storage list
    private readonly List<InventorySlot> slots = new List<InventorySlot>();
    // reference to the UI that displays this inventory
    private InventoryUI inventoryUI;

    // events used so the UI can update whenever something changes
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
        // Create the slot list and connect it to the runtime inventory UI.
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
        // reject invalid add requests
        if (itemData == null || quantity <= 0)
        {
            return false;
        }

        // tracks how many items still need to be placed into slots
        int remaining = quantity;

        // First try to add onto existing stacks of the same item.
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

        // If no matching stack has room, use the first empty slot.
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
        // reject invalid slot indexes or quantities
        if (!IsValidSlot(slotIndex) || quantity <= 0)
        {
            return false;
        }

        // do nothing if the target slot is already empty
        InventorySlot slot = slots[slotIndex];
        if (slot.IsEmpty)
        {
            return false;
        }

        // subtract from the stack
        slot.quantity -= quantity;

        // fully clear the slot if the stack reaches zero
        if (slot.quantity <= 0)
        {
            slot.Clear();
        }

        NotifyInventoryChanged();
        return true;
    }

    public InventorySlot GetSelectedSlot()
    {
        // returns the slot currently highlighted by the player
        if (!IsValidSlot(SelectedSlotIndex))
        {
            return null;
        }

        return slots[SelectedSlotIndex];
    }

    public void ToggleInventory()
    {
        // swap between open and closed inventory states
        SetInventoryOpen(!IsInventoryOpen);
    }

    public void SetInventoryOpen(bool isOpen)
    {
        if (IsInventoryOpen == isOpen)
        {
            return;
        }

        // The UI decides whether only the hotbar or the full inventory should be shown.
        IsInventoryOpen = isOpen;
        inventoryUI.SetInventoryVisible(isOpen);
        NotifyInventoryChanged();
    }

    public void SetSelectedSlot(int slotIndex)
    {
        // ignore invalid slot numbers
        if (!IsValidSlot(slotIndex))
        {
            return;
        }

        // store the selected slot and notify the UI
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

        // Remove one item from this slot and spawn it back into the world.
        InventoryItemData itemToDrop = slot.item;
        RemoveFromSlot(slotIndex, 1);

        Vector3 dropPosition = transform.position + Vector3.right * 0.75f;
        WorldItemPickup.SpawnDroppedItem(itemToDrop, dropPosition, 1);
    }

    private void EnsureSlotCapacity()
    {
        // Fill the list with empty slots until it matches the configured size.
        while (slots.Count < TotalSlotCount)
        {
            slots.Add(new InventorySlot());
        }
    }

    private bool IsValidSlot(int slotIndex)
    {
        // makes sure the requested slot exists in the list
        return slotIndex >= 0 && slotIndex < slots.Count;
    }

    private void NotifyInventoryChanged()
    {
        // tells any listening UI that the inventory needs a redraw
        if (InventoryChanged != null)
        {
            InventoryChanged.Invoke();
        }
    }
}
