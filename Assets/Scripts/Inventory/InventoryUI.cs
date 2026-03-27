using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{
    private const int SlotSize = 58;
    private const int SlotSpacing = 8;

    private PlayerInventory inventory;
    private GameObject inventoryPanel;
    private readonly List<InventorySlotView> slotViews = new List<InventorySlotView>();
    private Font builtInFont;

    public void Initialize(PlayerInventory targetInventory)
    {
        inventory = targetInventory;
        builtInFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        EnsureEventSystem();
        BuildUi();

        inventory.InventoryChanged -= Refresh;
        inventory.InventoryChanged += Refresh;

        inventory.SelectedSlotChanged -= HandleSelectedSlotChanged;
        inventory.SelectedSlotChanged += HandleSelectedSlotChanged;

        Refresh();
        SetInventoryVisible(inventory.IsInventoryOpen);
    }

    public void SetInventoryVisible(bool visible)
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(visible);
        }
    }

    private void BuildUi()
    {
        Canvas canvas = GetComponentInChildren<Canvas>();

        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("InventoryCanvas");
            canvasObject.transform.SetParent(transform, false);

            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;

            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);

            canvasObject.AddComponent<GraphicRaycaster>();
        }

        CreateHotBar(canvas.transform);
        inventoryPanel = CreateInventoryPanel(canvas.transform);
    }

    private void CreateHotBar(Transform parent)
    {
        GameObject hotbarRoot = new GameObject("Hotbar");
        hotbarRoot.transform.SetParent(parent, false);

        RectTransform hotbarRect = hotbarRoot.AddComponent<RectTransform>();
        hotbarRect.anchorMin = new Vector2(0.5f, 0f);
        hotbarRect.anchorMax = new Vector2(0.5f, 0f);
        hotbarRect.pivot = new Vector2(0.5f, 0f);
        hotbarRect.anchoredPosition = new Vector2(0f, 28f);
        hotbarRect.sizeDelta = new Vector2((SlotSize + SlotSpacing) * inventory.HotbarSize, SlotSize + 18f);

        HorizontalLayoutGroup layout = hotbarRoot.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = SlotSpacing;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlHeight = false;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;

        for (int i = 0; i < inventory.HotbarSize; i++)
        {
            slotViews.Add(CreateSlotView(hotbarRoot.transform, i));
        }
    }

    private GameObject CreateInventoryPanel(Transform parent)
    {
        GameObject panelRoot = new GameObject("InventoryPanel");
        panelRoot.transform.SetParent(parent, false);

        Image panelImage = panelRoot.AddComponent<Image>();
        panelImage.color = new Color(0.09f, 0.11f, 0.14f, 0.92f);

        RectTransform panelRect = panelRoot.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(620f, 320f);

        CreateLabel(panelRoot.transform, "InventoryTitle", "Inventory", 24, new Vector2(0f, 130f), TextAnchor.MiddleCenter);
        CreateLabel(panelRoot.transform, "InventoryHint", "Left click to select. Right click to drop one.", 14, new Vector2(0f, -132f), TextAnchor.MiddleCenter);

        GameObject gridRoot = new GameObject("InventoryGrid");
        gridRoot.transform.SetParent(panelRoot.transform, false);

        RectTransform gridRect = gridRoot.AddComponent<RectTransform>();
        gridRect.anchorMin = new Vector2(0.5f, 0.5f);
        gridRect.anchorMax = new Vector2(0.5f, 0.5f);
        gridRect.pivot = new Vector2(0.5f, 0.5f);
        gridRect.anchoredPosition = Vector2.zero;
        gridRect.sizeDelta = new Vector2(520f, 200f);

        GridLayoutGroup grid = gridRoot.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(SlotSize, SlotSize);
        grid.spacing = new Vector2(SlotSpacing, SlotSpacing);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = inventory.HotbarSize;
        grid.childAlignment = TextAnchor.MiddleCenter;

        for (int i = inventory.HotbarSize; i < inventory.TotalSlotCount; i++)
        {
            slotViews.Add(CreateSlotView(gridRoot.transform, i));
        }

        return panelRoot;
    }

    private InventorySlotView CreateSlotView(Transform parent, int slotIndex)
    {
        GameObject slotObject = new GameObject("Slot_" + slotIndex);
        slotObject.transform.SetParent(parent, false);

        Image slotImage = slotObject.AddComponent<Image>();
        slotImage.color = new Color(0.18f, 0.2f, 0.24f, 0.95f);

        Button button = slotObject.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white; 
        colors.highlightedColor = new Color(1f, 1f, 1f, 0.95f);
        colors.pressedColor = new Color(0.85f, 0.85f, 0.85f, 0.95f);
        button.colors = colors;

        RectTransform slotRect = slotObject.GetComponent<RectTransform>();
        slotRect.sizeDelta = new Vector2(SlotSize, SlotSize);

        GameObject highlightObject = new GameObject("Highlight");
        highlightObject.transform.SetParent(slotObject.transform, false);

        Image highlightImage = highlightObject.AddComponent<Image>();
        highlightImage.color = new Color(0.94f, 0.82f, 0.39f, 0.45f);

        RectTransform highlightRect = highlightObject.GetComponent<RectTransform>();
        highlightRect.anchorMin = Vector2.zero;
        highlightRect.anchorMax = Vector2.one;
        highlightRect.offsetMin = Vector2.zero;
        highlightRect.offsetMax = Vector2.zero;

        GameObject iconObject = new GameObject("Icon");
        iconObject.transform.SetParent(slotObject.transform, false);

        Image iconImage = iconObject.AddComponent<Image>();
        RectTransform iconRect = iconObject.GetComponent<RectTransform>();
        iconRect.anchorMin = new Vector2(0.5f, 0.5f);
        iconRect.anchorMax = new Vector2(0.5f, 0.5f);
        iconRect.pivot = new Vector2(0.5f, 0.5f);
        iconRect.sizeDelta = new Vector2(40f, 40f);

        GameObject quantityObject = new GameObject("Quantity");
        quantityObject.transform.SetParent(slotObject.transform, false);

        Text quantityText = quantityObject.AddComponent<Text>();
        quantityText.font = builtInFont;
        quantityText.fontSize = 14;
        quantityText.alignment = TextAnchor.LowerRight;
        quantityText.color = Color.white;

        RectTransform quantityRect = quantityObject.GetComponent<RectTransform>();
        quantityRect.anchorMin = Vector2.zero;
        quantityRect.anchorMax = Vector2.one;
        quantityRect.offsetMin = new Vector2(6f, 4f);
        quantityRect.offsetMax = new Vector2(-6f, -4f);

        InventorySlotView slotView = slotObject.AddComponent<InventorySlotView>();
        slotView.SetReferences(iconImage, quantityText, highlightImage, button);
        slotView.Initialize(inventory, slotIndex);

        return slotView;
    }

    private void Refresh()
    {
        if (inventory == null)
        {
            return;
        }

        for (int i = 0; i < slotViews.Count && i < inventory.Slots.Count; i++)
        {
            slotViews[i].Refresh(inventory.Slots[i], i == inventory.SelectedSlotIndex);
        }

    }


    private void HandleSelectedSlotChanged(int selectedIndex)
    {
        Refresh();
    }

    
    private void CreateLabel(Transform parent, string objectName, string textValue, int fontSize, Vector2 position, TextAnchor alignment)
    {
        GameObject labelObject = new GameObject(objectName);
        labelObject.transform.SetParent(parent, false);

        Text text = labelObject.AddComponent<Text>();
        text.font = builtInFont;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = Color.white;
        text.text = textValue;

        RectTransform rectTransform = labelObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(540f, 32f);
    }

    private void EnsureEventSystem()
    {
        if (FindObjectOfType<EventSystem>() != null)
        {
            return;
        }
    
    GameObject eventSystemObject = new GameObject("EventSystem");
    eventSystemObject.AddComponent<EventSystem>();
    eventSystemObject.AddComponent<InputSystemUIInputModule>();
    }
}
