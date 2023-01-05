using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ���������
/// </summary>
public class InventoryData : MonoBehaviour
{
    public static List<Cell> cellsData = new List<Cell>(); // ������ �����

    public List<ItemData> itemsData; // ������ ���������
}


/// <summary>
/// ������ ��������
/// </summary>
[System.Serializable]
public class ItemData
{
    public int id; // �������������

    public string name; // ��������
    public string type; // ���

    public float durability; // ���������
    public float damage; // ����

    public Sprite sprite; // �����������
}
