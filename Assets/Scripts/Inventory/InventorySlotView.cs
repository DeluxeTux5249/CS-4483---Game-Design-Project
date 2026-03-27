using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour, IPointerClickHandler
{
    // References to the visual parts inside a single UI slot.
    private Image iconImage;
    private Text quantityText;
    private Image highlightImage;
    private Button button;

    // inventory data source and the slot index this view represents
    private PlayerInventory inventory;
    private int slotIndex;

    // called by InventoryUI after creating the slot
    public void SetReferences(Image newIconImage, Text newQuantityText, Image newHighlightImage, Button newButton)
    {
        iconImage = newIconImage;
        quantityText = newQuantityText;
        highlightImage = newHighlightImage;
        button = newButton;
    }

    public void Initialize(PlayerInventory targetInventory, int index)
    {
        // stores which inventory and slot this UI object controls
        inventory = targetInventory;
        slotIndex = index;

        // fallback in case the button was not passed in manually
        if (button == null)
        {
            button = GetComponent<Button>();
        }
    }

    public void Refresh(InventorySlot slot, bool isSelected)
    {
        // Sync this UI slot with the data from the matching inventory slot.
        bool hasItem = slot != null && !slot.IsEmpty;

        iconImage.enabled = hasItem && slot.item.icon != null;
        iconImage.sprite = hasItem ? slot.item.icon : null;
        quantityText.text = hasItem && slot.quantity > 1 ? slot.quantity.ToString() : "";
        highlightImage.enabled = isSelected;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // stop if the slot has not been connected yet
        if (inventory == null)
        {
            return;
        }

        // Left click selects the slot, right click drops one item while inventory is open.
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            inventory.SetSelectedSlot(slotIndex);
        }
        else if (eventData.button == PointerEventData.InputButton.Right && inventory.IsInventoryOpen)
        {
            inventory.DropOneFromSlot(slotIndex);
        }
    }
}
