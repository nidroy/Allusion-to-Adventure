using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// данные инвентар€
/// </summary>
public class InventoryData : MonoBehaviour
{
    public static List<Cell> cellsData = new List<Cell>(); // данные €чеек

    public List<ItemData> itemsData; // данные предметов
}


/// <summary>
/// данные предмета
/// </summary>
[System.Serializable]
public class ItemData
{
    public int id; // идентификатор

    public string name; // название
    public string type; // тип

    public float durability; // прочность
    public float damage; // урон

    public Sprite sprite; // изображение
}
