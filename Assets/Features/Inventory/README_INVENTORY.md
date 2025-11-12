# Inventory System - Setup Guide

## Overview
The inventory system manages item storage, retrieval, and tracking across the game. It uses a singleton pattern and persists between scenes.

## Files Created
- `ItemData.cs` - ScriptableObject for defining items
- `InventorySlot.cs` - Data class representing inventory slots
- `InventorySystem.cs` - Singleton manager for inventory operations
- `InventoryTester.cs` - Test script for verifying functionality

## Setup Instructions

### Step 1: Create Sample ItemData Assets

1. In Unity, navigate to `Assets/Data/Items/Weapons/`
2. Right-click → Create → Game → Inventory → Item Data
3. Name it "Iron Sword"
4. Configure the item:
   - Item Name: "Iron Sword"
   - Description: "A basic iron sword"
   - Item Type: Weapon
   - Rarity: Common
   - Is Stackable: false (weapons typically don't stack)
   - Damage: 10
   - Attack Speed: 1.0

5. Create more sample items:

**Material Example** (`Assets/Data/Items/Materials/`):
   - Item Name: "Iron Ore"
   - Item Type: Material
   - Rarity: Common
   - Is Stackable: true
   - Max Stack Size: 99

**Consumable Example** (`Assets/Data/Items/Consumables/`):
   - Item Name: "Health Potion"
   - Item Type: Consumable
   - Rarity: Uncommon
   - Is Stackable: true
   - Max Stack Size: 20

**Armor Example** (`Assets/Data/Items/Armor/`):
   - Item Name: "Leather Armor"
   - Item Type: Armor
   - Rarity: Common
   - Is Stackable: false
   - Defense: 5

### Step 2: Add InventorySystem to Scene

1. Open your testing scene (e.g., `Testing Scene.unity`)
2. Create an empty GameObject: GameObject → Create Empty
3. Rename it to "InventorySystem"
4. Add the `InventorySystem` component
5. The system will persist across scenes automatically

### Step 3: Test the System

**Option A: Use InventoryTester**
1. Create another empty GameObject named "InventoryTester"
2. Add the `InventoryTester` component
3. In the Inspector, assign your created ItemData assets to:
   - Test Weapon
   - Test Armor
   - Test Consumable
   - Test Material
4. Check "Run Tests On Start"
5. Enter Play Mode
6. Watch the Console for test results
7. Use keyboard shortcuts:
   - Press 1: Add items to inventory
   - Press 2: Remove some items
   - Press 3: Print inventory contents
   - Press 4: Clear inventory

**Option B: Use Code**
```csharp
// Add items
InventorySystem.Instance.AddItem(myItemData, 5);

// Check if has item
bool hasItem = InventorySystem.Instance.HasItem(myItemData, 3);

// Get item count
int count = InventorySystem.Instance.GetItemCount(myItemData);

// Remove items
bool success = InventorySystem.Instance.RemoveItem(myItemData, 2);

// Get all items
List<InventorySlot> allItems = InventorySystem.Instance.GetAllItems();

// Filter by type
List<InventorySlot> weapons = InventorySystem.Instance.GetItemsByType(ItemType.Weapon);

// Clear inventory
InventorySystem.Instance.ClearInventory();
```

## Events

Subscribe to inventory events for UI updates:

```csharp
void Start()
{
    InventorySystem.Instance.OnItemAdded.AddListener(HandleItemAdded);
    InventorySystem.Instance.OnItemRemoved.AddListener(HandleItemRemoved);
    InventorySystem.Instance.OnInventoryChanged.AddListener(HandleInventoryChanged);
}

void HandleItemAdded(ItemData item, int quantity)
{
    Debug.Log($"Added {quantity}x {item.ItemName}");
}

void HandleItemRemoved(ItemData item, int quantity)
{
    Debug.Log($"Removed {quantity}x {item.ItemName}");
}

void HandleInventoryChanged(InventorySlot slot)
{
    // Update UI
}
```

## Save/Load

The inventory system includes basic save/load functionality:

```csharp
// Save
SerializableInventory saveData = InventorySystem.Instance.GetSaveData();
// Store saveData (integrate with SaveManager)

// Load
InventorySystem.Instance.LoadSaveData(saveData);
```

## Next Steps

1. **Phase 1.2**: Implement Crafting System (uses InventorySystem)
2. **Phase 1.3**: Implement Lobby with crafting station
3. Create Inventory UI for displaying items
4. Integrate with player pickup system
5. Integrate with loot drop system

## API Reference

### InventorySystem Methods

- `AddItem(ItemData item, int quantity)` - Add items to inventory
- `RemoveItem(ItemData item, int quantity)` - Remove items from inventory
- `HasItem(ItemData item, int quantity)` - Check if item exists with quantity
- `GetItemCount(ItemData item)` - Get quantity of specific item
- `GetAllItems()` - Get list of all inventory slots
- `GetItemsByType(ItemType type)` - Filter items by type
- `GetItemsByRarity(Rarity rarity)` - Filter items by rarity
- `ClearInventory()` - Remove all items
- `DebugPrintInventory()` - Print inventory to console

### ItemData Properties

- `ItemName` - Display name
- `Description` - Item description
- `Icon` - Sprite for UI
- `Type` - Weapon/Armor/Consumable/Material/Misc
- `ItemRarity` - Common/Uncommon/Rare/Epic/Legendary
- `IsStackable` - Can items stack?
- `MaxStackSize` - Maximum stack quantity
- `BaseDropRate` - Drop rate multiplier
- `Damage` - Weapon damage stat
- `Defense` - Armor defense stat
- `AttackSpeed` - Weapon attack speed
- `SpecialEffects` - List of effects (WIP)

## Troubleshooting

**InventorySystem instance is null**
- Make sure InventorySystem GameObject exists in the scene
- Check that it has the InventorySystem component attached
- Verify it's not a child of another GameObject (needs to be root for DontDestroyOnLoad)

**Items not persisting between scenes**
- InventorySystem uses DontDestroyOnLoad automatically
- Check that you're accessing InventorySystem.Instance, not a scene-specific reference

**Can't create ItemData assets**
- Make sure ItemData.cs is compiled without errors
- Look for "Game → Inventory → Item Data" in the Create menu
- Check Unity's Console for any compilation errors
