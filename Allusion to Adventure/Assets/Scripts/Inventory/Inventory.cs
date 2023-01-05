using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// инвентарь
/// </summary>
public class Inventory : MonoBehaviour
{
    public static bool isNewGame = true; // €вл€етс€ ли игра новой?

    public InventoryData data; // данные инвентар€

    public List<Cell> cells; // €чейки


    private void Start()
    {
        UpdateCellsData();
        FillInventory();
    }


    /// <summary>
    /// обновить данные €чеек
    /// </summary>
    public void UpdateCellsData()
    {
        foreach (Cell cell in cells)
        {
            cell.id = InventoryData.cellsData.Count;
            InventoryData.cellsData.Add(cell);
        }
    }

    /// <summary>
    /// заполнить инвентарь
    /// </summary>
    private void FillInventory()
    {
        if (transform.parent.name == "WorldUI" && isNewGame)
        {
            for (int i = 0; i < 2; i++)
                SpawnItem(i, 0);
            for (int i = 2; i < 4; i++)
                SpawnItem(i, 3);
        }
    }

    /// <summary>
    /// по€вление предмета
    /// </summary>
    /// <param name="cellID">идентификатор €чейки</param>
    /// <param name="itemDataID">идентификатор данных предмета</param>
    private void SpawnItem(int cellID, int itemDataID)
    {
        cells[cellID].item.data.id = data.itemsData[itemDataID].id;
        cells[cellID].item.data.name = data.itemsData[itemDataID].name;
        cells[cellID].item.data.type = data.itemsData[itemDataID].type;
        cells[cellID].item.data.durability = data.itemsData[itemDataID].durability;
        cells[cellID].item.data.damage = data.itemsData[itemDataID].damage;
        cells[cellID].item.data.sprite = data.itemsData[itemDataID].sprite;

        cells[cellID].item.image.sprite = cells[cellID].item.data.sprite;
        cells[cellID].item.count.text = "1";
        cells[cellID].item.maxDurability = cells[cellID].item.data.durability;

        cells[cellID].item.gameObject.SetActive(true);
    }
}
