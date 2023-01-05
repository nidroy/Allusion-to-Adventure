using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������
/// </summary>
public class Item : MonoBehaviour
{
    public ItemData data; // ������ ��������

    public Image image; // �����������
    public TMP_Text count; // ����������
    public Image durability; // ���������

    public float maxDurability; // ������������ ���������

    public bool isSelected; // �������� �� ������� ���������

    private IMoving typeOfMoving; // ��� �����������


    private void Start()
    {
        typeOfMoving = new MovingItem(this);
    }

    private void Update()
    {
        DisplayCount();
        DisplayDurability();
        Move();
        BreakItem();
    }


    /// <summary>
    /// ������� �������
    /// </summary>
    public void SelectItem()
    {
        if (isSelected)
            DropItem();
        else
            CreateItemCopy();

        DestroyItem();
    }

    /// <summary>
    /// ���������� �������
    /// </summary>
    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// ��������� �������
    /// </summary>
    private void DropItem()
    {
        Cell cell = GetSelectedCell();

        if (cell == null)
        {
            Cell emptyCell = GetEmptyCell();
            CreateItemCopy(this, emptyCell);
        }
        else
            CellExist(cell);
    }

    /// <summary>
    /// ���������� ���������� ���������
    /// </summary>
    private void DisplayCount()
    {
        if (data.type == "Consumables")
        {
            durability.gameObject.SetActive(false);

            if (int.Parse(count.text) > 1)
                count.gameObject.SetActive(true);
            else
                count.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ���������� ��������� ��������
    /// </summary>
    private void DisplayDurability()
    {
        if (data.type == "Weapon" || data.type == "Armor")
        {
            count.gameObject.SetActive(false);
            durability.gameObject.SetActive(true);

            Image durabilityCount = durability.transform.Find("Count").GetComponent<Image>();
            durabilityCount.fillAmount = data.durability / maxDurability;
        }
    }

    /// <summary>
    /// �������������
    /// </summary>
    private void Move()
    {
        if (isSelected)
            typeOfMoving.Move();
    }

    /// <summary>
    /// ������� �������
    /// </summary>
    private void BreakItem()
    {
        if ((data.type == "Weapon" && data.durability <= 0) || (data.type == "Armor" && data.durability <= 0))
            gameObject.SetActive(false);
    }

    #region ������������� ������ � ��������
    /// <summary>
    /// ������ ����������
    /// </summary>
    /// <param name="cell">������</param>
    private void CellExist(Cell cell)
    {
        string inventoryName = cell.transform.parent.name;
        Cell emptyCell = GetEmptyCell();

        if ((inventoryName == "Equipment" && cell.tag == data.type) || (inventoryName != "Equipment"))
        {
            if (cell.item == null)
                CreateItemCopy(this, cell);
            else
                ItemExist(cell, emptyCell);
        }
        else
            CreateItemCopy(this, emptyCell);
    }

    /// <summary>
    /// ������� � ������ ����������
    /// </summary>
    /// <param name="cell">������</param>
    /// <param name="emptyCell">������ ������</param>
    private void ItemExist(Cell cell, Cell emptyCell)
    {
        if ((cell.item.data.type == "Consumables" && emptyCell.tag == "Weapon") || (cell.item.data.type == "Consumables" && emptyCell.tag == "Armor"))
            CreateItemCopy();
        else
        {
            Item item = CreateItemCopy(cell.item, emptyCell);

            if (cell.item.data.id == data.id && cell.item.gameObject.activeInHierarchy && data.type == "Consumables")
                StackItems(cell, item);
            else
                SwapItems(cell);
        }
    }
    #endregion

    #region ���� � ����
    /// <summary>
    /// �������� �������� � ������
    /// </summary>
    /// <param name="cell">������</param>
    /// <param name="item">�������</param>
    private void StackItems(Cell cell, Item item)
    {
        item.gameObject.SetActive(false);

        int count = int.Parse(cell.item.count.text) + int.Parse(this.count.text);
        cell.item.count.text = count.ToString();
    }

    /// <summary>
    /// �������� �������� � ������
    /// </summary>
    /// <param name="cell">������</param>
    private void SwapItems(Cell cell)
    {
        cell.item.DestroyItem();
        CreateItemCopy(this, cell);
    }
    #endregion

    #region ������� ����� ��������
    /// <summary>
    /// ������� ����� ��������
    /// </summary>
    /// <returns>����� ��������</returns>
    private Item CreateItemCopy()
    {
        Transform worldUI = GameObject.Find("WorldUI").transform;

        Item itemCopy = Instantiate(this, worldUI);
        itemCopy.transform.position = Input.mousePosition;
        itemCopy.transform.SetAsLastSibling();
        itemCopy.name = name;
        itemCopy.isSelected = true;

        return itemCopy;
    }

    /// <summary>
    /// ������� ����� ��������
    /// </summary>
    /// <param name="item">�������</param>
    /// <param name="cell">������</param>
    /// <returns>����� ��������</returns>
    private Item CreateItemCopy(Item item, Cell cell)
    {
        Item itemCopy = Instantiate(item, cell.transform);
        itemCopy.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        itemCopy.name = item.name;
        itemCopy.isSelected = false;

        cell.item = itemCopy;

        return itemCopy;
    }
    #endregion

    #region �������� ������
    /// <summary>
    /// �������� ��������� ������
    /// </summary>
    /// <returns>��������� ������</returns>
    private Cell GetSelectedCell()
    {
        Cell[] cells = FindObjectsOfType<Cell>();

        foreach (Cell cell in cells)
        {
            float width = cell.GetComponent<RectTransform>().rect.width / 2;
            float height = cell.GetComponent<RectTransform>().rect.height / 2;

            Vector2 position = cell.transform.position;

            if (transform.position.x < position.x + width && transform.position.x > position.x - width &&
                transform.position.y < position.y + height && transform.position.y > position.y - height)
                return cell;
        }

        return null;
    }

    /// <summary>
    /// �������� ������ ������
    /// </summary>
    /// <returns>������ ������</returns>
    private Cell GetEmptyCell()
    {
        Cell[] cells = FindObjectsOfType<Cell>();

        foreach (Cell cell in cells)
            if (cell.item == null)
                return cell;

        return null;
    }
    #endregion
}
