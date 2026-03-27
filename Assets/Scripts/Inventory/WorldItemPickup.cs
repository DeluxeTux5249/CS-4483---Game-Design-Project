using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WorldItemPickup : MonoBehaviour
{
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int quantity = 1;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Collider2D pickupCollider = GetComponent<Collider2D>();
        pickupCollider.isTrigger = true;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null && itemData != null && itemData.icon != null)
        {
            spriteRenderer.sprite = itemData.icon;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory == null)
        {
            playerInventory = other.GetComponentInParent<PlayerInventory>();
        }

        if (playerInventory == null)
        {
            return;
        }

        if (playerInventory.AddItem(itemData, quantity))
        {
            Destroy(gameObject);
        }
    }


    public static WorldItemPickup SpawnDroppedItem(InventoryItemData itemData, Vector3 worldPosition, int quantity = 1)
    {
        if (itemData == null)
        {
            return null;
        }

        GameObject pickupObject = new GameObject(itemData.itemName + "_Pickup");
        pickupObject.transform.position = worldPosition;

        SpriteRenderer renderer = pickupObject.AddComponent<SpriteRenderer>();
        renderer.sprite = itemData.icon;
        renderer.sortingOrder = 10;

        CircleCollider2D circleCollider = pickupObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = 0.35f;
        circleCollider.isTrigger = true;

        WorldItemPickup pickup = pickupObject.AddComponent<WorldItemPickup>();
        pickup.itemData = itemData;
        pickup.quantity = quantity;
        pickup.spriteRenderer = renderer;

        return pickup;
    }
}