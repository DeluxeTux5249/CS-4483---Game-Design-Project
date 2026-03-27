using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour, IPointerClickHandler
{
    private Image iconImage;
    private Text quantityText;
    private Image highlightImage;
    private Button button;

    private PlayerInventory inventory;
    private int slotIndex;

    public void SetReferences(Image newIconImage, Text newQuantityText, Image newHighlightImage, Button newButton)
    {
        iconImage = newIconImage;
        quantityText = newQuantityText;
        highlightImage = newHighlightImage;
        button = newButton;
    }

    public void Initialize(PlayerInventory targetInventory, int index)
    {
        inventory = targetInventory;
        slotIndex = index;

        if (button == null)
        {
            button = GetComponent<Button>();
        }
    }

    public void Refresh(InventorySlot slot, bool isSelected)
    {
        bool hasItem = slot != null && !slot.IsEmpty;

        iconImage.enabled = hasItem && slot.item.icon != null;
        iconImage.sprite = hasItem ? slot.item.icon : null;
        quantityText.text = hasItem && slot.quantity > 1 ? slot.quantity.ToString() : "";
        highlightImage.enabled = isSelected;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (inventory == null)
        {
            return;
        }

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
