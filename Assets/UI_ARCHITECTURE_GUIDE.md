# UI/Frontend Architecture Guide

Complete guide for organizing and implementing UI in the roguelike dungeon crawler project.

---

## Table of Contents

1. [UI Framework Choice](#ui-framework-choice)
2. [UI Architecture Pattern](#ui-architecture-pattern)
3. [Folder Structure](#folder-structure)
4. [UI Manager Pattern](#ui-manager-pattern)
5. [Canvas Setup](#canvas-setup)
6. [UI Screens & Panels](#ui-screens--panels)
7. [Connecting UI to Backend](#connecting-ui-to-backend)
8. [Best Practices](#best-practices)
9. [Implementation Roadmap](#implementation-roadmap)

---

## UI Framework Choice

### Option 1: Unity UI (uGUI) - **RECOMMENDED**

✅ **Pros**:
- Mature, stable, well-documented
- Works in Unity 2022.3 LTS
- WYSIWYG editor
- Easy to learn
- Compatible with all Unity features
- Large community support

❌ **Cons**:
- Performance can be an issue with many elements (optimizable)
- Less modern than UI Toolkit

**Recommendation**: **Use Unity UI (uGUI)** for this project. It's proven, reliable, and perfect for 2D games.

---

### Option 2: UI Toolkit (formerly UIElements)

✅ **Pros**:
- Modern, performant
- CSS-like styling (USS)
- Better for complex UIs
- Better performance for many elements

❌ **Cons**:
- Steeper learning curve
- Less visual editor support
- Still evolving
- Fewer community resources

**When to use**: Large-scale projects with complex UIs or editor tools.

---

## UI Architecture Pattern

### Recommended Pattern: **MV-Presenter** (Model-View-Presenter Lite)

A simplified MVP pattern that works well with Unity:

```
Backend (Model)          UI (View)               Controller (Presenter)
─────────────────        ──────────────          ─────────────────────
InventorySystem    ←───  InventoryUI       ←───  InventoryController
  (Data + Logic)          (Display Only)          (Connects them)
```

### Why This Pattern?

1. **Separation of Concerns**: Backend systems don't know about UI
2. **Testable**: Can test backend without UI
3. **Flexible**: Easy to change UI without touching backend
4. **Unity-Friendly**: Works with existing Singleton pattern

---

## Folder Structure

### Recommended UI Organization

```
Assets/Features/
├── Inventory/
│   ├── InventorySystem.cs          # Backend (Model)
│   ├── ItemData.cs
│   ├── UI/
│   │   ├── InventoryUI.cs          # View (displays data)
│   │   ├── InventoryController.cs  # Presenter (connects Model & View)
│   │   ├── ItemSlotUI.cs           # Individual UI component
│   │   ├── ItemTooltipUI.cs
│   │   └── Prefabs/
│   │       ├── InventoryPanel.prefab
│   │       ├── ItemSlot.prefab
│   │       └── ItemTooltip.prefab
│   └── InventorySlot.cs
│
├── Crafting/
│   ├── CraftingSystem.cs           # Backend
│   ├── RecipeData.cs
│   └── UI/
│       ├── CraftingUI.cs           # View
│       ├── CraftingController.cs   # Presenter
│       ├── RecipeButtonUI.cs
│       └── Prefabs/
│           ├── CraftingPanel.prefab
│           └── RecipeButton.prefab
│
├── LevelSelection/
│   ├── LevelSelectionManager.cs    # Backend
│   ├── FloorData.cs
│   └── UI/
│       ├── LevelSelectionUI.cs     # View
│       ├── LevelSelectionController.cs
│       └── Prefabs/
│           └── LevelSelectionPanel.prefab
│
└── UI/                              # Shared UI components
    ├── UIManager.cs                 # Global UI controller (Singleton)
    ├── Shared/
    │   ├── ButtonUI.cs
    │   ├── TooltipUI.cs
    │   └── ConfirmDialogUI.cs
    └── Prefabs/
        ├── CommonButton.prefab
        ├── Tooltip.prefab
        └── ConfirmDialog.prefab
```

---

## UI Manager Pattern

### UIManager - Central UI Controller

A singleton that manages all UI screens and panels.

**Location**: `Assets/Features/UI/UIManager.cs`

```csharp
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central manager for all UI screens and panels.
/// Handles showing/hiding screens, transitions, and global UI state.
/// </summary>
public class UIManager : Singleton<UIManager>
{
    [Header("UI Screens")]
    [SerializeField] private List<UIScreen> allScreens = new List<UIScreen>();

    [Header("Settings")]
    [SerializeField] private bool hideOnStart = true;

    private UIScreen currentScreen;
    private Stack<UIScreen> screenHistory = new Stack<UIScreen>();

    protected override void Awake()
    {
        base.Awake();

        // Register all screens
        if (allScreens.Count == 0)
        {
            allScreens.AddRange(GetComponentsInChildren<UIScreen>(true));
        }

        if (hideOnStart)
        {
            HideAllScreens();
        }
    }

    /// <summary>
    /// Shows a specific UI screen by name
    /// </summary>
    public void ShowScreen(string screenName, bool addToHistory = true)
    {
        UIScreen screen = allScreens.Find(s => s.ScreenName == screenName);
        if (screen != null)
        {
            ShowScreen(screen, addToHistory);
        }
        else
        {
            Debug.LogWarning($"UIManager: Screen '{screenName}' not found");
        }
    }

    /// <summary>
    /// Shows a specific UI screen
    /// </summary>
    public void ShowScreen(UIScreen screen, bool addToHistory = true)
    {
        if (screen == null) return;

        // Add current screen to history
        if (addToHistory && currentScreen != null)
        {
            screenHistory.Push(currentScreen);
        }

        // Hide current screen
        if (currentScreen != null)
        {
            currentScreen.Hide();
        }

        // Show new screen
        currentScreen = screen;
        currentScreen.Show();
    }

    /// <summary>
    /// Goes back to previous screen in history
    /// </summary>
    public void GoBack()
    {
        if (screenHistory.Count > 0)
        {
            UIScreen previousScreen = screenHistory.Pop();
            ShowScreen(previousScreen, addToHistory: false);
        }
        else
        {
            Debug.LogWarning("UIManager: No screens in history to go back to");
        }
    }

    /// <summary>
    /// Hides the current screen
    /// </summary>
    public void HideCurrentScreen()
    {
        if (currentScreen != null)
        {
            currentScreen.Hide();
            currentScreen = null;
        }
    }

    /// <summary>
    /// Hides all UI screens
    /// </summary>
    public void HideAllScreens()
    {
        foreach (var screen in allScreens)
        {
            screen.Hide();
        }
        currentScreen = null;
        screenHistory.Clear();
    }

    /// <summary>
    /// Shows a popup message
    /// </summary>
    public void ShowPopup(string message, Sprite icon = null)
    {
        Debug.Log($"[POPUP] {message}");
        // TODO: Implement popup UI
    }

    /// <summary>
    /// Shows a confirmation dialog
    /// </summary>
    public void ShowConfirmation(string message, System.Action onConfirm, System.Action onCancel = null)
    {
        Debug.Log($"[CONFIRM] {message}");
        // TODO: Implement confirmation dialog
    }
}
```

### UIScreen - Base Class for All Screens

**Location**: `Assets/Features/UI/UIScreen.cs`

```csharp
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for all UI screens/panels.
/// Provides common functionality for showing, hiding, and managing UI screens.
/// </summary>
public class UIScreen : MonoBehaviour
{
    [Header("Screen Settings")]
    [SerializeField] private string screenName;
    [SerializeField] private bool showOnAwake = false;

    [Header("Events")]
    public UnityEvent OnScreenShown;
    public UnityEvent OnScreenHidden;

    public string ScreenName => screenName;
    public bool IsVisible { get; private set; }

    protected virtual void Awake()
    {
        // Auto-assign screen name if empty
        if (string.IsNullOrEmpty(screenName))
        {
            screenName = gameObject.name;
        }

        if (!showOnAwake)
        {
            Hide();
        }
    }

    protected virtual void Start()
    {
        if (showOnAwake)
        {
            Show();
        }
    }

    /// <summary>
    /// Shows this screen
    /// </summary>
    public virtual void Show()
    {
        gameObject.SetActive(true);
        IsVisible = true;
        OnShow();
        OnScreenShown?.Invoke();
    }

    /// <summary>
    /// Hides this screen
    /// </summary>
    public virtual void Hide()
    {
        IsVisible = false;
        OnHide();
        gameObject.SetActive(false);
        OnScreenHidden?.Invoke();
    }

    /// <summary>
    /// Toggle visibility
    /// </summary>
    public void Toggle()
    {
        if (IsVisible)
            Hide();
        else
            Show();
    }

    /// <summary>
    /// Called when screen is shown (override in subclasses)
    /// </summary>
    protected virtual void OnShow()
    {
        // Override in subclasses
    }

    /// <summary>
    /// Called when screen is hidden (override in subclasses)
    /// </summary>
    protected virtual void OnHide()
    {
        // Override in subclasses
    }
}
```

---

## Canvas Setup

### Scene Canvas Hierarchy

```
Scene Root
└── Canvas (Screen Space - Overlay)
    ├── EventSystem
    ├── HUD (Always visible)
    │   ├── HealthBar
    │   ├── TaskPanel
    │   └── QuickSlots
    │
    ├── Screens (Mutually exclusive)
    │   ├── InventoryScreen
    │   ├── CraftingScreen
    │   ├── LevelSelectionScreen
    │   └── PreparationScreen
    │
    └── Overlays (Can show over screens)
        ├── Tooltip
        ├── ConfirmDialog
        └── PopupMessage
```

### Canvas Setup Settings

**Main Canvas**:
- Render Mode: Screen Space - Overlay (for UI that's always on top)
- Canvas Scaler:
  - UI Scale Mode: Scale With Screen Size
  - Reference Resolution: 1920x1080 (or your target)
  - Match: 0.5 (balance between width/height)

**For World Space UI** (like damage numbers, health bars above enemies):
- Render Mode: World Space
- Render Camera: Main Camera

---

## UI Screens & Panels

### Example: Inventory UI Implementation

#### 1. InventoryUI.cs (View - Display Only)

**Location**: `Assets/Features/Inventory/UI/InventoryUI.cs`

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Inventory UI View - Displays inventory data.
/// Does NOT contain business logic.
/// </summary>
public class InventoryUI : UIScreen
{
    [Header("UI References")]
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private Button closeButton;

    [Header("Filters")]
    [SerializeField] private Button filterAllButton;
    [SerializeField] private Button filterWeaponsButton;
    [SerializeField] private Button filterArmorButton;
    [SerializeField] private Button filterConsumablesButton;
    [SerializeField] private Button filterMaterialsButton;

    private List<ItemSlotUI> itemSlots = new List<ItemSlotUI>();
    private InventoryController controller;

    protected override void Awake()
    {
        base.Awake();

        // Hook up buttons
        if (closeButton != null)
            closeButton.onClick.AddListener(OnCloseClicked);

        if (filterAllButton != null)
            filterAllButton.onClick.AddListener(() => OnFilterClicked(null));
        if (filterWeaponsButton != null)
            filterWeaponsButton.onClick.AddListener(() => OnFilterClicked(ItemType.Weapon));
        if (filterArmorButton != null)
            filterArmorButton.onClick.AddListener(() => OnFilterClicked(ItemType.Armor));
        if (filterConsumablesButton != null)
            filterConsumablesButton.onClick.AddListener(() => OnFilterClicked(ItemType.Consumable));
        if (filterMaterialsButton != null)
            filterMaterialsButton.onClick.AddListener(() => OnFilterClicked(ItemType.Material));
    }

    /// <summary>
    /// Initialize with controller
    /// </summary>
    public void Initialize(InventoryController controller)
    {
        this.controller = controller;
    }

    /// <summary>
    /// Refreshes the inventory display
    /// </summary>
    public void RefreshInventory(List<InventorySlot> items)
    {
        // Clear existing slots
        ClearItemSlots();

        // Create new slots for each item
        foreach (var item in items)
        {
            CreateItemSlot(item);
        }

        // Update item count text
        if (itemCountText != null)
        {
            itemCountText.text = $"Items: {items.Count}";
        }
    }

    private void CreateItemSlot(InventorySlot item)
    {
        GameObject slotObj = Instantiate(itemSlotPrefab, itemSlotContainer);
        ItemSlotUI slotUI = slotObj.GetComponent<ItemSlotUI>();

        if (slotUI != null)
        {
            slotUI.Setup(item, controller);
            itemSlots.Add(slotUI);
        }
    }

    private void ClearItemSlots()
    {
        foreach (var slot in itemSlots)
        {
            Destroy(slot.gameObject);
        }
        itemSlots.Clear();
    }

    private void OnCloseClicked()
    {
        controller?.CloseInventory();
    }

    private void OnFilterClicked(ItemType? filter)
    {
        controller?.FilterItems(filter);
    }

    protected override void OnHide()
    {
        base.OnHide();
        ClearItemSlots();
    }
}
```

#### 2. InventoryController.cs (Presenter - Connects Model & View)

**Location**: `Assets/Features/Inventory/UI/InventoryController.cs`

```csharp
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Inventory Controller - Connects InventorySystem (Model) to InventoryUI (View).
/// Contains UI logic and handles user input.
/// </summary>
public class InventoryController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryUI inventoryUI;

    private ItemType? currentFilter = null;

    private void Awake()
    {
        if (inventoryUI != null)
        {
            inventoryUI.Initialize(this);
        }
    }

    private void Start()
    {
        // Subscribe to inventory events
        if (InventorySystem.Instance != null)
        {
            InventorySystem.Instance.OnInventoryChanged.AddListener(OnInventoryChanged);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (InventorySystem.Instance != null)
        {
            InventorySystem.Instance.OnInventoryChanged.RemoveListener(OnInventoryChanged);
        }
    }

    private void Update()
    {
        // Keyboard shortcut to toggle inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        // ESC to close
        if (Input.GetKeyDown(KeyCode.Escape) && inventoryUI.IsVisible)
        {
            CloseInventory();
        }
    }

    /// <summary>
    /// Opens the inventory UI
    /// </summary>
    public void OpenInventory()
    {
        if (InventorySystem.Instance == null)
        {
            Debug.LogError("InventoryController: InventorySystem not found");
            return;
        }

        inventoryUI.Show();
        RefreshDisplay();
    }

    /// <summary>
    /// Closes the inventory UI
    /// </summary>
    public void CloseInventory()
    {
        inventoryUI.Hide();
    }

    /// <summary>
    /// Toggles inventory visibility
    /// </summary>
    public void ToggleInventory()
    {
        if (inventoryUI.IsVisible)
            CloseInventory();
        else
            OpenInventory();
    }

    /// <summary>
    /// Filters inventory by item type
    /// </summary>
    public void FilterItems(ItemType? filter)
    {
        currentFilter = filter;
        RefreshDisplay();
    }

    /// <summary>
    /// Called when an item slot is clicked
    /// </summary>
    public void OnItemClicked(InventorySlot slot)
    {
        Debug.Log($"Item clicked: {slot.ItemData.ItemName}");
        // TODO: Show item details, equip, use, etc.
    }

    /// <summary>
    /// Called when inventory changes
    /// </summary>
    private void OnInventoryChanged(InventorySlot slot)
    {
        if (inventoryUI.IsVisible)
        {
            RefreshDisplay();
        }
    }

    /// <summary>
    /// Refreshes the UI display
    /// </summary>
    private void RefreshDisplay()
    {
        List<InventorySlot> items = GetFilteredItems();
        inventoryUI.RefreshInventory(items);
    }

    /// <summary>
    /// Gets filtered inventory items
    /// </summary>
    private List<InventorySlot> GetFilteredItems()
    {
        if (InventorySystem.Instance == null)
            return new List<InventorySlot>();

        List<InventorySlot> items = InventorySystem.Instance.GetAllItems();

        if (currentFilter.HasValue)
        {
            items = items.Where(slot => slot.ItemData.Type == currentFilter.Value).ToList();
        }

        return items;
    }
}
```

#### 3. ItemSlotUI.cs (Individual UI Component)

**Location**: `Assets/Features/Inventory/UI/ItemSlotUI.cs`

```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Individual item slot UI component.
/// Displays a single inventory item.
/// </summary>
public class ItemSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private Image rarityBorder;
    [SerializeField] private GameObject equippedIndicator;

    private InventorySlot slot;
    private InventoryController controller;

    /// <summary>
    /// Setup this slot with item data
    /// </summary>
    public void Setup(InventorySlot slot, InventoryController controller)
    {
        this.slot = slot;
        this.controller = controller;

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (slot == null || slot.ItemData == null) return;

        // Set icon
        if (iconImage != null)
        {
            iconImage.sprite = slot.ItemData.Icon;
            iconImage.enabled = slot.ItemData.Icon != null;
        }

        // Set quantity
        if (quantityText != null)
        {
            if (slot.ItemData.IsStackable && slot.Quantity > 1)
            {
                quantityText.text = slot.Quantity.ToString();
                quantityText.enabled = true;
            }
            else
            {
                quantityText.enabled = false;
            }
        }

        // Set rarity border color
        if (rarityBorder != null)
        {
            rarityBorder.color = GetRarityColor(slot.ItemData.ItemRarity);
        }

        // Show equipped indicator
        if (equippedIndicator != null)
        {
            equippedIndicator.SetActive(slot.IsEquipped);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        controller?.OnItemClicked(slot);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // TODO: Show tooltip
        Debug.Log($"Hovering: {slot.ItemData.ItemName}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // TODO: Hide tooltip
    }

    private Color GetRarityColor(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Common => Color.white,
            Rarity.Uncommon => Color.green,
            Rarity.Rare => Color.blue,
            Rarity.Epic => new Color(0.64f, 0.21f, 0.93f), // Purple
            Rarity.Legendary => new Color(1f, 0.5f, 0f),   // Orange
            _ => Color.white
        };
    }
}
```

---

## Connecting UI to Backend

### Pattern Summary

```
User Action → UI View → Controller → Backend System → Event → Controller → UI View Update
```

### Example Flow: Adding an Item

```
1. Enemy dies → LootManager.DropLoot()
2. Player picks up → InventorySystem.AddItem()
3. InventorySystem fires OnItemAdded event
4. InventoryController receives event (subscribed)
5. InventoryController calls InventoryUI.RefreshInventory()
6. InventoryUI updates display
```

### Key Principles

1. **UI never directly modifies backend**
   ```csharp
   ❌ InventorySystem.Instance.items.Add(item);  // BAD
   ✅ InventorySystem.Instance.AddItem(item, 1); // GOOD
   ```

2. **Backend never references UI**
   ```csharp
   // In InventorySystem.cs
   ❌ inventoryUI.RefreshDisplay();  // BAD - backend shouldn't know about UI
   ✅ OnItemAdded?.Invoke(item, qty); // GOOD - use events
   ```

3. **Controller subscribes to backend events**
   ```csharp
   // In InventoryController.cs
   InventorySystem.Instance.OnItemAdded.AddListener(OnItemAdded);
   ```

---

## Best Practices

### 1. Use Object Pooling for Dynamic UI

For item slots, recipe buttons, etc. that are created/destroyed frequently:

```csharp
// Instead of Instantiate/Destroy every time
GameObject slot = ObjectPool.Get(itemSlotPrefab);
// ... use it ...
ObjectPool.Return(slot);
```

### 2. Lazy Loading

Don't refresh UI every frame, only when needed:

```csharp
// ❌ BAD - refreshes every frame
void Update() {
    RefreshInventory();
}

// ✅ GOOD - only refresh when data changes
void OnInventoryChanged() {
    if (inventoryUI.IsVisible) {
        RefreshInventory();
    }
}
```

### 3. Use Canvas Groups for Fade/Disable

```csharp
CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
canvasGroup.alpha = 0.5f;           // Semi-transparent
canvasGroup.interactable = false;    // Disable input
canvasGroup.blocksRaycasts = false;  // Don't block clicks
```

### 4. Separate Raycaster Cameras

For world-space UI health bars:
- Use a separate camera for UI rendering
- Avoids conflicts with main camera

### 5. Optimize Canvas Rebuilds

- Group static elements in one Canvas
- Group dynamic elements in another Canvas
- Reduces expensive Canvas.Rebuild() calls

---

## Implementation Roadmap

### Phase 1: Foundation (Week 1-2)

1. **Create UIManager & UIScreen base**
   - `UIManager.cs` singleton
   - `UIScreen.cs` base class
   - Canvas setup with EventSystem

2. **Implement InventoryUI**
   - `InventoryUI.cs` (View)
   - `InventoryController.cs` (Presenter)
   - `ItemSlotUI.cs` (Component)
   - Create prefabs

3. **Test with existing InventorySystem**
   - Wire up events
   - Test add/remove/filter

### Phase 2: Crafting UI (Week 3-4)

1. **CraftingUI**
   - `CraftingUI.cs` (View)
   - `CraftingController.cs` (Presenter)
   - `RecipeButtonUI.cs` (Component)

2. **Connect to CraftingSystem**
   - Wire up events
   - Test recipe selection and crafting

### Phase 3: Game Flow UIs (Week 5-6)

1. **LevelSelectionUI**
2. **PreparationUI**
3. **HUD (Health, Tasks)**

### Phase 4: Polish (Week 7+)

1. **Tooltips**
2. **Confirm Dialogs**
3. **Transitions/Animations**
4. **Settings Menu**

---

## Quick Start Checklist

- [ ] Create UIManager prefab in scene
- [ ] Create Canvas with proper settings
- [ ] Create InventoryUI panel prefab
- [ ] Create ItemSlotUI prefab
- [ ] Wire up InventoryController
- [ ] Test opening/closing inventory with 'I' key
- [ ] Test adding items shows in UI
- [ ] Test filtering works

---

**Next Steps**: Ready to implement InventoryUI? I can generate the full implementation files with this architecture!

**Last Updated**: 2025-11-13
