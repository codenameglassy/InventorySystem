using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    public Inventory inventory;

    public ItemData ironOre;
    public ItemData wood;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            inventory.AddItem(ironOre, 1);
        }

    }
}