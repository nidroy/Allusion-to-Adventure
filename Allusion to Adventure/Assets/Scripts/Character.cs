using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// персонаж
/// </summary>
public class Character : MonoBehaviour
{
    public Rigidbody2D rb; // физика
    public Animator anim; // анимации

    public Image healthPoints; // очки здоровья

    public Character enemy; // враг

    public string type; // тип
    public string typeOfMoving; // тип перемещения

    public Inventory inventory; // инвентарь
    public Characteristics characteristics; // характеристики
    public Equipment equipment; // снаряжение

    public float[] border; // границы передвижения

    private Actions actions; // действия

    private float maxHealthPoints; // максимальное количество очков здоровья

    private bool isGround; // находится ли персонаж на земле?


    private void Start()
    {
        SetMaxHealthPoints(characteristics.healthPoints);

        actions = new Actions();
        actions.FillMovingsDictionary(this);
        actions.FillInteractionsWithEnemyDictionary(this);
    }

    private void Update()
    {
        UpdateHealthPoints();
        UpdateType();
        UpdateEquipment();
        InteractWithEnemy();
        UpdateTypeOfMoving();
        Die();
    }

    private void FixedUpdate()
    {
        Move();
    }


    /// <summary>
    /// установить максимальное количество очков здоровья
    /// </summary>
    public void SetMaxHealthPoints(float healthPoints)
    {
        maxHealthPoints = healthPoints;
    }

    /// <summary>
    /// обновить очки здоровья
    /// </summary>
    private void UpdateHealthPoints()
    {
        HealthRegen();
        healthPoints.fillAmount = characteristics.healthPoints / maxHealthPoints;
    }

    /// <summary>
    /// восстановление здоровья
    /// </summary>
    private void HealthRegen()
    {
        UseHealingPotion();

        if (characteristics.healthPoints < maxHealthPoints)
            characteristics.healthPoints += 0.005f * maxHealthPoints * Time.deltaTime;
        else
            characteristics.healthPoints = maxHealthPoints;
    }

    /// <summary>
    /// использовать целебное зелье
    /// </summary>
    private void UseHealingPotion()
    {
        List<Cell> cells = inventory.cells;

        foreach (Cell cell in cells)
        {
            if (cell.item != null)
                if (cell.item.gameObject.activeInHierarchy && cell.item.data.name == "Healing potion" && characteristics.healthPoints < maxHealthPoints - cell.item.data.durability)
                {
                    if (int.Parse(cell.item.count.text) > 1)
                    {
                        characteristics.healthPoints += cell.item.data.durability;
                        cell.item.count.text = (int.Parse(cell.item.count.text) - 1).ToString();
                    }
                    else
                    {
                        characteristics.healthPoints += cell.item.data.durability;
                        cell.item.gameObject.SetActive(false);
                    }
                }
        }
    }

    /// <summary>
    /// обновить тип персонажа
    /// </summary>
    private void UpdateType()
    {
        if (equipment.weapon == null)
            type = "Peaceful";
        else
            type = "Swordsman";

        UpdateTypeAnim();
    }

    /// <summary>
    /// обновить анимацию типа персонажа
    /// </summary>
    private void UpdateTypeAnim()
    {
        if (type == "Peaceful")
            anim.SetInteger("type", 0);
        else if (type == "Swordsman")
            anim.SetInteger("type", 1);
    }

    /// <summary>
    /// обновить снаряжение
    /// </summary>
    private void UpdateEquipment()
    {
        Inventory equipment = inventory.transform.Find("Equipment").GetComponent<Inventory>();
        List<Cell> cells = equipment.cells;

        foreach (Cell cell in cells)
        {
            UpdateWeapon(cell);
            UpdateArmor(cell);
        }

    }

    /// <summary>
    /// обновить оружие
    /// </summary>
    /// <param name="cell">ячейка</param>
    private void UpdateWeapon(Cell cell)
    {
        if (cell.tag == "Weapon")
        {
            if (cell.item == null || !cell.item.gameObject.activeInHierarchy)
                equipment.weapon = null;
            else
                equipment.weapon = cell.item;
        }
    }

    /// <summary>
    /// обновить броню
    /// </summary>
    /// <param name="cell">ячейка</param>
    private void UpdateArmor(Cell cell)
    {
        if (cell.tag == "Armor")
        {
            if (cell.item == null || !cell.item.gameObject.activeInHierarchy)
                equipment.armor = null;
            else
                equipment.armor = cell.item;
        }
    }

    /// <summary>
    /// обновить тип перемещения
    /// </summary>
    private void UpdateTypeOfMoving()
    {
        if (enemy == null)
            typeOfMoving = "Patrolling";
        else
        {
            if (type == "Peaceful")
                typeOfMoving = "Escape";
            else
                typeOfMoving = "Following";
        }
    }

    /// <summary>
    /// передвигаться
    /// </summary>
    private void Move()
    {
        if (isGround)
            actions.movingsDictionary[typeOfMoving].Move();
    }

    /// <summary>
    /// взаимодействовать с противником
    /// </summary>
    private void InteractWithEnemy()
    {
        actions.interactionsWithEnemyDictionary["DetectionEnemy"].InteractWithEnemy();
        if (type != "Peaceful")
            actions.interactionsWithEnemyDictionary["AttackingEnemy"].InteractWithEnemy();
    }

    /// <summary>
    /// нанести урон
    /// </summary>
    private void DealDamage()
    {
        if (enemy != null && equipment.weapon != null)
        {
            if (enemy.equipment.armor == null)
                enemy.characteristics.healthPoints -= equipment.weapon.data.damage;
            else
                enemy.equipment.armor.data.durability -= equipment.weapon.data.damage;

            equipment.weapon.data.durability -= 1;
        }
    }

    /// <summary>
    /// умереть
    /// </summary>
    private void Die()
    {
        if (characteristics.healthPoints <= 0)
        {
            characteristics.moveSpeed = 0;
            anim.speed = 1f;
            anim.SetBool("isDie", true);
        }
    }

    /// <summary>
    /// уничтожить персонажа
    /// </summary>
    private void DestroyCharacter()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// соприкосновение с коллизией объекта
    /// </summary>
    /// <param name="collision">коллизия объекта</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGround = true;
    }

    /// <summary>
    /// прекратить контакт с коллизией объекта
    /// </summary>
    /// <param name="collision">коллизия объекта</param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGround = false;
    }
}


/// <summary>
/// характеристики
/// </summary>
[System.Serializable]
public class Characteristics
{
    public float healthPoints; // очки здоровья

    public float detectionRange; // дальность обнаружения
    public float attackRange; // дальность атаки

    public float moveSpeed; // скорость передвижения
    public float attackSpeed; // скорость атаки

    public float waitingTime; // время ожидания
}

/// <summary>
/// снаряжение
/// </summary>
[System.Serializable]
public class Equipment
{
    public Item weapon; // оружие
    public Item armor; // броня
}

/// <summary>
/// действия
/// </summary>
public class Actions
{
    public Dictionary<string, IMoving> movingsDictionary = new Dictionary<string, IMoving>(); // словарь перемещений
    public Dictionary<string, IInteractionWithEnemy> interactionsWithEnemyDictionary = new Dictionary<string, IInteractionWithEnemy>(); // словарь взаимодействий с противником


    /// <summary>
    /// заполнить словарь перемещений
    /// </summary>
    /// <param name="character">персонаж</param>
    public void FillMovingsDictionary(Character character)
    {
        movingsDictionary.Clear();

        movingsDictionary.Add("Patrolling", new Patrolling(character));
        movingsDictionary.Add("Following", new Following(character));
        movingsDictionary.Add("Escape", new Escape(character));
    }

    /// <summary>
    /// заполнить словарь взаимодействий с противником
    /// </summary>
    /// <param name="character">персонаж</param>
    public void FillInteractionsWithEnemyDictionary(Character character)
    {
        interactionsWithEnemyDictionary.Clear();

        interactionsWithEnemyDictionary.Add("DetectionEnemy", new DetectionEnemy(character));
        interactionsWithEnemyDictionary.Add("AttackingEnemy", new AttackingEnemy(character));
    }
}