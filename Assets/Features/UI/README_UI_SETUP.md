# UI System - Quick Setup Guide

Quick reference for setting up the UI system in your scenes.

---

## 🚀 Quick Start (5 Minutes)

### Step 1: Create Canvas in Scene

1. Open your scene (e.g., `Testing Scene.unity`)
2. Right-click in Hierarchy → UI → Canvas
3. This creates:
   - Canvas (with Canvas component)
   - EventSystem (handles UI input)

### Step 2: Configure Canvas Settings

Select the Canvas and set:

**Canvas Component**:
- Render Mode: `Screen Space - Overlay`
- Pixel Perfect: ☐ (unchecked for better performance)

**Canvas Scaler Component**:
- UI Scale Mode: `Scale With Screen Size`
- Reference Resolution: `1920 x 1080`
- Match: `0.5` (balances width/height scaling)

### Step 3: Add UIManager

1. Create empty GameObject in scene root: `GameObject → Create Empty`
2. Name it: `UIManager`
3. Add component: `UIManager` script
4. Configure in Inspector:
   - Hide On Start: ☑ (checked - hides all screens at start)

### Step 4: Create Your First UI Screen

1. Right-click Canvas → UI → Panel
2. Name it: `InventoryScreen`
3. Add component: `InventoryUI` script (or just `UIScreen` for now)
4. Configure UIScreen settings:
   - Screen Name: `Inventory` (or leave blank to auto-use GameObject name)
   - Show On Awake: ☐ (unchecked - will show manually)
   - Register With Manager: ☑ (checked)

### Step 5: Test It!

```csharp
// Test in any script
void Update()
{
    if (Input.GetKeyDown(KeyCode.I))
    {
        UIManager.Instance.ShowScreen("InventoryScreen");
    }

    if (Input.GetKeyDown(KeyCode.Escape))
    {
        UIManager.Instance.HideCurrentScreen();
    }
}
```

---

## 📐 Recommended Canvas Hierarchy

```
Scene Root
├── UIManager                    (empty GameObject with UIManager script)
│
└── Canvas                       (UI root)
    ├── EventSystem
    │
    ├── HUD                      (Panel - always visible)
    │   ├── HealthBar
    │   ├── TaskPanel
    │   └── QuickSlots
    │
    ├── Screens                  (Empty GameObject - organizer)
    │   ├── InventoryScreen      (Panel with UIScreen script)
    │   ├── CraftingScreen       (Panel with UIScreen script)
    │   └── LevelSelectionScreen (Panel with UIScreen script)
    │
    └── Overlays                 (Empty GameObject - organizer)
        ├── Tooltip              (Panel - shows over screens)
        └── ConfirmDialog        (Panel - shows over screens)
```

---

## 🎨 Creating UI Screens

### Option A: Inherit from UIScreen

For simple display-only screens:

```csharp
using UnityEngine;

public class MyCustomScreen : UIScreen
{
    protected override void OnShow()
    {
        base.OnShow();
        // Called when screen is shown
        Debug.Log("MyCustomScreen opened!");
    }

    protected override void OnHide()
    {
        base.OnHide();
        // Called when screen is hidden
        Debug.Log("MyCustomScreen closed!");
    }
}
```

### Option B: UIScreen + Controller Pattern

For screens with complex logic (RECOMMENDED for Inventory, Crafting, etc.):

**View** (`InventoryUI.cs`):
```csharp
public class InventoryUI : UIScreen
{
    // Only display logic here
    public void RefreshDisplay(List<Item> items) { }
}
```

**Controller** (`InventoryController.cs`):
```csharp
public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            inventoryUI.Toggle();
    }
}
```

---

## 💻 Common Usage Patterns

### Show Screen by Name

```csharp
UIManager.Instance.ShowScreen("InventoryScreen");
```

### Show Screen by Reference

```csharp
[SerializeField] private UIScreen inventoryScreen;

void OpenInventory()
{
    UIManager.Instance.ShowScreen(inventoryScreen);
}
```

### Hide Current Screen

```csharp
UIManager.Instance.HideCurrentScreen();
```

### Hide All Screens

```csharp
UIManager.Instance.HideAllScreens();
```

### Check if Screen is Visible

```csharp
if (UIManager.Instance.IsScreenVisible("InventoryScreen"))
{
    Debug.Log("Inventory is open!");
}
```

### Go Back to Previous Screen

```csharp
UIManager.Instance.GoBack();
```

---

## 🎯 Screen Navigation Example

```csharp
// User opens inventory
UIManager.Instance.ShowScreen("InventoryScreen");
// History: [empty]

// User opens crafting from inventory
UIManager.Instance.ShowScreen("CraftingScreen");
// History: [InventoryScreen]

// User presses ESC or Back button
UIManager.Instance.GoBack();
// Now showing InventoryScreen again
```

---

## 🔧 Canvas Best Practices

### 1. Separate Static and Dynamic UI

**Static Canvas** (doesn't change often):
- HUD elements
- Background panels
- Fixed decorations

**Dynamic Canvas** (changes frequently):
- Inventory grid (items added/removed)
- Chat messages
- Damage numbers

This reduces expensive Canvas rebuilds.

### 2. Use Canvas Groups for Fading

```csharp
CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

// Fade out
canvasGroup.alpha = 0.5f;

// Disable input but keep visible
canvasGroup.interactable = false;
canvasGroup.blocksRaycasts = false;
```

UIScreen already does this automatically if `Use Animation` is checked!

### 3. Anchor UI Elements Properly

- **Top-left UI** (health bar): Anchor to top-left
- **Center UI** (inventory): Anchor to center
- **Bottom UI** (hotbar): Anchor to bottom
- **Full-screen**: Anchor to stretch (all corners)

### 4. Use Layout Groups

For dynamic lists (inventory, crafting recipes):
- Use `Vertical Layout Group` or `Horizontal Layout Group`
- Or use `Grid Layout Group` for grid-based layouts

---

## 🎮 Input Handling

### Global UI Input (Recommended)

Put this in UIManager or a dedicated InputManager:

```csharp
void Update()
{
    // Toggle inventory
    if (Input.GetKeyDown(KeyCode.I))
        UIManager.Instance.ShowScreen("InventoryScreen");

    // Toggle crafting
    if (Input.GetKeyDown(KeyCode.C))
        UIManager.Instance.ShowScreen("CraftingScreen");

    // Close any open screen
    if (Input.GetKeyDown(KeyCode.Escape))
        UIManager.Instance.HideCurrentScreen();
}
```

### Per-Screen Input

Put this in the Controller for each screen:

```csharp
// In InventoryController.cs
void Update()
{
    if (!inventoryUI.IsVisible) return; // Only handle input if visible

    if (Input.GetKeyDown(KeyCode.Escape))
        CloseInventory();
}
```

---

## 🐛 Troubleshooting

### Screen doesn't show when I call ShowScreen()

**Check**:
1. Is UIManager in the scene?
2. Is the screen registered? (Check `Register With Manager` is checked)
3. Is the screen name correct? (case-sensitive!)
4. Check Console for warnings

**Debug**:
```csharp
UIManager.Instance.DebugPrintScreens(); // Right-click UIManager → Debug: Print All Screens
```

### UI doesn't respond to clicks

**Check**:
1. Is there an EventSystem in the scene?
2. Is the Canvas Graphic Raycaster enabled?
3. Is CanvasGroup.blocksRaycasts = true?
4. Is there a collider blocking UI clicks?

### Screen shows but is invisible

**Check**:
1. CanvasGroup.alpha should be 1
2. All parent objects are active
3. Canvas is set to correct render mode
4. Check if it's behind another UI element (wrong sorting)

### Performance issues with many UI elements

**Solutions**:
1. Use object pooling for dynamic elements
2. Separate static/dynamic canvases
3. Disable Pixel Perfect on Canvas
4. Use fewer Layout Groups (they're expensive)
5. Don't call SetActive() every frame on UI

---

## 📚 Next Steps

1. **Phase 1**: Create InventoryUI with ItemSlots
   - See `UI_ARCHITECTURE_GUIDE.md` for full example
   - Create ItemSlotUI prefab

2. **Phase 2**: Create CraftingUI with RecipeButtons
   - Similar pattern to InventoryUI

3. **Phase 3**: Create HUD (health, tasks)
   - Don't use UIScreen (HUD is always visible)
   - Create separate HUD manager

4. **Phase 4**: Add animations and polish
   - Use CanvasGroup for fades
   - Add button hover effects
   - Add screen transition animations

---

## 📖 Related Documentation

- **Full UI Architecture**: `UI_ARCHITECTURE_GUIDE.md`
- **Inventory System**: `Features/Inventory/README_INVENTORY.md`
- **Project Structure**: `PROJECT_STRUCTURE_GUIDE.md`

---

**Summary**: UIManager + UIScreen pattern gives you a flexible, scalable UI system that works with your existing Singleton backend systems!

**Last Updated**: 2025-11-13
