using UnityEngine;
using UnityEngine.UI;

public class ShopTrigger : MonoBehaviour
{
    public Button shopButton;
    public GameObject shopCanvas;

    public void OpenShop()
    {
        shopCanvas.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            shopButton.gameObject.SetActive(true);
            shopButton.onClick.AddListener(this.OpenShop);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            shopButton.onClick.RemoveListener(this.OpenShop);
            shopButton.gameObject.SetActive(false);
        }
    }
}
