using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Inventory/Item Data")]
public class InventoryItemData : ScriptableObject
{
    [Header("Display")]
    public string itemName = "New Item";
    [TextArea]
    public string description;
    public Sprite icon;

    [Header("Stacking")]
    [Min(1)]
    public int maxStackSize = 99;
}
