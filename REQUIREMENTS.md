# Technical Requirements & Specifications

This document outlines the technical requirements and implementation details for the Roguelike Dungeon Crawler game systems.

---

## 📋 Table of Contents

1. [System Overview](#system-overview)
2. [Phase 1: Core Loop Foundation](#phase-1-core-loop-foundation)
3. [Phase 2: Mission Flow](#phase-2-mission-flow)
4. [Phase 3: Progression & Loot](#phase-3-progression--loot)
5. [Phase 4: Polish & Expansion](#phase-4-polish--expansion)
6. [Data Structures](#data-structures)
7. [Technical Architecture](#technical-architecture)
8. [Dependencies](#dependencies)
9. [Testing Requirements](#testing-requirements)

---

## System Overview

### Architecture Principles
- **Data-Driven Design**: Use ScriptableObjects for all game data (items, enemies, floors, recipes)
- **Component-Based**: Systems are modular MonoBehaviour components
- **Singleton Managers**: Core managers use Singleton pattern for global access
- **Event-Driven**: Use UnityEvents or C# events for loose coupling
- **Separation of Concerns**: Clear distinction between data, logic, and presentation

### Performance Targets
- **Target FPS**: 60 FPS on mid-range hardware
- **Scene Load Time**: < 3 seconds per scene
- **Inventory Operations**: < 16ms per frame
- **Enemy Count**: Support 20+ simultaneous enemies

---

## Phase 1: Core Loop Foundation

### 1.1 Inventory System

#### Requirements
- Store unlimited items (with practical UI pagination)
- Track item quantity, metadata, and equipment status
- Support multiple item types: weapons, armor, consumables, materials
- Persist between scenes and game sessions
- Notify UI when inventory changes

#### Components

**`InventorySystem.cs`** (Singleton)
```csharp
// Core Methods
- AddItem(ItemData item, int quantity) → bool
- RemoveItem(ItemData item, int quantity) → bool
- HasItem(ItemData item, int quantity) → bool
- GetItemCount(ItemData item) → int
- GetAllItems() → List<InventorySlot>
- ClearInventory()

// Events
- OnInventoryChanged → UnityEvent<InventorySlot>
- OnItemAdded → UnityEvent<ItemData, int>
- OnItemRemoved → UnityEvent<ItemData, int>

// Properties
- MaxInventorySlots → int (for UI display only)
- Items → Dictionary<ItemData, InventorySlot>
```

**`InventorySlot.cs`** (Data Class)
```csharp
// Properties
- ItemData → ItemData (ScriptableObject reference)
- Quantity → int
- IsEquipped → bool
- SlotIndex → int (for UI)
```

**`ItemData.cs`** (ScriptableObject)
```csharp
// Properties
- ItemName → string
- Description → string
- Icon → Sprite
- ItemType → enum (Weapon, Armor, Consumable, Material, Misc)
- Rarity → enum (Common, Uncommon, Rare, Epic, Legendary)
- IsStackable → bool
- MaxStackSize → int
- DropRate → float (base drop rate)

// Stats (for equipment)
- Damage → float
- Defense → float
- AttackSpeed → float
- SpecialEffects → List<EffectData>
```

#### Dependencies
- SaveSystem (for persistence)
- UI System (for inventory display)

---

### 1.2 Crafting System

#### Requirements
- Recipe-based crafting with material requirements
- Check if player has required materials
- Consume materials on successful craft
- Add crafted item to inventory
- Display available recipes
- Show locked/unlocked recipes

#### Components

**`CraftingSystem.cs`** (Singleton)
```csharp
// Core Methods
- CanCraft(RecipeData recipe) → bool
- CraftItem(RecipeData recipe) → bool
- GetAvailableRecipes() → List<RecipeData>
- IsRecipeUnlocked(RecipeData recipe) → bool
- UnlockRecipe(RecipeData recipe)

// Events
- OnItemCrafted → UnityEvent<ItemData>
- OnRecipeUnlocked → UnityEvent<RecipeData>

// Properties
- AllRecipes → List<RecipeData> (loaded from Resources)
- UnlockedRecipes → HashSet<RecipeData>
```

**`RecipeData.cs`** (ScriptableObject)
```csharp
// Properties
- RecipeName → string
- Description → string
- ResultItem → ItemData
- ResultQuantity → int
- RequiredMaterials → List<MaterialRequirement>
- CraftingTime → float (in seconds, can be 0 for instant)
- IsUnlockedByDefault → bool

// Nested Class
MaterialRequirement {
    ItemData Material
    int Quantity
}
```

**`CraftingStation.cs`** (MonoBehaviour)
```csharp
// Interaction trigger for player
// Opens crafting UI when player presses interact key
// Attached to crafting station objects in Lobby

// Core Methods
- OnPlayerEnter()
- OnPlayerExit()
- OpenCraftingUI()
- CloseCraftingUI()

// Properties
- InteractionPrompt → string ("Press E to Craft")
- InteractionRange → float
```

#### Dependencies
- InventorySystem (material checking and consumption)
- UI System (crafting UI)

---

### 1.3 Lobby System

#### Requirements
- Free-roam 2D area with boundaries
- Persistent between floor runs
- Contains crafting station(s)
- Contains level selection gate
- Display player progression/stats
- Safe area (no enemies or damage)

#### Components

**`LobbyManager.cs`** (Singleton)
```csharp
// Core Methods
- OnLobbyEnter()
- OnLobbyExit()
- ReturnPlayerToSpawn()
- UpdateProgressionDisplay()

// Events
- OnPlayerEnteredLobby → UnityEvent
- OnPlayerLeftLobby → UnityEvent

// Properties
- PlayerSpawnPoint → Transform
- CraftingStations → List<CraftingStation>
- LevelGate → LevelGate
```

**`LevelGate.cs`** (MonoBehaviour)
```csharp
// Trigger to start level selection
// Similar to AreaExit but leads to LevelSelection scene

// Core Methods
- OnPlayerEnter()
- OpenLevelSelection()

// Properties
- InteractionPrompt → string ("Press E to Select Mission")
- InteractionRange → float
```

#### Scene Setup
- **Scene Name**: `Lobby.unity`
- **Required Objects**:
  - Player spawn point
  - At least one CraftingStation
  - LevelGate trigger
  - UI Canvas for progression display
  - Tilemap environment

#### Dependencies
- SceneManagement (scene transitions)
- InventorySystem (accessible in lobby)
- CraftingSystem (crafting stations)

---

## Phase 2: Mission Flow

### 2.1 Level Selection System

#### Requirements
- Display available floors (1-10+)
- Show floor information: difficulty, enemy types, loot tier
- Deeper floors = higher difficulty + better rewards
- Confirm selection and proceed to preparation
- Return to lobby option

#### Components

**`LevelSelectionManager.cs`** (Singleton)
```csharp
// Core Methods
- LoadFloorData(int floorDepth) → FloorData
- SelectFloor(int floorDepth)
- ConfirmSelection()
- ReturnToLobby()
- GetUnlockedFloors() → List<int>

// Events
- OnFloorSelected → UnityEvent<FloorData>
- OnSelectionConfirmed → UnityEvent<FloorData>

// Properties
- SelectedFloor → FloorData
- MaxUnlockedFloor → int
- AllFloors → List<FloorData>
```

**`FloorData.cs`** (ScriptableObject)
```csharp
// Properties
- FloorDepth → int (1-10+)
- FloorName → string ("Floor 1 - Abandoned Mine")
- Description → string
- DifficultyRating → int (1-5 stars)
- RecommendedPreparation → string

// Enemy Configuration
- EnemyTypes → List<EnemyData>
- MinEnemies → int
- MaxEnemies → int
- BossEnemy → EnemyData (optional)

// Loot Configuration
- LootTier → int (affects drop rates)
- GuaranteedDrops → List<ItemData>
- RareDropChance → float

// Environment
- FloorSceneName → string
- BackgroundMusic → AudioClip
- AmbientEffects → List<GameObject>
```

**`FloorSelectionUI.cs`** (MonoBehaviour)
```csharp
// UI component for displaying floors
// Grid or list of selectable floor buttons

// Core Methods
- DisplayFloors(List<FloorData> floors)
- OnFloorButtonClicked(FloorData floor)
- UpdateFloorDetails(FloorData floor)
- ShowConfirmationDialog()
```

#### Scene Setup
- **Scene Name**: `LevelSelection.unity`
- **UI Elements**:
  - Floor selection grid/list
  - Floor details panel
  - Confirm/Cancel buttons
  - Back to Lobby button

#### Dependencies
- LobbyManager (return to lobby)
- PreparationManager (proceed to prep)
- FloorData ScriptableObjects

---

### 2.2 Preparation Scene

#### Requirements
- View current inventory
- Equip weapons, armor, and accessories
- Select consumables to bring (limited slots)
- View floor objectives and briefing
- Save loadout configuration
- Confirm and start mission
- Return to level selection

#### Components

**`LoadoutManager.cs`** (Singleton)
```csharp
// Core Methods
- EquipItem(ItemData item, EquipmentSlot slot) → bool
- UnequipItem(EquipmentSlot slot) → bool
- AddConsumable(ItemData item, int quantity) → bool
- RemoveConsumable(ItemData item, int quantity)
- SaveLoadout(string loadoutName)
- LoadLoadout(string loadoutName)
- GetCurrentLoadout() → Loadout
- ConfirmAndStartMission()

// Events
- OnItemEquipped → UnityEvent<ItemData, EquipmentSlot>
- OnItemUnequipped → UnityEvent<EquipmentSlot>
- OnLoadoutChanged → UnityEvent

// Properties
- CurrentLoadout → Loadout
- MaxConsumableSlots → int (e.g., 5)
- EquipmentSlots → Dictionary<EquipmentSlot, ItemData>
```

**`Loadout.cs`** (Data Class)
```csharp
// Properties
- LoadoutName → string
- WeaponSlot → ItemData
- ArmorSlots → Dictionary<ArmorType, ItemData>
  - Head, Chest, Legs, Accessory1, Accessory2
- ConsumableSlots → List<ConsumableSlot>

// Nested Class
ConsumableSlot {
    ItemData Item
    int Quantity
}
```

**`EquipmentSlot.cs`** (Enum)
```csharp
enum EquipmentSlot {
    Weapon,
    Head,
    Chest,
    Legs,
    Accessory1,
    Accessory2
}
```

**`PreparationManager.cs`** (Singleton)
```csharp
// Manages the preparation scene flow

// Core Methods
- Initialize(FloorData selectedFloor)
- ShowMissionBriefing()
- StartMission()
- ReturnToLevelSelection()

// Events
- OnMissionStarted → UnityEvent<FloorData, Loadout>

// Properties
- SelectedFloor → FloorData
- CurrentLoadout → Loadout
```

#### Scene Setup
- **Scene Name**: `Preparation.unity`
- **UI Elements**:
  - Equipment slots display
  - Consumable slots
  - Mission briefing panel
  - Start Mission button
  - Back button

#### Dependencies
- InventorySystem (item access)
- LevelSelectionManager (selected floor data)
- GameplayManager (start mission)

---

### 2.3 Task System

#### Requirements
- Define mission objectives (kill enemies, gather resources, survive, explore)
- Track task progress in real-time
- Display active tasks on HUD
- Support multiple simultaneous tasks
- Reward completion with bonus loot
- Optional vs required tasks

#### Components

**`TaskManager.cs`** (Singleton)
```csharp
// Core Methods
- InitializeTasks(List<TaskData> tasks)
- UpdateTaskProgress(TaskType type, int amount)
- CompleteTask(TaskData task)
- AreAllRequiredTasksComplete() → bool
- GetActiveTasks() → List<Task>

// Events
- OnTaskProgressUpdated → UnityEvent<Task>
- OnTaskCompleted → UnityEvent<Task>
- OnAllTasksComplete → UnityEvent

// Properties
- ActiveTasks → List<Task>
- CompletedTasks → List<Task>
```

**`TaskData.cs`** (ScriptableObject)
```csharp
// Properties
- TaskName → string
- Description → string
- TaskType → enum (Kill, Gather, Survive, Explore, Interact)
- TargetID → string (enemy type ID, item ID, etc.)
- RequiredAmount → int
- IsRequired → bool (required vs optional)
- RewardItems → List<ItemData>
- BonusXP → int (if XP system added later)
```

**`Task.cs`** (Runtime Class)
```csharp
// Runtime instance of TaskData with progress tracking

// Properties
- TaskData → TaskData
- CurrentProgress → int
- IsComplete → bool
- TimeStarted → float
- TimeCompleted → float
```

**Task Types**
```csharp
enum TaskType {
    Kill,           // Kill X enemies of type Y
    Gather,         // Collect X items of type Y
    Survive,        // Survive for X seconds
    Explore,        // Discover X locations
    Interact,       // Interact with X objects
    Reach           // Reach specific location
}
```

#### Dependencies
- GameplayManager (task initialization per floor)
- EnemyHealth (kill tracking)
- LootSystem (gather tracking)
- UI System (HUD task display)

---

## Phase 3: Progression & Loot

### 3.1 Loot System

#### Requirements
- Enemies drop loot on death based on loot tables
- Drop rates influenced by floor depth
- Rarity system affects drop chance
- Display dropped loot as collectible objects
- Auto-pickup or manual pickup
- Chest loot generation

#### Components

**`LootManager.cs`** (Singleton)
```csharp
// Core Methods
- GenerateLoot(LootTable lootTable, int floorDepth) → List<ItemDrop>
- SpawnLoot(List<ItemDrop> drops, Vector3 position)
- CalculateDropChance(LootEntry entry, int floorDepth) → float
- RollForItem(LootEntry entry, int floorDepth) → bool

// Events
- OnLootDropped → UnityEvent<List<ItemDrop>>

// Properties
- FloorDepthMultiplier → float (increases drop rates per floor)
```

**`LootTable.cs`** (ScriptableObject)
```csharp
// Properties
- TableName → string
- LootEntries → List<LootEntry>
- GuaranteedDrops → List<ItemData>
- MinDrops → int
- MaxDrops → int

// Nested Class
LootEntry {
    ItemData Item
    float BaseDropChance    // 0.0 to 1.0
    int MinQuantity
    int MaxQuantity
    Rarity MinRarity        // Only drops if floor depth high enough
}
```

**`LootPickup.cs`** (MonoBehaviour)
```csharp
// Spawned loot object in the world
// Player collides with it to pick up

// Core Methods
- Initialize(ItemData item, int quantity)
- OnPlayerPickup()
- AnimateToPlayer()
- Despawn()

// Properties
- ItemData → ItemData
- Quantity → int
- Lifetime → float (auto-despawn after X seconds)
- PickupRadius → float
```

**`ItemDrop.cs`** (Data Class)
```csharp
// Properties
- ItemData → ItemData
- Quantity → int
- DropPosition → Vector3
```

#### Dependencies
- InventorySystem (add items on pickup)
- EnemyHealth (drop loot on death)
- FloorData (floor depth for drop rates)

---

### 3.2 Floor Generation

#### Requirements
- Load pre-designed floor layouts OR generate procedurally
- Spawn enemies based on FloorData configuration
- Place loot chests and resource nodes
- Place entrance and exit points
- Ensure playable paths exist

#### Components

**`FloorGenerator.cs`** (Singleton)
```csharp
// Core Methods
- GenerateFloor(FloorData floorData)
- SpawnEnemies(FloorData floorData)
- PlaceResourceNodes(int count)
- PlaceChests(int count)
- PlaceExitPoint()
- ClearFloor()

// Events
- OnFloorGenerated → UnityEvent

// Properties
- CurrentFloorData → FloorData
- SpawnedEnemies → List<GameObject>
- SpawnedChests → List<GameObject>
- ExitPoint → GameObject
```

**`EnemySpawner.cs`** (MonoBehaviour)
```csharp
// Placed in floor scenes as spawn points

// Core Methods
- SpawnEnemy(EnemyData enemyData)
- GetSpawnPosition() → Vector3

// Properties
- SpawnRadius → float
- MaxEnemiesAtThisSpawner → int
```

**Floor Scene Structure**
```
Floor_1.unity (or procedural)
├── Tilemap (floor layout)
├── EnemySpawners (empty GameObjects)
├── ChestSpawnPoints (empty GameObjects)
├── ResourceNodeSpawnPoints (empty GameObjects)
├── PlayerEntrance (spawn point)
└── Exit (goal trigger)
```

#### Dependencies
- FloorData (enemy types, counts)
- EnemyAI (spawned enemies)
- LootManager (chest loot generation)

---

### 3.3 Progression System

#### Requirements
- No traditional XP/leveling
- All progression from drops and crafting
- Track floors completed
- Track deaths and runs
- Unlock new recipes on floor completion
- Optional: Unlock new floors by completing previous ones

#### Components

**`ProgressionManager.cs`** (Singleton)
```csharp
// Core Methods
- RecordFloorCompletion(int floorDepth, bool success)
- RecordDeath(int floorDepth)
- UnlockRecipe(RecipeData recipe)
- IsFloorUnlocked(int floorDepth) → bool
- UnlockFloor(int floorDepth)
- GetPlayerStats() → ProgressionStats

// Events
- OnFloorCompleted → UnityEvent<int>
- OnRecipeUnlocked → UnityEvent<RecipeData>
- OnFloorUnlocked → UnityEvent<int>

// Properties
- CompletedFloors → HashSet<int>
- UnlockedFloors → HashSet<int>
- TotalDeaths → int
- TotalRuns → int
- DeepestFloorReached → int
```

**`ProgressionStats.cs`** (Data Class)
```csharp
// Properties
- TotalRuns → int
- SuccessfulRuns → int
- TotalDeaths → int
- DeepestFloorReached → int
- TotalEnemiesKilled → int
- TotalItemsCrafted → int
- TotalLootCollected → int
- PlaytimeSeconds → float
```

#### Dependencies
- SaveSystem (persist progression)
- GameplayManager (track run outcomes)
- CraftingSystem (unlock recipes)

---

## Phase 4: Polish & Expansion

### 4.1 Save System

#### Requirements
- Save/load inventory
- Save/load crafting progress (unlocked recipes)
- Save/load progression stats
- Save/load settings
- Auto-save on scene transitions
- Manual save option
- Multiple save slots (optional)

#### Components

**`SaveManager.cs`** (Singleton)
```csharp
// Core Methods
- SaveGame()
- LoadGame()
- DeleteSave()
- SaveExists() → bool
- AutoSave()

// Events
- OnGameSaved → UnityEvent
- OnGameLoaded → UnityEvent

// Properties
- CurrentSaveSlot → int
- AutoSaveEnabled → bool
- AutoSaveInterval → float
```

**`SaveData.cs`** (Serializable Class)
```csharp
// Properties
- InventoryData → SerializableInventory
- CraftingData → SerializableCraftingProgress
- ProgressionData → ProgressionStats
- SettingsData → GameSettings
- LastSaveTime → DateTime
```

**Save Location**
- PC: `Application.persistentDataPath + "/saves/"`
- Format: JSON or Binary

#### Dependencies
- InventorySystem (inventory data)
- CraftingSystem (recipe unlocks)
- ProgressionManager (stats)

---

### 4.2 UI/UX Systems

#### Required UI Screens

**Inventory UI**
- Grid-based item display
- Item tooltip on hover
- Drag-and-drop for equipment
- Sort/filter options
- Search functionality

**Crafting UI**
- Recipe list (filterable by category)
- Selected recipe details
- Required materials display (highlight if insufficient)
- Craft button (disabled if can't craft)
- Crafting progress bar (if crafting takes time)

**Level Selection UI**
- Floor cards/buttons
- Floor details panel
- Locked floor indicators
- Difficulty rating display

**Preparation UI**
- Equipment slots (drag-and-drop)
- Consumable slots (limited quantity)
- Mission briefing panel
- Start button

**HUD (In-Game)**
- Health bar
- Active tasks panel
- Mini-map (optional)
- Equipment quick slots
- Consumable quick use buttons

**Death/Victory Screen**
- Results summary (kills, loot, time)
- Rewards display
- Continue button (return to lobby)

#### Components

**`UIManager.cs`** (Singleton)
```csharp
// Manages UI screen transitions

// Core Methods
- ShowScreen(UIScreen screen)
- HideScreen(UIScreen screen)
- HideAllScreens()
- ShowPopup(string message, Sprite icon)
- ShowConfirmation(string message, Action onConfirm)

// Properties
- CurrentScreen → UIScreen
- AllScreens → Dictionary<string, UIScreen>
```

---

### 4.3 Audio System

#### Requirements
- Background music per scene
- Combat sound effects
- UI interaction sounds
- Ambient environment sounds
- Volume controls (Master, Music, SFX)
- Audio mixing

#### Components

**`AudioManager.cs`** (Singleton)
```csharp
// Core Methods
- PlayMusic(AudioClip clip, bool loop)
- PlaySFX(AudioClip clip, Vector3 position)
- PlayUISFX(AudioClip clip)
- StopMusic()
- StopAllSFX()
- SetMasterVolume(float volume)
- SetMusicVolume(float volume)
- SetSFXVolume(float volume)

// Properties
- MusicSource → AudioSource
- SFXPoolSize → int
- AudioMixer → AudioMixer
```

---

## Data Structures

### ScriptableObject Organization

```
Assets/Data/
├── Items/
│   ├── Weapons/
│   ├── Armor/
│   ├── Consumables/
│   └── Materials/
├── Enemies/
│   ├── Floor1/
│   ├── Floor2/
│   └── Bosses/
├── Floors/
│   ├── Floor_1_Data.asset
│   ├── Floor_2_Data.asset
│   └── ...
├── Recipes/
│   ├── WeaponRecipes/
│   ├── ArmorRecipes/
│   └── ConsumableRecipes/
└── LootTables/
    ├── EnemyLootTables/
    └── ChestLootTables/
```

---

## Technical Architecture

### Scene Flow Diagram

```
Lobby.unity
    ↓ (LevelGate)
LevelSelection.unity
    ↓ (Floor Selected)
Preparation.unity
    ↓ (Start Mission)
Floor_X.unity (Gameplay)
    ↓ (Success/Death)
Results Screen (UI)
    ↓
Back to Lobby.unity
```

### Manager Hierarchy

```
DontDestroyOnLoad Root
├── GameManager (overall game state)
├── InventorySystem
├── CraftingSystem
├── LootManager
├── ProgressionManager
├── SaveManager
├── AudioManager
└── UIManager

Scene-Specific (Destroyed on Load)
├── LobbyManager (Lobby scene)
├── LevelSelectionManager (LevelSelection scene)
├── PreparationManager (Preparation scene)
├── GameplayManager (Floor scenes)
└── FloorGenerator (Floor scenes)
```

### Event Flow Example: Starting a Floor

1. Player interacts with LevelGate in Lobby
2. LevelGate calls SceneManagement to load LevelSelection scene
3. LevelSelectionManager initializes and loads FloorData
4. Player selects Floor 3
5. LevelSelectionManager stores selected floor
6. SceneManagement loads Preparation scene
7. PreparationManager retrieves selected floor and displays briefing
8. Player configures loadout via LoadoutManager
9. Player clicks "Start Mission"
10. PreparationManager calls GameplayManager to start mission
11. SceneManagement loads Floor_3 scene
12. FloorGenerator generates floor layout and spawns enemies
13. TaskManager initializes floor tasks
14. Player plays, completes tasks, collects loot
15. Player reaches exit or dies
16. GameplayManager records results
17. ProgressionManager updates stats
18. Results screen displays
19. SceneManagement returns to Lobby

---

## Dependencies

### System Dependency Graph

```
SaveSystem
    ↑
    ├── InventorySystem
    ├── CraftingSystem
    ├── ProgressionManager
    └── (all managers)

InventorySystem
    ↑
    ├── CraftingSystem (material checking)
    ├── LoadoutManager (equipment)
    ├── LootManager (item pickup)
    └── UI (inventory display)

CraftingSystem
    ↑
    ├── InventorySystem (materials)
    ├── CraftingStation (lobby interaction)
    └── ProgressionManager (unlock recipes)

LootManager
    ↑
    ├── InventorySystem (add items)
    ├── EnemyHealth (drop on death)
    └── FloorData (drop rate modifiers)

GameplayManager (per floor)
    ↑
    ├── TaskManager (objectives)
    ├── FloorGenerator (level setup)
    ├── ProgressionManager (record results)
    └── UIManager (HUD, results)
```

---

## Testing Requirements

### Unit Tests
- [ ] InventorySystem: Add/Remove/HasItem logic
- [ ] CraftingSystem: Recipe validation and crafting logic
- [ ] LootManager: Drop rate calculations
- [ ] TaskManager: Progress tracking and completion
- [ ] SaveManager: Serialization/deserialization

### Integration Tests
- [ ] Lobby → Level Selection → Preparation → Floor flow
- [ ] Inventory persistence across scenes
- [ ] Crafting and using crafted items in floor
- [ ] Loot drop and pickup integration
- [ ] Task completion and rewards

### Gameplay Tests
- [ ] Complete Floor 1 with different loadouts
- [ ] Test death penalty (item loss)
- [ ] Test crafting all recipe types
- [ ] Test floor difficulty scaling (Floor 1 vs Floor 10)
- [ ] Test edge cases (full inventory, insufficient materials)

### Performance Tests
- [ ] 20+ enemies on screen at 60 FPS
- [ ] Scene load times < 3 seconds
- [ ] Inventory UI with 100+ items smooth
- [ ] Save/Load time < 1 second

---

## Implementation Priority

### Sprint 1: Foundation (Weeks 1-2)
1. InventorySystem + ItemData ScriptableObjects
2. Basic Lobby scene with free roam
3. InventoryUI (basic grid display)

### Sprint 2: Crafting (Weeks 3-4)
1. CraftingSystem + RecipeData ScriptableObjects
2. CraftingStation interaction
3. CraftingUI
4. Test crafting flow in Lobby

### Sprint 3: Level Selection (Weeks 5-6)
1. LevelSelectionManager + FloorData ScriptableObjects
2. LevelGate in Lobby
3. LevelSelectionUI
4. Scene transitions (Lobby ↔ LevelSelection)

### Sprint 4: Preparation & Loadout (Weeks 7-8)
1. LoadoutManager + Loadout system
2. Preparation scene UI
3. Equipment system integration
4. Scene transitions (LevelSelection ↔ Preparation)

### Sprint 5: Gameplay Floor (Weeks 9-10)
1. GameplayManager + TaskManager
2. Floor scene template
3. Task UI on HUD
4. Exit trigger and results

### Sprint 6: Loot & Progression (Weeks 11-12)
1. LootManager + LootTable ScriptableObjects
2. LootPickup prefab
3. ProgressionManager
4. Death/Victory flow

### Sprint 7: Save System (Week 13)
1. SaveManager
2. SaveData serialization
3. Auto-save on scene transitions
4. Load game on startup

### Sprint 8: Polish (Week 14+)
1. AudioManager + sound effects
2. UI polish and animations
3. Particle effects
4. Bug fixes and optimization

---

## Notes

- All timestamps use `Time.time` or `Time.deltaTime`
- Scene loading uses `SceneManager.LoadScene()` with fade transitions
- Inventory uses `Dictionary<ItemData, int>` for O(1) lookups
- Loot drop rates use `Random.Range()` with seeded RNG for consistency
- UI uses Unity's UI Toolkit or uGUI (Canvas)
- SaveSystem uses JSON serialization with Unity's `JsonUtility`

---

**Document Version**: 1.0
**Last Updated**: 2025-11-12
**Author**: Khesir
