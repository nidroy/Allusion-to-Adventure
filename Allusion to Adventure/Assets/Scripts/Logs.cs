using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// деревья
/// </summary>
public class Logs : MonoBehaviour
{
    public InventoryData inventoryData; // данные инвентаря


    /// <summary>
    /// добавить бревна в инвентарь
    /// </summary>
    public void AddToInventory()
    {
        Inventory inventory = GameObject.Find("WorldUI").GetComponentInChildren<Inventory>();
        List<Cell> cells = inventory.cells;

        foreach (Cell cell in cells)
            if (cell.item.gameObject.activeInHierarchy)
            {
                if (cell.item.data.id == 4)
                {
                    int count = int.Parse(cell.item.count.text) + 2;
                    cell.item.count.text = count.ToString();

                    Destroy(gameObject);
                    return;
                }
            }
            else
            {
                cell.item.data.id = inventoryData.itemsData[4].id;
                cell.item.data.name = inventoryData.itemsData[4].name;
                cell.item.data.type = inventoryData.itemsData[4].type;
                cell.item.data.durability = inventoryData.itemsData[4].durability;
                cell.item.data.damage = inventoryData.itemsData[4].damage;
                cell.item.data.sprite = inventoryData.itemsData[4].sprite;

                cell.item.image.sprite = cell.item.data.sprite;
                cell.item.count.text = "2";
                cell.item.maxDurability = cell.item.data.durability;

                cell.item.gameObject.SetActive(true);

                Destroy(gameObject);
                return;
            }
    }
}
