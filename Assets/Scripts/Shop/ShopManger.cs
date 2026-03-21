using System.Collections.Generic;
using UnityEngine;

public class ShopManger : MonoBehaviour
{
    [SerializeField] private List<ShopItems> shopItems;
    [SerializeField] private List<ShopItems> shopBuildings;
    [SerializeField] private List<ShopItems> shopTroops;
    [SerializeField] private ShopSlot[] shopSlots;
    string currentTab = "Items";

    private void Start()
    {
        populateShopItems();
    }

    public void shopUpdate(string tab)
    {
        currentTab = tab;
        if (currentTab == "Items")
        {
            populateShopItems();
        }
        else if (currentTab == "Buildings")
        {
            populateShopBuildings();
        }
        else if (currentTab == "Troops")
        {
            populateShopTroops();
        }
    }
    public void populateShopItems()
    {
        for (int i = 0; i < shopSlots.Length; i++)
        {
            if (i < shopItems.Count)
            {
                print("setting item " + shopItems[i].item.itemName + " with price " + shopItems[i].price);
                shopSlots[i].SetItem(shopItems[i].item, shopItems[i].price);
                shopSlots[i].gameObject.SetActive(true);
            }
        }
        for (int i = shopItems.Count; i < shopSlots.Length; i++)
        {
            if (shopSlots[i] != null)
                shopSlots[i].gameObject.SetActive(false);
        }
    }

    public void populateShopBuildings()
    {
        for (int i = 0; i < shopSlots.Length; i++)
        {
            if (i < shopBuildings.Count)
            {
                print("setting item " + shopItems[i].item.itemName + " with price " + shopItems[i].price);
                shopSlots[i].SetItem(shopBuildings[i].item, shopBuildings[i].price);
                shopSlots[i].gameObject.SetActive(true);
            }
        }
        for (int i = shopBuildings.Count; i < shopSlots.Length; i++)
        {
            if (shopSlots[i] != null)
                shopSlots[i].gameObject.SetActive(false);
        }
    }

    public void populateShopTroops()
    {
        for (int i = 0; i < shopSlots.Length; i++)
        {
            if (i < shopTroops.Count)
            {
                print("setting item " + shopItems[i].item.itemName + " with price " + shopItems[i].price);
                shopSlots[i].SetItem(shopTroops[i].item, shopTroops[i].price);
                shopSlots[i].gameObject.SetActive(true);
            }
        }
        for (int i = shopTroops.Count; i < shopSlots.Length; i++)
        {
            if (shopSlots[i] != null)
                shopSlots[i].gameObject.SetActive(false);
        }
    }
}

[System.Serializable]
public class ShopItems
{
    public ItemSO item;
    public int price;
}
