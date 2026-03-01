using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    public Inventory playerInventory;
    public Inventory storeInventory;

    public ItemData ironOre;
    public ItemData wood;

    void Update()
    {
        // Add Iron Ore to player inventory
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerInventory.AddItem(ironOre, 1);
        }

        // Add Wood to player inventory
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerInventory.AddItem(wood, 5);
        }

        // Add Iron Ore to store
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            storeInventory.AddItem(ironOre, 3);
        }

        // Add Wood to store
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            storeInventory.AddItem(wood, 2);
        }
    }
}