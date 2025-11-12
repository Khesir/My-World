# 🎮 Roguelike Dungeon Crawler - Unity Game Systems

A 2D roguelike dungeon crawler with hub-based progression, crafting systems, and procedurally challenging floors. Built with Unity URP as a modular game development showcase.

## 🎯 Game Overview

A roguelike action game where players explore increasingly dangerous floors, collect loot, craft items, and prepare for deeper challenges. The game features a central lobby hub for progression and preparation, with randomly generated or pre-designed floors offering escalating difficulty and rewards.

---

## 🔄 Core Gameplay Loop

```
┌─────────────────────────────────────────────────────────────┐
│                                                               │
│  LOBBY (Hub)                                                  │
│  ├─ Free roam in safe area                                   │
│  ├─ Access inventory and crafting station                    │
│  ├─ Craft items from collected materials                     │
│  ├─ View player stats and progression                        │
│  └─ Approach gate to start level selection                   │
│                                                               │
└──────────────────┬────────────────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────────────┐
│                                                               │
│  LEVEL SELECTION                                              │
│  ├─ Choose floor depth (lower = harder + better loot)        │
│  ├─ View floor info: difficulty, enemy types, rewards        │
│  └─ Confirm selection                                         │
│                                                               │
└──────────────────┬────────────────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────────────┐
│                                                               │
│  PREPARATION SCENE                                            │
│  ├─ Equip weapons and armor                                  │
│  ├─ Select consumables to bring                              │
│  ├─ Configure loadout from lobby-crafted items               │
│  ├─ Review floor objectives and challenges                   │
│  └─ Begin mission                                             │
│                                                               │
└──────────────────┬────────────────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────────────┐
│                                                               │
│  GAMEPLAY (Floor Exploration)                                 │
│  ├─ Complete tasks:                                           │
│  │  ├─ Gather resources and materials                        │
│  │  ├─ Kill specific enemy types or quantities               │
│  │  ├─ Find rare items or treasures                          │
│  │  └─ Survive for duration or reach exit                    │
│  ├─ Combat enemies (all improvements from drops)             │
│  ├─ Collect loot from defeated enemies and chests            │
│  └─ Exit floor (success or death)                            │
│                                                               │
└──────────────────┬────────────────────────────────────────────┘
                   │
                   ▼
          ┌────────┴─────────┐
          │                  │
     SUCCESS              DEATH
          │                  │
          ▼                  ▼
   ┌──────────────┐   ┌──────────────┐
   │ Keep Loot    │   │ Lose Items   │
   │ Return to    │   │ Return to    │
   │ Lobby        │   │ Lobby        │
   └──────┬───────┘   └──────┬───────┘
          │                  │
          └────────┬─────────┘
                   │
                   ▼
              [BACK TO LOBBY]
```

---

## 🏗️ Project Structure

```
Assets/
├── Core/                          # Core systems and managers
│   ├── Scripts/
│   │   ├── Singleton.cs          # Generic singleton pattern
│   │   ├── SceneManagement.cs    # Scene flow controller
│   │   └── CameraController.cs   # Camera follow system
│   ├── Managers/                 # Game managers (to be implemented)
│   └── Graphics/URP/             # Universal Render Pipeline settings
│
├── Features/                      # Feature-specific gameplay systems
│   ├── Player/
│   │   ├── Player/
│   │   │   ├── PlayerController.cs      # Movement & input
│   │   │   ├── ActiveWeapon.cs          # Weapon management
│   │   │   ├── Sword.cs                 # Melee weapon logic
│   │   │   └── Player Controls.inputactions
│   │   ├── Animation/           # Player animations
│   │   └── Prefab/              # Player prefab
│   │
│   ├── Enemies/
│   │   ├── Enemies/
│   │   │   ├── EnemyAI.cs              # AI state machine
│   │   │   ├── EnemyPathfinding.cs     # Movement logic
│   │   │   └── EnemyHealth.cs          # Health system
│   │   ├── Prefab/              # Enemy prefabs
│   │   └── Animation/           # Enemy animations
│   │
│   ├── SceneTransition/
│   │   ├── AreaExit.cs          # Scene exit triggers
│   │   ├── AreaEntrance.cs      # Spawn point management
│   │   └── UIFade.cs            # Screen transitions
│   │
│   ├── Interactables/           # Interactive objects
│   │   └── [Barrels, crates, chests, etc.]
│   │
│   ├── Lobby/                   # 🚧 TO BE IMPLEMENTED
│   │   ├── LobbyManager.cs
│   │   ├── CraftingStation.cs
│   │   └── LevelGate.cs
│   │
│   ├── LevelSelection/          # 🚧 TO BE IMPLEMENTED
│   │   ├── LevelSelector.cs
│   │   └── FloorData.cs
│   │
│   ├── Preparation/             # 🚧 TO BE IMPLEMENTED
│   │   ├── LoadoutManager.cs
│   │   └── EquipmentSlot.cs
│   │
│   ├── Inventory/               # 🚧 TO BE IMPLEMENTED
│   │   ├── InventorySystem.cs
│   │   ├── Item.cs
│   │   └── Crafting/
│   │
│   └── LootSystem/              # 🚧 TO BE IMPLEMENTED
│       ├── LootTable.cs
│       └── DropManager.cs
│
├── Shared/                       # Reusable components
│   ├── Misc/
│   │   ├── Knockback.cs         # Physics knockback
│   │   ├── Flash.cs             # Damage feedback
│   │   ├── Destructible.cs      # Breakable objects
│   │   └── TransparentDetection.cs
│   ├── Environment/             # Environment prefabs & VFX
│   ├── Tilemap/                 # Tiles and rule tiles
│   └── Materials/               # Shaders and materials
│
├── Scenes/
│   ├── Lobby.unity              # 🚧 Main hub scene
│   ├── LevelSelection.unity     # 🚧 Floor selection UI
│   ├── Preparation.unity        # 🚧 Loadout configuration
│   ├── Floor_Template.unity     # 🚧 Base floor template
│   ├── Scene1.unity             # Legacy test scene
│   ├── Scene2.unity             # Legacy test scene
│   └── Testing Scene.unity      # Development testing
│
└── Settings/                     # Unity project settings
    ├── UniversalRP.asset
    └── Renderer2D.asset
```

---

## 🎮 Game Systems

### ✅ Currently Implemented

- **Player Movement**: WASD movement with mouse-aim direction
- **Combat System**: Melee attacks with knockback physics
- **Damage System**: Health, damage sources, and visual feedback
- **Scene Transitions**: Fade effects and persistent scene state
- **Enemy AI**: Basic roaming state machine
- **Destructibles**: Breakable objects (barrels, crates, bushes)

### 🚧 To Be Implemented

#### Phase 1: Core Loop Foundation
- [ ] **Lobby System**
  - Free-roam hub area
  - Crafting station interaction
  - Level gate access point
  - Player progression display

- [ ] **Inventory System**
  - Item storage and management
  - Material collection tracking
  - Equipment slots
  - Item metadata (rarity, stats, type)

- [ ] **Crafting System**
  - Recipe definitions (ScriptableObjects)
  - Material requirements
  - Crafting UI and interactions
  - Crafted item persistence

#### Phase 2: Mission Flow
- [ ] **Level Selection System**
  - Floor depth selection (1-10+)
  - Difficulty scaling based on depth
  - Floor preview: enemies, rewards, modifiers
  - Risk/reward visibility

- [ ] **Preparation Scene**
  - Loadout configuration UI
  - Equipment management (weapons, armor, consumables)
  - Loadout saving/loading
  - Mission briefing display

- [ ] **Task System**
  - Kill X enemies objectives
  - Gather X resources objectives
  - Survive duration objectives
  - Exploration/discovery objectives
  - Task completion tracking

#### Phase 3: Progression & Loot
- [ ] **Loot System**
  - Loot tables per enemy type
  - Rarity system (common → legendary)
  - Drop rate calculation based on floor depth
  - Loot pickup and collection

- [ ] **Floor Generation**
  - Procedural or hand-crafted floor layouts
  - Enemy spawning based on floor depth
  - Resource node placement
  - Exit/goal placement

- [ ] **Progression System**
  - All upgrades from grinding/drops (no exp/levels)
  - Permanent unlock tracking
  - Death penalty system
  - Success/failure rewards

#### Phase 4: Polish & Expansion
- [ ] **Save System**
  - Persistent inventory
  - Crafted items saved
  - Progression state
  - Settings and preferences

- [ ] **UI/UX**
  - Inventory UI
  - Crafting UI
  - Level selection UI
  - HUD (health, task progress)
  - Death/victory screens

- [ ] **Audio**
  - Combat sounds
  - UI feedback sounds
  - Ambient music per scene
  - Dynamic audio mixing

---

## 🛠️ Tech Stack

- **Engine**: Unity 2022.3 LTS
- **Render Pipeline**: Universal Render Pipeline (URP) 14.0.11
- **Input**: Unity Input System (new)
- **Language**: C# (.NET Standard 2.1)
- **Patterns**: Singleton, Component-based, ScriptableObject data
- **Version Control**: Git

---

## 🚀 Getting Started

### Prerequisites
- Unity 2022.3 LTS or newer
- Visual Studio 2022 or JetBrains Rider

### Opening the Project
1. Clone the repository
   ```bash
   git clone https://github.com/khesir/Game-Systems-library.git
   ```
2. Open Unity Hub
3. Click "Add" and select the project folder
4. Open the project with Unity 2022.3 LTS

### Building
- **Editor Play**: Open any scene and press Play
- **Build**: File → Build Settings → Build
- **Platform**: Currently configured for PC/Mac/Linux

### Testing
- **Lobby Testing**: Open `Lobby.unity` (when implemented)
- **Combat Testing**: Open `Testing Scene.unity`
- **Scene Flow**: Test Scene1 → Scene2 transitions

---

## 📋 Development Roadmap

See [REQUIREMENTS.md](./REQUIREMENTS.md) for detailed technical specifications and implementation requirements.

**Current Phase**: Phase 1 - Core Loop Foundation
**Next Milestone**: Lobby System with Crafting

---

## 🎨 Design Philosophy

- **Grind-Based Progression**: All improvements come from drops and crafting, no traditional leveling
- **Risk/Reward**: Deeper floors = harder enemies but better loot
- **Preparation Matters**: Time spent in lobby crafting and preparing loadouts affects success
- **Modular Systems**: Each system is independent and reusable
- **Data-Driven**: ScriptableObjects for items, recipes, enemies, and floors

---

## 📄 License

This project is released under the [MIT License](./LICENSE).
Feel free to use or modify these systems in your own personal or commercial projects.

---

## 🙋‍♂️ About the Author

Created by **Khesir** — software engineer, backend developer, and game developer.
Exploring game development systems and roguelike design patterns.

🔗 [github.com/khesir](https://github.com/khesir)

---

## 📚 Additional Documentation

- [CLAUDE.md](./CLAUDE.md) - AI assistant guidance for codebase
- [REQUIREMENTS.md](./REQUIREMENTS.md) - Technical specifications (to be created)
