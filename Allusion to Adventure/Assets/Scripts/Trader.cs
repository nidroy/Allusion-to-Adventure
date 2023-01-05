using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public class Trader : MonoBehaviour
{
    public Inventory inventory; // ���������


    /// <summary>
    /// ������ ������
    /// </summary>
    public void BuyMoney()
    {
        Purchase(0, 4, 2, 5, 10);
    }

    /// <summary>
    /// ������ ���
    /// </summary>
    public void BuySword()
    {
        Purchase(1, 5, 500, 0, 1);
    }

    /// <summary>
    /// ������ �����
    /// </summary>
    public void BuyAxe()
    {
        Purchase(2, 5, 100, 3, 1);
    }

    /// <summary>
    /// ������ �����
    /// </summary>
    public void BuyArmor()
    {
        Purchase(3, 5, 1000, 1, 1);
    }

    /// <summary>
    /// ������ ����� �������
    /// </summary>
    public void BuyHealingPotion()
    {
        Purchase(4, 5, 100, 2, 1);
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="cellID">������������� ������</param>
    /// <param name="itemDataID">������������� ������ ��������</param>
    /// <param name="price">���� ��������</param>
    /// <param name="productDataID">������������� ������ ������</param>
    /// <param name="count">���������� �������</param>
    private void Purchase(int cellID, int itemDataID, int price, int productDataID, int count)
    {
        Cell cell = GetCell(cellID, itemDataID, productDataID, price);

        if (cell != null)
        {
            int countItem = int.Parse(inventory.cells[cellID].item.count.text) - price;
            if (countItem > 0)
                inventory.cells[cellID].item.count.text = countItem.ToString();
            else
                inventory.cells[cellID].item.gameObject.SetActive(false);

            if (cell.item.gameObject.activeInHierarchy)
            {
                int countProduct = int.Parse(cell.item.count.text) + count;
                cell.item.count.text = countProduct.ToString();
            }
            else
                BuyProduct(cell, productDataID, count);
        }
    }

    /// <summary>
    /// �������� ������
    /// </summary>
    /// <param name="cellID">������������� ������</param>
    /// <param name="itemDataID">������������� ������ ��������</param>
    /// <param name="productDataID">������������� ������ ������</param>
    /// <param name="price">���� ������</param>
    /// <returns>������</returns>
    private Cell GetCell(int cellID, int itemDataID, int productDataID, int price)
    {
        Inventory inventory = GameObject.Find("WorldUI").GetComponentInChildren<Inventory>();
        List<Cell> cells = inventory.cells;

        Item item = this.inventory.cells[cellID].item;

        if (item.gameObject.activeInHierarchy && item.data.id == itemDataID && int.Parse(item.count.text) >= price)
        {
            foreach (Cell cell in cells)
                if (cell.item.gameObject.activeInHierarchy)
                {
                    if (cell.item.data.id == productDataID && cell.item.data.type == "Consumables")
                        return cell;
                }
                else
                    return cell;
        }

        return null;
    }

    /// <summary>
    /// ������ �����
    /// </summary>
    /// <param name="cell">������</param>
    /// <param name="productDataID">������������� ������ ������</param>
    /// <param name="count">���������� �������</param>
    private void BuyProduct(Cell cell, int productDataID, int count)
    {
        cell.item.data.id = inventory.data.itemsData[productDataID].id;
        cell.item.data.name = inventory.data.itemsData[productDataID].name;
        cell.item.data.type = inventory.data.itemsData[productDataID].type;
        cell.item.data.durability = inventory.data.itemsData[productDataID].durability;
        cell.item.data.damage = inventory.data.itemsData[productDataID].damage;
        cell.item.data.sprite = inventory.data.itemsData[productDataID].sprite;

        cell.item.image.sprite = cell.item.data.sprite;
        cell.item.count.text = count.ToString();
        cell.item.maxDurability = cell.item.data.durability;

        cell.item.gameObject.SetActive(true);
    }
}