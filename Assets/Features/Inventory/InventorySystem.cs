using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Singleton manager for the inventory system.
/// Handles item storage, retrieval, and management across the game.
/// Persists between scenes with DontDestroyOnLoad.
/// </summary>
public class InventorySystem : Singleton<InventorySystem>
{
    [Header("Inventory Settings")]
    [SerializeField] private int maxInventorySlots = 50; // UI display limit, not storage limit

    // Core inventory storage - uses Dictionary for O(1) lookup performance
    private Dictionary<ItemData, InventorySlot> items = new Dictionary<ItemData, InventorySlot>();

    // Events for UI and other systems to respond to inventory changes
    [System.Serializable] public class InventorySlotEvent : UnityEvent<InventorySlot> { }
    [System.Serializable] public class ItemEvent : UnityEvent<ItemData, int> { }

    public InventorySlotEvent OnInventoryChanged = new InventorySlotEvent();
    public ItemEvent OnItemAdded = new ItemEvent();
    public ItemEvent OnItemRemoved = new ItemEvent();

    // Public Properties
    public int MaxInventorySlots => maxInventorySlots;
    public int CurrentItemTypeCount => items.Count; // Number of unique item types
    public int TotalItemCount => items.Values.Sum(slot => slot.Quantity); // Total quantity of all items

    protected override void Awake()
    {
        base.Awake();
        InitializeInventory();
    }

    /// <summary>
    /// Initialize the inventory system
    /// </summary>
    private void InitializeInventory()
    {
        if (items == null)
        {
            items = new Dictionary<ItemData, InventorySlot>();
        }
    }

    /// <summary>
    /// Adds an item to the inventory
    /// </summary>
    /// <param name="item">The item to add</param>
    /// <param name="quantity">Amount to add</param>
    /// <returns>True if successfully added, false if failed</returns>
    public bool AddItem(ItemData item, int quantity)
    {
        if (item == null || quantity <= 0)
        {
            Debug.LogWarning("InventorySystem: Cannot add null item or invalid quantity");
            return false;
        }

        // Check if item already exists in inventory
        if (items.ContainsKey(item))
        {
            // Item exists - try to add to existing stack(s)
            if (item.IsStackable)
            {
                int remaining = quantity;
                InventorySlot slot = items[item];

                // Add to existing slot
                remaining = slot.AddQuantity(remaining);

                // If there's still remaining, we may need overflow handling
                // For now, we just add what we can
                if (remaining > 0)
                {
                    Debug.LogWarning($"InventorySystem: Added {quantity - remaining}/{quantity} of {item.ItemName}. {remaining} items couldn't fit.");
                }

                OnInventoryChanged?.Invoke(slot);
                OnItemAdded?.Invoke(item, quantity - remaining);
                return true;
            }
            else
            {
                // Non-stackable items - each needs its own slot
                // For simplicity, we'll just increment quantity (can represent multiple unique instances)
                Debug.LogWarning($"InventorySystem: {item.ItemName} is not stackable. Consider using multiple ItemData instances for unique items.");
                return false;
            }
        }
        else
        {
            // Item doesn't exist - create new slot
            InventorySlot newSlot = new InventorySlot(item, quantity, items.Count);
            items.Add(item, newSlot);

            OnInventoryChanged?.Invoke(newSlot);
            OnItemAdded?.Invoke(item, quantity);
            return true;
        }
    }

    /// <summary>
    /// Removes an item from the inventory
    /// </summary>
    /// <param name="item">The item to remove</param>
    /// <param name="quantity">Amount to remove</param>
    /// <returns>True if successfully removed, false if not enough quantity</returns>
    public bool RemoveItem(ItemData item, int quantity)
    {
        if (item == null || quantity <= 0)
        {
            Debug.LogWarning("InventorySystem: Cannot remove null item or invalid quantity");
            return false;
        }

        if (!items.ContainsKey(item))
        {
            Debug.LogWarning($"InventorySystem: Cannot remove {item.ItemName} - not in inventory");
            return false;
        }

        InventorySlot slot = items[item];

        // Check if we have enough
        if (slot.Quantity < quantity)
        {
            Debug.LogWarning($"InventorySystem: Not enough {item.ItemName}. Have {slot.Quantity}, need {quantity}");
            return false;
        }

        // Remove quantity
        slot.RemoveQuantity(quantity);

        // If slot is empty, remove it from dictionary
        if (slot.IsEmpty())
        {
            items.Remove(item);
        }

        OnInventoryChanged?.Invoke(slot);
        OnItemRemoved?.Invoke(item, quantity);
        return true;
    }

    /// <summary>
    /// Checks if the inventory contains a specific item with sufficient quantity
    /// </summary>
    /// <param name="item">The item to check</param>
    /// <param name="quantity">Required quantity</param>
    /// <returns>True if inventory has the item with sufficient quantity</returns>
    public bool HasItem(ItemData item, int quantity = 1)
    {
        if (item == null)
            return false;

        return items.ContainsKey(item) && items[item].Quantity >= quantity;
    }

    /// <summary>
    /// Gets the quantity of a specific item in the inventory
    /// </summary>
    /// <param name="item">The item to check</param>
    /// <returns>Quantity of the item, or 0 if not found</returns>
    public int GetItemCount(ItemData item)
    {
        if (item == null || !items.ContainsKey(item))
            return 0;

        return items[item].Quantity;
    }

    /// <summary>
    /// Gets all items in the inventory
    /// </summary>
    /// <returns>List of all inventory slots</returns>
    public List<InventorySlot> GetAllItems()
    {
        return items.Values.ToList();
    }

    /// <summary>
    /// Gets all items of a specific type
    /// </summary>
    /// <param name="itemType">The type to filter by</param>
    /// <returns>List of inventory slots matching the type</returns>
    public List<InventorySlot> GetItemsByType(ItemType itemType)
    {
        return items.Values
            .Where(slot => slot.ItemData.Type == itemType)
            .ToList();
    }

    /// <summary>
    /// Gets all items of a specific rarity
    /// </summary>
    /// <param name="rarity">The rarity to filter by</param>
    /// <returns>List of inventory slots matching the rarity</returns>
    public List<InventorySlot> GetItemsByRarity(Rarity rarity)
    {
        return items.Values
            .Where(slot => slot.ItemData.ItemRarity == rarity)
            .ToList();
    }

    /// <summary>
    /// Clears the entire inventory
    /// </summary>
    public void ClearInventory()
    {
        items.Clear();
        OnInventoryChanged?.Invoke(null);
        Debug.Log("InventorySystem: Inventory cleared");
    }

    /// <summary>
    /// Gets inventory data for saving
    /// </summary>
    /// <returns>Serializable inventory data</returns>
    public SerializableInventory GetSaveData()
    {
        SerializableInventory saveData = new SerializableInventory();
        saveData.items = new List<SerializableInventorySlot>();

        foreach (var kvp in items)
        {
            SerializableInventorySlot slot = new SerializableInventorySlot
            {
                itemName = kvp.Key.name, // ScriptableObject name
                quantity = kvp.Value.Quantity,
                isEquipped = kvp.Value.IsEquipped
            };
            saveData.items.Add(slot);
        }

        return saveData;
    }

    /// <summary>
    /// Loads inventory data from save
    /// </summary>
    /// <param name="saveData">The save data to load</param>
    public void LoadSaveData(SerializableInventory saveData)
    {
        ClearInventory();

        if (saveData == null || saveData.items == null)
        {
            Debug.LogWarning("InventorySystem: No save data to load");
            return;
        }

        foreach (var slot in saveData.items)
        {
            // Load ItemData from Resources by name
            ItemData itemData = Resources.Load<ItemData>($"Data/Items/{slot.itemName}");
            if (itemData != null)
            {
                AddItem(itemData, slot.quantity);
                if (slot.isEquipped && items.ContainsKey(itemData))
                {
                    items[itemData].IsEquipped = true;
                }
            }
            else
            {
                Debug.LogWarning($"InventorySystem: Could not load item {slot.itemName} from Resources");
            }
        }

        Debug.Log("InventorySystem: Inventory loaded from save data");
    }

    /// <summary>
    /// Debug method to print inventory contents
    /// </summary>
    public void DebugPrintInventory()
    {
        Debug.Log("=== INVENTORY CONTENTS ===");
        Debug.Log($"Total Item Types: {CurrentItemTypeCount}");
        Debug.Log($"Total Items: {TotalItemCount}");

        foreach (var slot in items.Values)
        {
            Debug.Log($"- {slot.ItemData.ItemName} x{slot.Quantity} ({slot.ItemData.ItemRarity})");
        }

        Debug.Log("=========================");
    }
}

/// <summary>
/// Serializable inventory data for save/load functionality
/// </summary>
[System.Serializable]
public class SerializableInventory
{
    public List<SerializableInventorySlot> items;
}

/// <summary>
/// Serializable inventory slot for save/load functionality
/// </summary>
[System.Serializable]
public class SerializableInventorySlot
{
    public string itemName; // ScriptableObject name
    public int quantity;
    public bool isEquipped;
}
