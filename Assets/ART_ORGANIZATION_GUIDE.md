# Art Asset Organization Guide

This guide explains where to place different types of art assets in the project.

---

## Quick Reference

| Asset Type | Location | Example |
|------------|----------|---------|
| **Item Icons** | `Assets/Data/Items/{Type}/Icons/` | `Data/Items/Weapons/Icons/sword_icon.png` |
| **UI Elements** | `Assets/Art/UI/` | `Art/UI/Buttons/confirm_button.png` |
| **Player Sprites** | `Assets/Features/Player/Animation/` | `Features/Player/Animation/player_idle.png` |
| **Enemy Sprites** | `Assets/Features/Enemies/{EnemyName}/` | `Features/Enemies/Blue Slime/spr_slime_idle.png` |
| **Environment** | `Assets/Shared/Environment/` or `Assets/Shared/Tilemap/` | `Shared/Tilemap/plains.png` |
| **VFX Particles** | `Assets/Art/VFX/` or feature-specific | `Art/VFX/Particles/smoke_particle.png` |

---

## Detailed Structure

### 1. Item Icons (for ItemData ScriptableObjects)

**Location**: `Assets/Data/Items/{ItemType}/Icons/`

Items use ScriptableObjects for data, and their icons should be close to the data files.

```
Assets/Data/Items/
├── Weapons/
│   ├── Icons/
│   │   ├── iron_sword_icon.png       (32x32 or 64x64)
│   │   ├── steel_axe_icon.png
│   │   └── magic_staff_icon.png
│   ├── Iron Sword.asset               (ItemData)
│   └── Steel Axe.asset
│
├── Armor/
│   ├── Icons/
│   │   ├── leather_helmet_icon.png
│   │   └── iron_chestplate_icon.png
│   └── Leather Helmet.asset
│
├── Consumables/
│   ├── Icons/
│   │   ├── health_potion_icon.png
│   │   └── mana_potion_icon.png
│   └── Health Potion.asset
│
└── Materials/
    ├── Icons/
    │   ├── iron_ore_icon.png
    │   ├── wood_icon.png
    │   └── leather_icon.png
    └── Iron Ore.asset
```

**When to use**:
- Item icons for inventory UI
- Crafting recipe icons
- Loot drop pickups

**Recommended size**: 64x64 or 128x128 pixels

---

### 2. UI Art

**Location**: `Assets/Art/UI/`

All UI-related graphics that aren't tied to specific game data.

```
Assets/Art/UI/
├── Icons/
│   ├── settings_icon.png
│   ├── close_icon.png
│   └── checkmark_icon.png
│
├── Buttons/
│   ├── button_normal.png
│   ├── button_hover.png
│   └── button_pressed.png
│
├── Panels/
│   ├── panel_background.png
│   ├── tooltip_background.png
│   └── inventory_panel.png
│
└── HUD/
    ├── health_bar.png
    ├── health_bar_fill.png
    └── taskbar_background.png
```

**When to use**:
- Main menu UI
- HUD elements
- Inventory UI backgrounds
- Buttons and controls
- General UI icons

---

### 3. Character Art (Player & Enemies)

#### Player Sprites

**Location**: `Assets/Features/Player/Animation/`

```
Assets/Features/Player/Animation/
├── Side animations/
│   ├── spr_player_right_idle.png      (sprite sheet)
│   ├── spr_player_right_walk.png      (sprite sheet)
│   └── spr_player_right_attack.png    (sprite sheet)
├── Idle.anim                           (animation clip)
└── Running.anim
```

**Existing**: Player sprites are already organized here. Continue this pattern.

#### Enemy Sprites

**Location**: `Assets/Features/Enemies/{EnemyName}/`

```
Assets/Features/Enemies/
├── Blue Slime/
│   ├── spr_Blue_slime_idle.png
│   ├── spr_Blue_slime_walk.png
│   ├── spr_Blue_slime_attack.png
│   └── Blue Slime.controller
│
└── Skeleton Warrior/                   (NEW enemy example)
    ├── spr_skeleton_idle.png
    ├── spr_skeleton_walk.png
    ├── spr_skeleton_attack.png
    └── Skeleton Warrior.controller
```

**Existing**: Enemies follow this pattern. Each enemy has its own subfolder.

---

### 4. Environment & World Art

#### Props & Interactables

**Location**: `Assets/Features/Interactables/` or `Assets/Shared/Environment/`

```
Assets/Features/Interactables/
├── Barrels and crates/
│   ├── spr_barrel1.png
│   └── spr_crate1.png
│
└── Chest/
    └── spr_chest.png

Assets/Shared/Environment/
├── tree.png
├── tree1.png
└── Torche.png
```

**Existing**: Props are split between Features and Shared. Continue this pattern.

#### Tilemaps

**Location**: `Assets/Shared/Tilemap/`

```
Assets/Shared/Tilemap/
├── plains.png           (tileset sprite sheet)
├── grass.png
├── decor_16x16.png
└── Water Tiles.png
```

**Existing**: Tilemap assets go here.

---

### 5. Visual Effects (VFX)

**Location**: `Assets/Art/VFX/` or feature-specific folders

```
Assets/Art/VFX/
├── Particles/
│   ├── smoke_particle.png
│   ├── spark_particle.png
│   └── blood_particle.png
│
└── Effects/
    ├── explosion_spritesheet.png
    ├── slash_effect.png            (can also be in Features/Player/)
    └── hit_flash.png

# Alternative: Feature-specific VFX
Assets/Features/Player/
└── Slash Effect.png                (EXISTING - weapon effects)

Assets/Features/Enemies/Animation/
└── Slime Death VFX.prefab          (EXISTING - enemy death effects)

Assets/Shared/Environment/VFX/
└── Particle Sprite.png             (EXISTING - environment particles)
```

**When to use**:
- Generic VFX → `Assets/Art/VFX/`
- Feature-specific VFX → Keep with feature (e.g., player attack effects)

---

### 6. Loot & Pickup Sprites

**Location**: `Assets/Features/LootSystem/Sprites/` (to be created)

When you implement the loot system, create:

```
Assets/Features/LootSystem/
├── Sprites/
│   ├── loot_pickup_common.png
│   ├── loot_pickup_rare.png
│   └── loot_pickup_legendary.png
└── LootPickup.cs
```

---

## Icon Import Settings

For UI icons and item icons:

1. **Texture Type**: Sprite (2D and UI)
2. **Pixels Per Unit**: 100 (default) or 64/32 depending on your project
3. **Filter Mode**: Point (no filter) for pixel art, or Bilinear for smooth art
4. **Compression**: None or Low Quality (for crisp icons)
5. **Max Size**: 256 or 512 (icons don't need to be huge)
6. **Alpha Is Transparency**: Checked

### For Item Icons Specifically:
- Use consistent sizes (64x64 or 128x128 recommended)
- Keep alpha channel for transparency
- Use sprite packing for better performance

---

## Tips

### Naming Conventions

- **Item Icons**: `{item_name}_icon.png`
  - Example: `iron_sword_icon.png`, `health_potion_icon.png`

- **UI Elements**: `{element}_{state}.png`
  - Example: `button_normal.png`, `button_hover.png`

- **Sprite Sheets**: `spr_{character}_{animation}.png`
  - Example: `spr_player_idle.png`, `spr_slime_attack.png`

- **VFX**: `{effect_name}_particle.png` or `{effect_name}_effect.png`
  - Example: `smoke_particle.png`, `explosion_effect.png`

### File Formats

- **PNG**: For sprites with transparency (UI, characters, items)
- **JPG**: For backgrounds without transparency (rare in 2D games)
- **PSD**: Source files (keep outside Assets folder or in separate project)

---

## Current Project Structure Reference

```
Assets/
├── Data/                    # ← ScriptableObjects + their icons
│   └── Items/
│       ├── Weapons/Icons/
│       ├── Armor/Icons/
│       ├── Consumables/Icons/
│       └── Materials/Icons/
│
├── Art/                     # ← NEW - Centralized art for UI & shared VFX
│   ├── UI/
│   └── VFX/
│
├── Features/                # ← Feature-specific sprites
│   ├── Player/Animation/   # Player sprite sheets
│   ├── Enemies/            # Enemy sprite sheets (per enemy folder)
│   ├── Interactables/      # Barrel, crate, chest sprites
│   └── LootSystem/         # (Future) Loot pickup sprites
│
└── Shared/                  # ← Shared/reusable sprites
    ├── Environment/         # Trees, torches, decorations
    ├── Tilemap/            # Tileset sprite sheets
    └── Materials/          # Shader graphs
```

---

## Migration Notes

If you already have art assets:

1. **Item icons** → Move to `Assets/Data/Items/{Type}/Icons/`
2. **UI sprites** → Move to `Assets/Art/UI/`
3. **Character sprites** → Keep in `Assets/Features/{Character}/`
4. **VFX sprites** → Move generic ones to `Assets/Art/VFX/`, keep specific ones with features

---

## Questions?

- **"Where do crafting result icons go?"** → Same as item icons, in `Data/Items/`
- **"Where do floor/scene background sprites go?"** → `Assets/Shared/Environment/` or scene-specific subfolder
- **"Where do quest/task icons go?"** → `Assets/Art/UI/Icons/` if generic, or with TaskSystem if specific
- **"Where do enemy loot table icons go?"** → They reference item icons, so same location

---

**Last Updated**: 2025-11-13
