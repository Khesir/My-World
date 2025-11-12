# Prefab Organization - Quick Guide

Quick reference for where to put prefabs in the project.

---

## 📦 Where Do My Prefabs Go?

### Player Prefabs → `Assets/Features/Player/Prefab/`
```
✅ Player.prefab
✅ Player abilities/weapons
✅ Player VFX (dash trail, etc.)
```

### Enemy Prefabs → `Assets/Features/Enemies/Prefab/`
```
✅ Blue Slime.prefab
✅ Skeleton Warrior.prefab
✅ Any enemy characters
```

### UI Prefabs → Two options:

**Option 1: Feature-Specific**
```
Assets/Features/Inventory/UI/
├── InventoryPanel.prefab
├── ItemSlot.prefab
└── ItemTooltip.prefab

Assets/Features/Crafting/UI/
├── CraftingPanel.prefab
└── RecipeButton.prefab
```

**Option 2: Shared UI**
```
Assets/Art/UI/Prefabs/
├── CommonButton.prefab
├── ConfirmDialog.prefab
└── TooltipPanel.prefab
```

### Environment Prefabs → `Assets/Shared/Environment/`
```
✅ Tree.prefab
✅ Torche.prefab
✅ Rock.prefab
✅ Any decorative non-interactive objects
```

### Interactable Prefabs → `Assets/Features/Interactables/`
```
✅ Barrel.prefab
✅ Crate.prefab
✅ Chest.prefab
✅ Anything the player can interact with
```

### Manager Prefabs → `Assets/Core/Prefabs/`
```
✅ GameManager.prefab
✅ InventorySystem.prefab
✅ AudioManager.prefab
✅ Singleton managers
```

### Loot Prefabs → `Assets/Features/LootSystem/Prefabs/`
```
⏳ LootPickup_Common.prefab
⏳ LootPickup_Rare.prefab
⏳ LootPickup_Legendary.prefab
⏳ CoinPickup.prefab
```

### VFX Prefabs → Keep with related feature OR shared location

**Feature-Specific VFX**:
```
Assets/Features/Player/Animation/
└── Slash Prefab.prefab              ✅ Player attack effect

Assets/Features/Interactables/
└── Barrel VFX.prefab                ✅ Barrel destruction effect

Assets/Features/Enemies/Animation/
└── Slime Death VFX.prefab           ✅ Enemy death effect
```

**Shared VFX**:
```
Assets/Shared/Environment/VFX/
└── Twinkle.prefab                   ✅ Generic particle effect
```

---

## 🎯 Quick Decision Tree

**Is it a character?**
→ YES: `Features/{Character}/Prefab/`
→ NO: Continue...

**Is it UI?**
→ YES: `Features/{Feature}/UI/` or `Art/UI/Prefabs/`
→ NO: Continue...

**Can player interact with it?**
→ YES: `Features/Interactables/`
→ NO: Continue...

**Is it a manager/system?**
→ YES: `Core/Prefabs/`
→ NO: Continue...

**Is it decorative?**
→ YES: `Shared/Environment/`

---

## 📝 Naming Convention for Prefabs

### Good Names ✅
- `Player.prefab`
- `Blue Slime.prefab`
- `InventoryPanel.prefab`
- `LootPickup_Common.prefab`
- `Barrel_Explosive.prefab`

### Bad Names ❌
- `prefab1.prefab` (not descriptive)
- `player.prefab` (not PascalCase)
- `BARREL.prefab` (all caps)
- `New Prefab.prefab` (Unity default, rename it!)

---

## 🔄 Prefab Variants

Use prefab variants for similar objects:

**Base Prefab**:
```
Assets/Features/Enemies/Prefab/
└── Slime_Base.prefab
```

**Variants**:
```
Assets/Features/Enemies/Prefab/
├── Slime_Blue.prefab (variant of Slime_Base)
├── Slime_Red.prefab (variant of Slime_Base)
└── Slime_Boss.prefab (variant of Slime_Base)
```

This keeps shared logic in the base prefab!

---

## 🚀 Current Project Prefabs

### Already Organized ✅

**Player**:
- `Assets/Features/Player/Prefab/Player.prefab`
- `Assets/Features/Player/Animation/Slash Prefab.prefab`

**Enemies**:
- `Assets/Features/Enemies/Prefab/Blue Slime.prefab`

**Environment**:
- `Assets/Shared/Environment/Tree.prefab`
- `Assets/Shared/Environment/Tree 1.prefab`
- `Assets/Shared/Environment/Torche.prefab`

**Interactables**:
- `Assets/Features/Interactables/Barrel.prefab`
- `Assets/Features/Interactables/Crate.prefab`
- `Assets/Features/Interactables/Bush.prefab`

**Core**:
- `Assets/Core/Camera.prefab`

### To Be Created ⏳

**Managers** (Phase 1+):
- `Assets/Core/Prefabs/InventorySystem.prefab`
- `Assets/Core/Prefabs/GameManager.prefab`
- `Assets/Core/Prefabs/SaveManager.prefab`

**UI** (Phase 1+):
- `Assets/Features/Inventory/UI/InventoryPanel.prefab`
- `Assets/Features/Crafting/UI/CraftingPanel.prefab`
- `Assets/Features/LevelSelection/UI/LevelSelectionPanel.prefab`

**Loot** (Phase 3):
- `Assets/Features/LootSystem/Prefabs/LootPickup.prefab`

---

## 💡 Pro Tips

### 1. Organize Before It Gets Messy
Create folders BEFORE creating prefabs. Don't let prefabs pile up in the root!

### 2. Use Descriptive Names
Always rename "New Prefab" immediately after creating it.

### 3. Keep Related Assets Together
Prefab, script, sprite, and materials should live close together:
```
Assets/Features/Player/
├── Prefab/
│   └── Player.prefab
├── Player/
│   └── PlayerController.cs
├── Animation/
│   └── spr_player_idle.png
└── Slash.mat
```

### 4. Don't Nest Too Deep
```
❌ Assets/Game/Characters/Player/Prefabs/Variants/Combat/Melee/
✅ Assets/Features/Player/Prefab/
```

### 5. Use Prefab Variants
Don't duplicate entire prefabs for small changes. Use variants!

---

## 🔍 Finding Prefabs in Your Project

### By Type:
- **Characters**: Check `Features/{Character}/Prefab/`
- **UI**: Check `Features/{Feature}/UI/` or `Art/UI/Prefabs/`
- **Environment**: Check `Shared/Environment/`
- **Systems**: Check `Core/Prefabs/`

### By Feature:
- **Inventory**: `Features/Inventory/`
- **Combat**: `Features/Player/` and `Features/Enemies/`
- **Loot**: `Features/LootSystem/`

### Using Unity Search:
1. Click the search bar in Project window
2. Type: `t:prefab` (shows all prefabs)
3. Type: `t:prefab player` (shows prefabs with "player" in name)

---

## 📚 Related Guides

- **Full Structure**: See `PROJECT_STRUCTURE_GUIDE.md`
- **Art Assets**: See `ART_ORGANIZATION_GUIDE.md`
- **Inventory**: See `Features/Inventory/README_INVENTORY.md`

---

**Quick Summary**: Keep prefabs with their related feature, use descriptive names, and organize early!

**Last Updated**: 2025-11-13
