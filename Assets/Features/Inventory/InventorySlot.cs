using UnityEngine;

/// <summary>
/// Represents a single slot in the inventory containing an item and its quantity.
/// Tracks whether the item is equipped and the slot's position in the UI.
/// </summary>
[System.Serializable]
public class InventorySlot
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int quantity;
    [SerializeField] private bool isEquipped;
    [SerializeField] private int slotIndex;

    // Public Properties
    public ItemData ItemData => itemData;
    public int Quantity
    {
        get => quantity;
        set => quantity = Mathf.Max(0, value); // Ensure quantity never goes negative
    }
    public bool IsEquipped
    {
        get => isEquipped;
        set => isEquipped = value;
    }
    public int SlotIndex
    {
        get => slotIndex;
        set => slotIndex = value;
    }

    /// <summary>
    /// Constructor for creating a new inventory slot
    /// </summary>
    public InventorySlot(ItemData itemData, int quantity, int slotIndex = -1)
    {
        this.itemData = itemData;
        this.quantity = Mathf.Max(0, quantity);
        this.slotIndex = slotIndex;
        this.isEquipped = false;
    }

    /// <summary>
    /// Adds quantity to this slot. Returns overflow amount if it exceeds max stack size.
    /// </summary>
    /// <param name="amount">Amount to add</param>
    /// <returns>Overflow amount that couldn't be added</returns>
    public int AddQuantity(int amount)
    {
        if (itemData == null || !itemData.IsStackable)
            return amount;

        int maxStack = itemData.MaxStackSize;
        int newQuantity = quantity + amount;

        if (newQuantity <= maxStack)
        {
            quantity = newQuantity;
            return 0; // No overflow
        }
        else
        {
            quantity = maxStack;
            return newQuantity - maxStack; // Return overflow
        }
    }

    /// <summary>
    /// Removes quantity from this slot. Returns false if not enough quantity available.
    /// </summary>
    /// <param name="amount">Amount to remove</param>
    /// <returns>True if successfully removed, false if not enough quantity</returns>
    public bool RemoveQuantity(int amount)
    {
        if (amount > quantity)
            return false;

        quantity -= amount;
        return true;
    }

    /// <summary>
    /// Checks if this slot can accept more of the same item
    /// </summary>
    /// <returns>True if slot has space for more items</returns>
    public bool CanAddMore()
    {
        if (itemData == null)
            return true;

        if (!itemData.IsStackable)
            return false;

        return quantity < itemData.MaxStackSize;
    }

    /// <summary>
    /// Returns how much more of this item can be added to the slot
    /// </summary>
    /// <returns>Remaining space in the stack</returns>
    public int GetRemainingSpace()
    {
        if (itemData == null)
            return int.MaxValue;

        if (!itemData.IsStackable)
            return 0;

        return itemData.MaxStackSize - quantity;
    }

    /// <summary>
    /// Checks if this slot is empty (no item or zero quantity)
    /// </summary>
    /// <returns>True if slot is empty</returns>
    public bool IsEmpty()
    {
        return itemData == null || quantity <= 0;
    }

    /// <summary>
    /// Clears the slot completely
    /// </summary>
    public void Clear()
    {
        itemData = null;
        quantity = 0;
        isEquipped = false;
    }

    /// <summary>
    /// Creates a deep copy of this inventory slot
    /// </summary>
    /// <returns>New InventorySlot with same data</returns>
    public InventorySlot Clone()
    {
        return new InventorySlot(itemData, quantity, slotIndex)
        {
            isEquipped = this.isEquipped
        };
    }
}
