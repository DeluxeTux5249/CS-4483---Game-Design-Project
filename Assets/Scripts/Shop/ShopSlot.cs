using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public ItemSO itemSO;
    public TMP_Text itemNameText;
    public TMP_Text priceText;
    public Image itemIcon;

    private int price;

    private void Start()
    {
        if (itemSO != null)
            SetItem(itemSO, price);
    }

    public void SetItem(ItemSO item, int price)
    {
        this.itemSO = item;
        print(item.itemName + " " + price);
        itemNameText.text = this.itemSO.itemName;
        itemIcon.sprite = this.itemSO.icon;
        this.price = price;
        priceText.text = price.ToString();
        print(itemNameText.text + " " + this.price);
    }
}
