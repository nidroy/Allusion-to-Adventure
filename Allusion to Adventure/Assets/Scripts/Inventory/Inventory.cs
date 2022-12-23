using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// инвентарь
/// </summary>
public class Inventory : MonoBehaviour
{
    public InventoryData data; // данные инвентар€

    public List<Cell> cells; // €чейки


    private void Start()
    {
        UpdateCellsData();
        i();
    }

    private void i()
    {
        if (transform.parent.name == "WorldUI")
        {
            ItemData weapon = new ItemData();
            weapon.id = data.itemsData[0].id;
            weapon.name = data.itemsData[0].name;
            weapon.type = data.itemsData[0].type;
            weapon.durability = data.itemsData[0].durability;
            weapon.damage = data.itemsData[0].damage;
            weapon.sprite = data.itemsData[0].sprite;

            ItemData armor = new ItemData();
            armor.id = data.itemsData[1].id;
            armor.name = data.itemsData[1].name;
            armor.type = data.itemsData[1].type;
            armor.durability = data.itemsData[1].durability;
            armor.damage = data.itemsData[1].damage;
            armor.sprite = data.itemsData[1].sprite;

            ItemData consumables = new ItemData();
            consumables.id = data.itemsData[2].id;
            consumables.name = data.itemsData[2].name;
            consumables.type = data.itemsData[2].type;
            consumables.durability = data.itemsData[2].durability;
            consumables.damage = data.itemsData[2].damage;
            consumables.sprite = data.itemsData[2].sprite;

            cells[0].item.data = weapon;
            cells[0].item.image.sprite = weapon.sprite;
            cells[0].item.count.text = "1";
            cells[0].item.maxDurability = weapon.durability;

            cells[0].item.gameObject.SetActive(true);

            cells[1].item.data = weapon;
            cells[1].item.image.sprite = weapon.sprite;
            cells[1].item.count.text = "1";
            cells[1].item.maxDurability = weapon.durability;

            cells[1].item.gameObject.SetActive(true);

            cells[2].item.data = armor;
            cells[2].item.image.sprite = armor.sprite;
            cells[2].item.count.text = "1";
            cells[2].item.maxDurability = armor.durability;

            cells[2].item.gameObject.SetActive(true);

            cells[3].item.data = consumables;
            cells[3].item.image.sprite = consumables.sprite;
            cells[3].item.count.text = "5";
            cells[3].item.maxDurability = consumables.durability;

            cells[3].item.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// обновить данные предметов
    /// </summary>
    public void UpdateCellsData()
    {
        foreach (Cell cell in cells)
        {
            cell.id = InventoryData.cellsData.Count;
            InventoryData.cellsData.Add(cell);
        }
    }
}
