# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a Unity 2D roguelike dungeon crawler with hub-based progression and crafting systems. Players explore increasingly dangerous floors, collect loot, craft items in a central lobby hub, and prepare loadouts before missions. The project uses Unity's Universal Render Pipeline (URP) and the new Input System.

**Core Gameplay Loop**: Lobby (craft/prepare) → Level Selection (choose floor) → Preparation (equip loadout) → Floor Exploration (combat/loot) → Results (success/death) → Return to Lobby

**Design Philosophy**: All progression comes from grinding and crafting - no traditional leveling. Deeper floors offer better loot but increased difficulty.

## Development Commands

### Opening the Project
- Open the project folder in Unity Hub (Unity LTS version recommended)
- The project uses Unity 2022.3 LTS or newer

### Building
- Build from Unity Editor: File > Build Settings > Build
- Build and Run: File > Build Settings > Build and Run

### Testing
- Play individual scenes from the Unity Editor
- Main test scenes are in `Assets/Scenes/`

## Code Architecture

### MVC-Style Folder Structure

The project follows a refactored MVC-inspired structure organized into three main categories:

```
Assets/
├── Core/                           # Persistent systems (DontDestroyOnLoad)
│   ├── Scripts/
│   │   ├── Singleton.cs           # Generic singleton pattern for managers
│   │   ├── BaseSingleton.cs       # Alternative singleton base class
│   │   ├── SceneManagement.cs     # Scene transitions & state persistence
│   │   └── CameraController.cs    # Camera follow and control
│   ├── Managers/                  # Core game managers (to be implemented)
│   │   ├── GameManager.cs         # 🚧 Overall game state
│   │   ├── SaveManager.cs         # 🚧 Save/load functionality
│   │   └── AudioManager.cs        # 🚧 Audio management
│   ├── Graphics/URP/              # Universal Render Pipeline settings
│   └── Camera.prefab              # Main camera prefab
│
├── Features/                       # Feature-specific gameplay systems
│   ├── Player/
│   │   ├── Player/
│   │   │   ├── PlayerController.cs        # Movement, input, dash (Singleton)
│   │   │   ├── Player Controls.inputactions  # Input System actions
│   │   │   ├── ActiveWeapon.cs            # Weapon management
│   │   │   ├── Sword.cs                   # Melee weapon logic
│   │   │   ├── DamageSource.cs            # Damage interface
│   │   │   └── SlashAnim.cs               # Attack animations
│   │   ├── Animation/             # Player animation clips & controllers
│   │   └── Prefab/                # Player prefab
│   │
│   ├── Enemies/
│   │   ├── Enemies/
│   │   │   ├── EnemyAI.cs                 # State machine AI (Roaming)
│   │   │   ├── EnemyPathfinding.cs        # Movement & pathfinding
│   │   │   └── EnemyHealth.cs             # Health system
│   │   ├── Animation/             # Enemy animations & VFX
│   │   ├── Prefab/                # Enemy prefabs (Blue Slime)
│   │   └── [Enemy sprites & controllers]
│   │
│   ├── SceneTransition/
│   │   ├── AreaExit.cs            # Trigger-based scene loading
│   │   ├── AreaEntrance.cs        # Player spawn positioning
│   │   └── UIFade.cs              # Screen fade transitions
│   │
│   ├── Interactables/             # Interactive objects
│   │   └── [Barrels, crates, bushes, chests prefabs]
│   │
│   ├── Inventory/                 # 🚧 TO BE IMPLEMENTED
│   │   ├── InventorySystem.cs     # Item storage & management (Singleton)
│   │   ├── Item.cs                # Item data class
│   │   └── ItemData.asset         # ScriptableObject items
│   │
│   ├── Crafting/                  # 🚧 TO BE IMPLEMENTED
│   │   ├── CraftingSystem.cs      # Recipe-based crafting (Singleton)
│   │   ├── CraftingStation.cs     # Lobby crafting interaction
│   │   └── RecipeData.asset       # ScriptableObject recipes
│   │
│   ├── Lobby/                     # 🚧 TO BE IMPLEMENTED
│   │   ├── LobbyManager.cs        # Lobby scene controller
│   │   └── LevelGate.cs           # Level selection trigger
│   │
│   ├── LevelSelection/            # 🚧 TO BE IMPLEMENTED
│   │   ├── LevelSelectionManager.cs
│   │   └── FloorData.asset        # ScriptableObject floor configs
│   │
│   ├── Preparation/               # 🚧 TO BE IMPLEMENTED
│   │   ├── LoadoutManager.cs      # Equipment & loadout system
│   │   └── PreparationManager.cs  # Prep scene controller
│   │
│   ├── LootSystem/                # 🚧 TO BE IMPLEMENTED
│   │   ├── LootManager.cs         # Loot generation & drops
│   │   ├── LootTable.asset        # ScriptableObject loot tables
│   │   └── LootPickup.cs          # Pickup objects
│   │
│   └── Progression/               # 🚧 TO BE IMPLEMENTED
│       └── ProgressionManager.cs  # Stats & unlocks tracking
│
├── Shared/                         # Reusable components & utilities
│   ├── Misc/
│   │   ├── Knockback.cs           # Physics-based knockback system
│   │   ├── Flash.cs               # Damage visual feedback
│   │   ├── Destructible.cs        # Breakable object component
│   │   ├── Parallax.cs            # Parallax scrolling
│   │   └── TransparentDetection.cs
│   ├── Environment/               # Environment prefabs & VFX
│   ├── Tilemap/                   # Tiles, rule tiles, animated tiles
│   ├── Materials/                 # Shaders (Glowshade.shadergraph)
│   └── Rocks/                     # Rock sprites & assets
│
├── Scenes/
│   ├── Lobby.unity                # 🚧 Main hub scene (craft/prepare)
│   ├── LevelSelection.unity       # 🚧 Floor selection UI
│   ├── Preparation.unity          # 🚧 Loadout configuration
│   ├── Floor_Template.unity       # 🚧 Base floor template
│   ├── Scene1.unity               # Legacy: Connected test scene
│   ├── Scene2.unity               # Legacy: Connected test scene
│   └── Testing Scene.unity        # Development testing
│
├── Settings/                       # Unity project settings
│   ├── UniversalRP.asset          # URP renderer settings
│   └── Renderer2D.asset           # 2D renderer config
│
└── Data/                           # 🚧 ScriptableObject data (to be organized)
    ├── Items/
    ├── Enemies/
    ├── Floors/
    ├── Recipes/
    └── LootTables/
```

**Legend**:
- ✅ Implemented and functional
- 🚧 Planned/To be implemented
- No indicator: Asset folders (sprites, prefabs, etc.)

### Key Design Patterns

**Singleton Pattern**
- Core managers (PlayerController, SceneManagement) use Singleton<T>
- Singletons persist with DontDestroyOnLoad
- Access via `ClassName.Instance`

**Component-Based Architecture**
- Systems are modular components attached to GameObjects
- Shared components (Knockback, Flash, Destructible) can be added to any GameObject
- Feature-specific components are organized in their respective folders

**State Management**
- EnemyAI uses enum-based state machine pattern
- Scene transitions maintain state through SceneManagement singleton

**Input System**
- Uses Unity's new Input System package
- Input actions defined in `Player Controls.inputactions`
- PlayerControls class is auto-generated from input actions

### Gameplay Loop & Scene Flow

**Full Game Loop** (Target Implementation):
1. **Lobby** → Player free-roams, accesses inventory, crafts items at CraftingStation
2. **Level Gate** → Player interacts with LevelGate trigger
3. **Level Selection** → LevelSelectionManager displays FloorData, player selects floor depth
4. **Preparation** → LoadoutManager allows equipment configuration, PreparationManager shows briefing
5. **Floor Gameplay** → Player completes tasks, fights enemies, collects loot
6. **Results** → Success (keep loot) or Death (lose items), ProgressionManager updates stats
7. **Return to Lobby** → Repeat loop with new resources

**Current Scene Transition Flow** (Legacy):
1. Player enters AreaExit trigger
2. SceneManagement stores transition name
3. UIFade fades screen to black
4. New scene loads via UnityEngine.SceneManager
5. AreaEntrance positions player at corresponding entrance point

### Planned Systems (See REQUIREMENTS.md for details)

**Phase 1: Core Loop Foundation**
- InventorySystem: Item storage with metadata (rarity, stats, quantity)
- CraftingSystem: Recipe-based crafting with material requirements
- LobbyManager: Hub scene with crafting stations and level gate

**Phase 2: Mission Flow**
- LevelSelectionManager: Floor depth selection with difficulty preview
- PreparationManager: Loadout configuration scene
- TaskManager: Mission objectives (kill, gather, survive, explore)

**Phase 3: Progression & Loot**
- LootManager: Drop tables with rarity and floor-depth scaling
- FloorGenerator: Enemy spawning and layout management
- ProgressionManager: Grind-based progression tracking (no XP/levels)

**Phase 4: Polish**
- SaveManager: Persistent inventory, progression, and settings
- AudioManager: Music, SFX, and audio mixing
- UI/UX: Comprehensive UI for all systems

### Physics and Combat

- 2D physics using Rigidbody2D
- Knockback applies impulse force based on direction and mass
- DamageSource interface for any damage-dealing object
- Enemies and destructibles respond to damage with health systems

### Data-Driven Design with ScriptableObjects

All game data is defined using ScriptableObjects for easy modification and balancing:

**ItemData.asset**
- Weapon stats (damage, attack speed)
- Armor stats (defense, resistances)
- Consumable effects
- Material info for crafting
- Rarity and drop rates

**RecipeData.asset**
- Result item and quantity
- Required materials and quantities
- Crafting time (if any)
- Unlock conditions

**FloorData.asset**
- Floor depth and difficulty
- Enemy types and spawn counts
- Loot tier and drop rate modifiers
- Scene reference and environment settings

**LootTable.asset**
- Weighted loot entries
- Per-enemy or per-chest configurations
- Floor-depth scaling factors

**EnemyData.asset**
- Health, damage, movement speed
- AI behavior parameters
- Loot table reference
- Sprite and animation controller

All ScriptableObjects should be organized in `Assets/Data/` with subfolders by type.

## Important Conventions

### Naming
- C# scripts use PascalCase
- Prefabs use descriptive names (e.g., "Blue Slime.prefab")
- Scene-specific assets in subfolders under Scenes/

### Serialization
- Use `[SerializeField]` for Unity Inspector fields
- Private fields exposed to Inspector rather than public fields
- ScriptableObjects for data-driven design (mentioned in README)

### Animation
- Animator controllers stored with related prefabs
- Animation clips organized by feature/character
- Sprite-based animations for 2D characters

### Scene Organization
- Testing Scene: General prototyping
- Scene1 and Scene2: Connected areas with transitions
- Each scene can have subfolder for scene-specific assets

## Unity Packages Used

- Universal Render Pipeline (URP) 14.0.11
- Shader Graph 14.0.11
- 2D Sprite package
- Unity Input System (new input system)

## Common Gotchas

- PlayerController uses Unity's new Input System - old Input Manager won't work
- Singletons must have no parent transform to persist with DontDestroyOnLoad
- Scene transitions require both AreaExit and AreaEntrance with matching transition names
- The project uses 2D URP - lighting and materials must be compatible with 2D Renderer
- InventorySystem and other core managers should persist across scenes with DontDestroyOnLoad
- All game data (items, enemies, floors) use ScriptableObjects - never hardcode stats in scripts
- Floor depth affects drop rates - higher floor = better loot multiplier
- Death penalty means items can be lost - check FloorData for death rules per floor
- Crafting consumes materials from inventory - validate quantity before crafting
- Loadout is separate from inventory - equipped items are still "owned" by inventory

## Implementation Guidelines

When implementing new systems:
1. Create ScriptableObject data types first (ItemData, RecipeData, etc.)
2. Implement Singleton manager for the system (if needs global access)
3. Add system initialization in appropriate scene manager
4. Hook up UnityEvents for loose coupling between systems
5. Test serialization if system needs saving/loading
6. Update UI to reflect system state changes

See [REQUIREMENTS.md](./REQUIREMENTS.md) for detailed technical specifications, data structures, and implementation roadmap.

## Current Development Phase

**Phase 1: Core Loop Foundation** (In Progress)
- Focus: Inventory, Crafting, and Lobby systems
- Next Milestone: Functional lobby with working crafting station
