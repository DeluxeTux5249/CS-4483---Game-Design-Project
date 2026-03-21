using UnityEngine;
using UnityEngine.UI;

public class ShopTrigger : MonoBehaviour
{
    public Button shopButton;
    public GameObject shopCanvas;
    public ShopManger shopManger;
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

    public void CloseShopTemp()
    {
        shopCanvas.SetActive(false);
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

    public void gotoPreviewMode()
    {
        shopButton.gameObject.SetActive(false);
    }

    public void gotoNormalMode()
    {
        shopButton.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (shopManger != null && shopManger.isViewingZone && shopManger.previewObject != null)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;
            shopManger.UpdatePreview(mouseWorld);

            if (Input.GetMouseButtonDown(0))
                shopManger.TryPlace(mouseWorld);
        }

        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        if (shopCanvas != null && shopCanvas.activeSelf)
            CloseShop();
        else if (shopManger != null && shopManger.isViewingZone)
            shopManger.CancelZoneView();
    }
}
