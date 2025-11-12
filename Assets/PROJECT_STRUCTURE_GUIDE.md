# Complete Project Structure Guide

This guide explains where to place ALL types of Unity assets: prefabs, scripts, animations, audio, materials, and more.

---

## Table of Contents

1. [Prefabs](#prefabs)
2. [Scripts](#scripts)
3. [Animations](#animations)
4. [Audio](#audio)
5. [Materials & Shaders](#materials--shaders)
6. [ScriptableObjects](#scriptableobjects)
7. [Scenes](#scenes)
8. [Resources Folder](#resources-folder)
9. [Quick Reference Table](#quick-reference-table)

---

## Prefabs

### General Rule
**Keep prefabs close to their related scripts and sprites**

### Character Prefabs

#### Player Prefabs
**Location**: `Assets/Features/Player/Prefab/`

```
Assets/Features/Player/
├── Prefab/
│   ├── Player.prefab                    ✅ EXISTING
│   ├── Player_Variant_Armored.prefab   (future variants)
│   └── Player_Variant_Mage.prefab
├── Animation/
│   └── Slash Prefab.prefab             ✅ EXISTING (VFX prefab)
└── Player/
    └── PlayerController.cs
```

**When to use**:
- Main player character prefab
- Player ability prefabs (weapon effects, shields)
- Player-specific VFX

#### Enemy Prefabs
**Location**: `Assets/Features/Enemies/Prefab/` or per-enemy folder

```
Assets/Features/Enemies/
├── Prefab/
│   └── Blue Slime.prefab               ✅ EXISTING
│
├── Blue Slime/                          (per-enemy organization)
│   ├── Blue Slime.prefab               (alternative location)
│   ├── spr_Blue_slime_idle.png
│   └── Blue Slime.controller
│
└── Skeleton Warrior/                    (NEW enemy example)
    ├── Skeleton Warrior.prefab
    ├── spr_skeleton_idle.png
    └── Skeleton Warrior.controller
```

**Recommendation**: Use per-enemy folders for complex enemies, shared Prefab folder for simple ones.

---

### Interactable Prefabs

**Location**: `Assets/Features/Interactables/`

```
Assets/Features/Interactables/
├── Barrel.prefab                       ✅ EXISTING
├── Barrel VFX.prefab                   ✅ EXISTING
├── Crate.prefab                        ✅ EXISTING
├── Crate VFX.prefab                    ✅ EXISTING
├── Bush.prefab                         ✅ EXISTING
├── Chest/
│   └── Chest.prefab                    (to be created)
└── Portal VFX.prefab                   ✅ EXISTING
```

**When to use**:
- Destructible objects (barrels, crates, bushes)
- Chests and loot containers
- Doors, gates, levers
- Any interactive object in the game world

---

### Environment Prefabs

**Location**: `Assets/Shared/Environment/`

```
Assets/Shared/Environment/
├── Tree.prefab                         ✅ EXISTING
├── Tree 1.prefab                       ✅ EXISTING
├── Torche.prefab                       ✅ EXISTING
├── VFX/
│   └── Twinkle.prefab                  ✅ EXISTING (particle effects)
└── Props/                              (to be created)
    ├── Rock.prefab
    ├── Fence.prefab
    └── Bridge.prefab
```

**When to use**:
- Decorative props (trees, rocks, torches)
- Non-interactive environment objects
- Reusable environment pieces

---

### UI Prefabs

**Location**: `Assets/Features/{FeatureName}/UI/` or `Assets/Art/UI/Prefabs/`

```
# Option 1: Feature-specific UI
Assets/Features/Inventory/
└── UI/
    ├── InventoryPanel.prefab
    ├── ItemSlot.prefab
    └── ItemTooltip.prefab

Assets/Features/Crafting/
└── UI/
    ├── CraftingPanel.prefab
    ├── RecipeButton.prefab
    └── CraftingSlot.prefab

# Option 2: Centralized UI (for shared UI elements)
Assets/Art/UI/
└── Prefabs/
    ├── CommonButton.prefab
    ├── ConfirmDialog.prefab
    └── TooltipPanel.prefab
```

**Recommendation**:
- Feature-specific UI → Keep with feature
- Reusable UI components → `Assets/Art/UI/Prefabs/`

---

### System Prefabs

**Location**: `Assets/Core/Prefabs/`

```
Assets/Core/
├── Prefabs/
│   ├── GameManager.prefab              (singleton managers)
│   ├── InventorySystem.prefab
│   ├── AudioManager.prefab
│   └── EventSystem.prefab              (Unity UI EventSystem)
└── Camera.prefab                       ✅ EXISTING
```

**When to use**:
- Singleton manager prefabs
- Core systems that appear in every scene
- Camera rigs

---

### Loot & Pickup Prefabs

**Location**: `Assets/Features/LootSystem/Prefabs/` (to be created)

```
Assets/Features/LootSystem/
├── Prefabs/
│   ├── LootPickup_Common.prefab
│   ├── LootPickup_Rare.prefab
│   ├── LootPickup_Legendary.prefab
│   └── CoinPickup.prefab
├── Sprites/
│   └── loot_pickup_icon.png
└── LootManager.cs
```

---

## Scripts

### General Rule
**Scripts live with their related feature or in Core for shared systems**

### Current Structure (Follow This Pattern)

```
Assets/
├── Core/Scripts/                       # Core systems & utilities
│   ├── Singleton.cs                    ✅ EXISTING
│   ├── BaseSingleton.cs                ✅ EXISTING
│   ├── SceneManagement.cs              ✅ EXISTING
│   └── CameraController.cs             ✅ EXISTING
│
├── Features/                           # Feature-specific scripts
│   ├── Player/Player/
│   │   ├── PlayerController.cs         ✅ EXISTING
│   │   ├── ActiveWeapon.cs             ✅ EXISTING
│   │   └── Sword.cs                    ✅ EXISTING
│   │
│   ├── Enemies/Enemies/
│   │   ├── EnemyAI.cs                  ✅ EXISTING
│   │   ├── EnemyPathfinding.cs         ✅ EXISTING
│   │   └── EnemyHealth.cs              ✅ EXISTING
│   │
│   ├── Inventory/
│   │   ├── InventorySystem.cs          ✅ NEW
│   │   ├── ItemData.cs                 ✅ NEW
│   │   └── InventorySlot.cs            ✅ NEW
│   │
│   └── SceneTransition/
│       ├── AreaExit.cs                 ✅ EXISTING
│       └── UIFade.cs                   ✅ EXISTING
│
└── Shared/Misc/                        # Reusable components
    ├── Knockback.cs                    ✅ EXISTING
    ├── Flash.cs                        ✅ EXISTING
    └── Destructible.cs                 ✅ EXISTING
```

### Script Organization Rules

1. **Manager Scripts** → `Assets/Core/Scripts/` or `Assets/Core/Managers/`
2. **Feature Scripts** → `Assets/Features/{FeatureName}/`
3. **Utility Scripts** → `Assets/Shared/Misc/` or `Assets/Core/Utilities/`
4. **UI Scripts** → With their feature or `Assets/Features/UI/`
5. **Editor Scripts** → `Assets/Editor/`

---

## Animations

### Animation Files Organization

```
Assets/Features/Player/
├── Player/
│   ├── Player.controller               ✅ Animator Controller
│   ├── Idle.anim                       ✅ Animation Clip
│   └── Running.anim                    ✅ Animation Clip
│
├── Animation/                          # Additional animations
│   └── Slash Prefab.prefab
│
└── Sword/
    ├── Sword.controller                ✅ Weapon controller
    ├── Entry.anim
    └── SwingDown.anim

Assets/Features/Enemies/Blue Slime/
├── Blue Slime.controller               ✅ Animator Controller
├── Idle.anim                           ✅ Animation Clip
└── spr_Blue_slime_idle.png            ✅ Sprite sheet
```

### Rules for Animations

1. **Animator Controller** → Same folder as the prefab it controls
2. **Animation Clips** → Same folder as Animator Controller
3. **Shared Animations** → `Assets/Shared/Animations/` (rare)

---

## Audio

### Audio Organization

```
Assets/Audio/                           # NEW - Create this
├── Music/
│   ├── Lobby/
│   │   └── lobby_theme.mp3
│   ├── Floor1/
│   │   └── dungeon_ambience.mp3
│   └── Combat/
│       └── battle_music.mp3
│
├── SFX/
│   ├── UI/
│   │   ├── button_click.wav
│   │   ├── item_pickup.wav
│   │   └── inventory_open.wav
│   │
│   ├── Combat/
│   │   ├── sword_slash.wav
│   │   ├── enemy_hit.wav
│   │   └── player_hurt.wav
│   │
│   ├── Environment/
│   │   ├── chest_open.wav
│   │   ├── door_open.wav
│   │   └── barrel_break.wav
│   │
│   └── Footsteps/
│       ├── grass_step_01.wav
│       └── stone_step_01.wav
│
└── Mixers/
    └── MainAudioMixer.mixer            # Unity Audio Mixer asset
```

### Audio Import Settings

- **Music**: MP3 or OGG, Streaming, Load in Background
- **Short SFX**: WAV, Decompress on Load
- **Voice**: OGG, Compressed in Memory

---

## Materials & Shaders

### Material Organization

```
Assets/Shared/Materials/                ✅ EXISTING
├── Glowshade.shadergraph              ✅ Shader Graph
├── WhiteFlash.mat                     ✅ Material
├── Torche.mat                         ✅ Material
└── Particle Material.mat              ✅ Material

Assets/Features/Player/
├── Slash.mat                          ✅ Player-specific material
└── Trail Renderer.mat                 ✅ Player-specific material
```

### Rules for Materials

1. **Shared Materials** → `Assets/Shared/Materials/`
2. **Feature-Specific Materials** → With the feature (e.g., player slash effect)
3. **Shader Graphs** → Same folder as materials that use them

---

## ScriptableObjects

### ScriptableObject Data Organization

```
Assets/Data/                            # All game data
├── Items/
│   ├── Weapons/
│   │   ├── Icons/                      (sprites)
│   │   ├── Iron Sword.asset            (ItemData)
│   │   └── Steel Axe.asset
│   ├── Armor/
│   │   ├── Icons/
│   │   └── Leather Helmet.asset
│   ├── Consumables/
│   │   └── Health Potion.asset
│   └── Materials/
│       └── Iron Ore.asset
│
├── Recipes/                            # CraftingRecipe ScriptableObjects
│   ├── Weapons/
│   │   └── Recipe_IronSword.asset
│   └── Consumables/
│       └── Recipe_HealthPotion.asset
│
├── Enemies/                            # EnemyData ScriptableObjects
│   ├── Floor1/
│   │   └── Enemy_BlueSlime.asset
│   └── Bosses/
│       └── Enemy_FloorBoss.asset
│
├── Floors/                             # FloorData ScriptableObjects
│   ├── Floor_1_Data.asset
│   ├── Floor_2_Data.asset
│   └── Floor_10_Data.asset
│
└── LootTables/                         # LootTable ScriptableObjects
    ├── Enemy_BlueSlime_LootTable.asset
    └── Chest_Common_LootTable.asset
```

### Rules for ScriptableObjects

1. **All game data** → `Assets/Data/{DataType}/`
2. **Keep related sprites nearby** → Use `Icons/` or `Sprites/` subfolders
3. **Organize by category** → Weapons, Armor, Floors, etc.
4. **Name clearly** → `{Type}_{Name}.asset` (e.g., `Recipe_IronSword.asset`)

---

## Scenes

### Scene Organization

```
Assets/Scenes/
├── Core/                               # NEW - Essential scenes
│   ├── Bootstrap.unity                 (loads managers)
│   └── MainMenu.unity                  (main menu)
│
├── Gameplay/                           # NEW - Gameplay scenes
│   ├── Lobby.unity                     (hub scene)
│   ├── LevelSelection.unity            (floor selection)
│   ├── Preparation.unity               (loadout config)
│   ├── Floor_1.unity                   (floor scenes)
│   ├── Floor_2.unity
│   └── Floor_Template.unity            (base template)
│
├── Testing/                            # NEW - Test scenes
│   ├── Testing Scene.unity             ✅ EXISTING
│   ├── InventoryTest.unity
│   └── CombatTest.unity
│
└── Legacy/                             # OLD - Legacy scenes
    ├── Scene1.unity                    ✅ EXISTING
    └── Scene2.unity                    ✅ EXISTING
```

### Scene Naming Convention

- **Core scenes**: `MainMenu.unity`, `Bootstrap.unity`
- **Gameplay scenes**: `{SceneName}.unity` (e.g., `Lobby.unity`)
- **Floor scenes**: `Floor_{Number}.unity` (e.g., `Floor_1.unity`)
- **Test scenes**: `{Feature}Test.unity` (e.g., `InventoryTest.unity`)

---

## Resources Folder

### What Goes in Resources

```
Assets/Resources/                       # Unity's special folder
├── Data/                               # For runtime loading of ScriptableObjects
│   └── Items/
│       └── Iron Sword.asset            (if needed at runtime by name)
│
├── Audio/                              # Audio loaded at runtime
│   └── UI/
│       └── button_click.wav
│
└── Prefabs/                            # Prefabs loaded at runtime
    └── RuntimeSpawnedPrefab.prefab
```

### Resources Folder Rules

⚠️ **Use sparingly!** Resources folder increases build time and memory usage.

**Only use for**:
- Assets loaded dynamically by name at runtime
- Save/Load system assets
- Addressables is preferred for large projects

**Don't use for**:
- Assets referenced in scenes (use direct references)
- Assets referenced in prefabs (use direct references)

---

## Quick Reference Table

| Asset Type | Primary Location | Alternative Location | Example |
|------------|------------------|---------------------|---------|
| **Player Prefab** | `Features/Player/Prefab/` | - | `Player.prefab` |
| **Enemy Prefab** | `Features/Enemies/Prefab/` | Per-enemy folder | `Blue Slime.prefab` |
| **UI Prefab** | `Features/{Feature}/UI/` | `Art/UI/Prefabs/` | `InventoryPanel.prefab` |
| **Environment Prefab** | `Shared/Environment/` | - | `Tree.prefab` |
| **Manager Prefab** | `Core/Prefabs/` | - | `InventorySystem.prefab` |
| **Item Icon** | `Data/Items/{Type}/Icons/` | - | `sword_icon.png` |
| **UI Art** | `Art/UI/` | - | `button_normal.png` |
| **Script (Feature)** | `Features/{Feature}/` | - | `InventorySystem.cs` |
| **Script (Core)** | `Core/Scripts/` | - | `Singleton.cs` |
| **Script (Utility)** | `Shared/Misc/` | `Core/Utilities/` | `Knockback.cs` |
| **Animation Controller** | With prefab | - | `Player.controller` |
| **Animation Clip** | With controller | - | `Idle.anim` |
| **Material** | `Shared/Materials/` | With feature | `WhiteFlash.mat` |
| **Shader** | `Shared/Materials/` | - | `Glowshade.shadergraph` |
| **ScriptableObject** | `Data/{Type}/` | - | `Iron Sword.asset` |
| **Audio (Music)** | `Audio/Music/{Scene}/` | - | `lobby_theme.mp3` |
| **Audio (SFX)** | `Audio/SFX/{Category}/` | - | `sword_slash.wav` |
| **Scene (Gameplay)** | `Scenes/Gameplay/` | - | `Lobby.unity` |
| **Scene (Test)** | `Scenes/Testing/` | - | `InventoryTest.unity` |

---

## Folder Creation Commands

If you want to create all recommended folders at once, create these:

### Essential Folders
```
Assets/Audio/Music/
Assets/Audio/SFX/UI/
Assets/Audio/SFX/Combat/
Assets/Audio/SFX/Environment/
Assets/Audio/Mixers/

Assets/Scenes/Core/
Assets/Scenes/Gameplay/
Assets/Scenes/Testing/
Assets/Scenes/Legacy/

Assets/Core/Managers/
Assets/Core/Utilities/

Assets/Features/UI/
Assets/Features/LootSystem/Prefabs/

Assets/Shared/Animations/
Assets/Shared/Environment/Props/

Assets/Resources/Data/
Assets/Resources/Prefabs/
```

---

## Best Practices

### Naming Conventions

1. **Prefabs**: PascalCase, descriptive
   - ✅ `PlayerCharacter.prefab`, `HealthPotion.prefab`
   - ❌ `player.prefab`, `prefab1.prefab`

2. **Scripts**: PascalCase, matches class name
   - ✅ `InventorySystem.cs`, `ItemData.cs`
   - ❌ `inventory_system.cs`, `item.cs`

3. **Scenes**: PascalCase or spaces
   - ✅ `MainMenu.unity`, `Testing Scene.unity`
   - ❌ `mainmenu.unity`, `test_scene_1.unity`

4. **Audio**: lowercase_snake_case
   - ✅ `sword_slash.wav`, `lobby_theme.mp3`
   - ❌ `SwordSlash.wav`, `LobbyTheme.mp3`

5. **Sprites**: snake_case with prefix
   - ✅ `spr_player_idle.png`, `icon_sword.png`
   - ❌ `PlayerIdle.png`, `sword.png`

### Avoid Deep Nesting

❌ **Too deep**:
```
Assets/Features/Player/Combat/Weapons/Melee/Swords/OneHanded/Scripts/
```

✅ **Better**:
```
Assets/Features/Player/Combat/
Assets/Data/Items/Weapons/
```

### Use Prefab Variants

For similar prefabs, use prefab variants:

```
Assets/Features/Enemies/Prefab/
├── Slime_Base.prefab           (base prefab)
├── Slime_Blue.prefab           (variant)
├── Slime_Red.prefab            (variant)
└── Slime_Boss.prefab           (variant)
```

---

## Migration Guide

If you already have assets scattered around:

### Step 1: Create New Folders
Run the folder creation commands above

### Step 2: Move Assets by Type

**Audio Assets**:
1. Find all `.mp3`, `.wav`, `.ogg` files
2. Move music → `Assets/Audio/Music/`
3. Move SFX → `Assets/Audio/SFX/{Category}/`

**UI Prefabs**:
1. Find all UI-related prefabs
2. Move to `Assets/Features/{Feature}/UI/` or `Assets/Art/UI/Prefabs/`

**Manager Prefabs**:
1. Find singleton manager prefabs
2. Move to `Assets/Core/Prefabs/`

### Step 3: Fix References
Unity will automatically update most references, but check:
- Scene references
- Prefab references
- Resources.Load() paths (if using Resources folder)

---

**Last Updated**: 2025-11-13
