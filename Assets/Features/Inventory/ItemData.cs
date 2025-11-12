using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject that defines an item's properties, stats, and metadata.
/// Used for all item types: weapons, armor, consumables, materials, and misc items.
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Game/Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string itemName;
    [SerializeField] [TextArea(3, 6)] private string description;
    [SerializeField] private Sprite icon;

    [Header("Item Properties")]
    [SerializeField] private ItemType itemType;
    [SerializeField] private Rarity rarity;
    [SerializeField] private bool isStackable = true;
    [SerializeField] private int maxStackSize = 99;

    [Header("Drop Settings")]
    [SerializeField] [Range(0f, 1f)] private float baseDropRate = 0.1f;

    [Header("Equipment Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float defense;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private List<EffectData> specialEffects = new List<EffectData>();

    // Public Properties
    public string ItemName => itemName;
    public string Description => description;
    public Sprite Icon => icon;
    public ItemType Type => itemType;
    public Rarity ItemRarity => rarity;
    public bool IsStackable => isStackable;
    public int MaxStackSize => maxStackSize;
    public float BaseDropRate => baseDropRate;

    // Equipment Stats
    public float Damage => damage;
    public float Defense => defense;
    public float AttackSpeed => attackSpeed;
    public List<EffectData> SpecialEffects => specialEffects;

    /// <summary>
    /// Returns a formatted tooltip string for UI display
    /// </summary>
    public string GetTooltip()
    {
        string tooltip = $"<b>{itemName}</b>\n";
        tooltip += $"<color=#{GetRarityColor()}>{rarity}</color>\n\n";
        tooltip += $"{description}\n\n";

        // Add stats if equipment
        if (itemType == ItemType.Weapon)
        {
            tooltip += $"Damage: {damage}\n";
            tooltip += $"Attack Speed: {attackSpeed}\n";
        }
        else if (itemType == ItemType.Armor)
        {
            tooltip += $"Defense: {defense}\n";
        }

        return tooltip;
    }

    /// <summary>
    /// Returns hex color code for rarity display
    /// </summary>
    private string GetRarityColor()
    {
        return rarity switch
        {
            Rarity.Common => "FFFFFF",      // White
            Rarity.Uncommon => "1EFF00",    // Green
            Rarity.Rare => "0070DD",        // Blue
            Rarity.Epic => "A335EE",        // Purple
            Rarity.Legendary => "FF8000",   // Orange
            _ => "FFFFFF"
        };
    }
}

/// <summary>
/// Defines the category/type of item
/// </summary>
public enum ItemType
{
    Weapon,
    Armor,
    Consumable,
    Material,
    Misc
}

/// <summary>
/// Defines item rarity which affects drop rates and display
/// </summary>
public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

/// <summary>
/// Placeholder for special effects that items can have
/// To be expanded when effect system is implemented
/// </summary>
[System.Serializable]
public class EffectData
{
    public string effectName;
    public float effectValue;
    public float duration;
}
