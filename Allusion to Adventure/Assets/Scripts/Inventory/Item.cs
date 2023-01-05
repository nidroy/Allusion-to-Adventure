using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// предмет
/// </summary>
public class Item : MonoBehaviour
{
    public ItemData data; // данные предмета

    public Image image; // изображение
    public TMP_Text count; // количество
    public Image durability; // прочность

    public float maxDurability; // максимальна€ прочность

    public bool isSelected; // €вл€етс€ ли предмет выбранным

    private IMoving typeOfMoving; // тип перемещени€


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
    /// выбрать предмет
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
    /// уничтожить предмет
    /// </summary>
    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// отпустить предмет
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
    /// отобразить количество предметов
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
    /// отобразить прочность предмета
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
    /// передвигатьс€
    /// </summary>
    private void Move()
    {
        if (isSelected)
            typeOfMoving.Move();
    }

    /// <summary>
    /// сломать предмет
    /// </summary>
    private void BreakItem()
    {
        if ((data.type == "Weapon" && data.durability <= 0) || (data.type == "Armor" && data.durability <= 0))
            gameObject.SetActive(false);
    }

    #region существование €чейки и предмета
    /// <summary>
    /// €чейка существует
    /// </summary>
    /// <param name="cell">€чейка</param>
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
    /// предмет в €чейке существует
    /// </summary>
    /// <param name="cell">€чейка</param>
    /// <param name="emptyCell">пуста€ €чейка</param>
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

    #region стак и свап
    /// <summary>
    /// стакнуть предметы в €чейке
    /// </summary>
    /// <param name="cell">€чейка</param>
    /// <param name="item">предмет</param>
    private void StackItems(Cell cell, Item item)
    {
        item.gameObject.SetActive(false);

        int count = int.Parse(cell.item.count.text) + int.Parse(this.count.text);
        cell.item.count.text = count.ToString();
    }

    /// <summary>
    /// свапнуть предметы в €чейке
    /// </summary>
    /// <param name="cell">€чейка</param>
    private void SwapItems(Cell cell)
    {
        cell.item.DestroyItem();
        CreateItemCopy(this, cell);
    }
    #endregion

    #region создать копию предмета
    /// <summary>
    /// создать копию предмета
    /// </summary>
    /// <returns>копи€ предмета</returns>
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
    /// создать копию предмета
    /// </summary>
    /// <param name="item">предмет</param>
    /// <param name="cell">€чейка</param>
    /// <returns>копи€ предмета</returns>
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

    #region получить €чейку
    /// <summary>
    /// получить выбранную €чейку
    /// </summary>
    /// <returns>выбранна€ €чейка</returns>
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
    /// получить пустую €чейку
    /// </summary>
    /// <returns>пуста€ €чейка</returns>
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
