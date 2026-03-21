using UnityEngine;
using UnityEngine.UI;

public class ShopTrigger : MonoBehaviour
{
    public Button shopButton;
    public GameObject shopCanvas;
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            shopButton.onClick.RemoveListener(this.OpenShop);
            shopButton.gameObject.SetActive(false);
            playerMovement = null;
        }
    }

    private void Update()
    {
        if (shopCanvas != null && shopCanvas.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
    }
}
