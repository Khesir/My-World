using UnityEngine;

/// <summary>
/// Test script for the Inventory System.
/// Attach this to a GameObject in the scene to test inventory functionality.
/// Requires sample ItemData assets to be created first.
/// </summary>
public class InventoryTester : MonoBehaviour
{
    [Header("Test Items (Assign in Inspector)")]
    [SerializeField] private ItemData testWeapon;
    [SerializeField] private ItemData testArmor;
    [SerializeField] private ItemData testConsumable;
    [SerializeField] private ItemData testMaterial;

    [Header("Test Settings")]
    [SerializeField] private bool runTestsOnStart = true;

    private void Start()
    {
        if (runTestsOnStart)
        {
            RunInventoryTests();
        }
    }

    private void Update()
    {
        // Keyboard shortcuts for testing
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TestAddItems();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TestRemoveItems();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TestPrintInventory();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TestClearInventory();
        }
    }

    /// <summary>
    /// Runs comprehensive tests of the inventory system
    /// </summary>
    public void RunInventoryTests()
    {
        Debug.Log("=== STARTING INVENTORY SYSTEM TESTS ===");

        if (InventorySystem.Instance == null)
        {
            Debug.LogError("InventorySystem instance not found! Make sure it exists in the scene.");
            return;
        }

        // Subscribe to events
        InventorySystem.Instance.OnItemAdded.AddListener(OnItemAdded);
        InventorySystem.Instance.OnItemRemoved.AddListener(OnItemRemoved);
        InventorySystem.Instance.OnInventoryChanged.AddListener(OnInventoryChanged);

        TestAddItems();
        TestItemQueries();
        TestRemoveItems();
        TestPrintInventory();

        Debug.Log("=== INVENTORY SYSTEM TESTS COMPLETE ===");
    }

    private void TestAddItems()
    {
        Debug.Log("--- TEST: Adding Items ---");

        if (testWeapon != null)
        {
            InventorySystem.Instance.AddItem(testWeapon, 1);
            Debug.Log($"Added 1x {testWeapon.ItemName}");
        }

        if (testArmor != null)
        {
            InventorySystem.Instance.AddItem(testArmor, 1);
            Debug.Log($"Added 1x {testArmor.ItemName}");
        }

        if (testConsumable != null)
        {
            InventorySystem.Instance.AddItem(testConsumable, 5);
            Debug.Log($"Added 5x {testConsumable.ItemName}");
        }

        if (testMaterial != null)
        {
            InventorySystem.Instance.AddItem(testMaterial, 20);
            Debug.Log($"Added 20x {testMaterial.ItemName}");
        }
    }

    private void TestItemQueries()
    {
        Debug.Log("--- TEST: Querying Items ---");

        if (testWeapon != null)
        {
            bool hasWeapon = InventorySystem.Instance.HasItem(testWeapon);
            int weaponCount = InventorySystem.Instance.GetItemCount(testWeapon);
            Debug.Log($"Has {testWeapon.ItemName}: {hasWeapon}, Count: {weaponCount}");
        }

        if (testMaterial != null)
        {
            bool hasEnoughMaterial = InventorySystem.Instance.HasItem(testMaterial, 10);
            Debug.Log($"Has at least 10x {testMaterial.ItemName}: {hasEnoughMaterial}");
        }

        // Test filtering
        var weapons = InventorySystem.Instance.GetItemsByType(ItemType.Weapon);
        Debug.Log($"Weapons in inventory: {weapons.Count}");

        var rareItems = InventorySystem.Instance.GetItemsByRarity(Rarity.Rare);
        Debug.Log($"Rare items in inventory: {rareItems.Count}");
    }

    private void TestRemoveItems()
    {
        Debug.Log("--- TEST: Removing Items ---");

        if (testConsumable != null)
        {
            bool success = InventorySystem.Instance.RemoveItem(testConsumable, 2);
            Debug.Log($"Removed 2x {testConsumable.ItemName}: {(success ? "SUCCESS" : "FAILED")}");
        }

        if (testMaterial != null)
        {
            bool success = InventorySystem.Instance.RemoveItem(testMaterial, 5);
            Debug.Log($"Removed 5x {testMaterial.ItemName}: {(success ? "SUCCESS" : "FAILED")}");
        }
    }

    private void TestPrintInventory()
    {
        Debug.Log("--- TEST: Printing Inventory ---");
        InventorySystem.Instance.DebugPrintInventory();
    }

    private void TestClearInventory()
    {
        Debug.Log("--- TEST: Clearing Inventory ---");
        InventorySystem.Instance.ClearInventory();
        Debug.Log("Inventory cleared!");
    }

    // Event Listeners
    private void OnItemAdded(ItemData item, int quantity)
    {
        Debug.Log($"[EVENT] Item Added: {item.ItemName} x{quantity}");
    }

    private void OnItemRemoved(ItemData item, int quantity)
    {
        Debug.Log($"[EVENT] Item Removed: {item.ItemName} x{quantity}");
    }

    private void OnInventoryChanged(InventorySlot slot)
    {
        if (slot != null)
        {
            Debug.Log($"[EVENT] Inventory Changed: {slot.ItemData.ItemName} now has {slot.Quantity}");
        }
        else
        {
            Debug.Log($"[EVENT] Inventory Changed: Cleared");
        }
    }

    private void OnGUI()
    {
        // Draw on-screen instructions
        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.Label("=== Inventory Tester ===");
        GUILayout.Label("Press 1: Add Items");
        GUILayout.Label("Press 2: Remove Items");
        GUILayout.Label("Press 3: Print Inventory");
        GUILayout.Label("Press 4: Clear Inventory");
        GUILayout.Label("");
        GUILayout.Label($"Total Items: {(InventorySystem.Instance != null ? InventorySystem.Instance.TotalItemCount : 0)}");
        GUILayout.Label($"Item Types: {(InventorySystem.Instance != null ? InventorySystem.Instance.CurrentItemTypeCount : 0)}");
        GUILayout.EndArea();
    }
}
