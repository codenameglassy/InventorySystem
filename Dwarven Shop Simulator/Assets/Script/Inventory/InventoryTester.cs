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

        if (Input.GetKeyDown(KeyCode.S))
        {
            inventory.AddItem(wood, 1);
        }


    }
}