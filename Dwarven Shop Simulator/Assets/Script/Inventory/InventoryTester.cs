using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    public Inventory inventory;

    public ItemData ironOre;
    public ItemData wood;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inventory.AddItem(ironOre, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inventory.AddItem(wood, 1);
        }


    }
}