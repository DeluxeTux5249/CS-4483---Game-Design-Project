using UnityEngine;
using UnityEngine.UI;

public class ShopTrigger : MonoBehaviour
{
    public Button shopButton;
    public GameObject shopCanvas;
    public Button closeShopButton;

    private PlayerMovement playerMovement;

    public void OpenShop()
    {
        shopCanvas.SetActive(true);
        if (playerMovement != null)
            playerMovement.canMove = false;
    }

    public void CloseShop()
    {
        shopCanvas.SetActive(false);
        if (playerMovement != null)
            playerMovement.canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerMovement = collision.GetComponent<PlayerMovement>();
            shopButton.gameObject.SetActive(true);
            shopButton.onClick.AddListener(this.OpenShop);
            if (closeShopButton != null)
                closeShopButton.onClick.AddListener(this.CloseShop);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CloseShop();
            shopButton.onClick.RemoveListener(this.OpenShop);
            shopButton.gameObject.SetActive(false);
            if (closeShopButton != null)
                closeShopButton.onClick.RemoveListener(this.CloseShop);
            playerMovement = null;
        }
    }
}
