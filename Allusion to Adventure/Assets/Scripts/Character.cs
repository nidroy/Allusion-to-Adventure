using System;
using System.Collections.Generic;
using TMPro;
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

    public GameObject Card; // карточка персонажа

    public Characteristics characteristics; // характеристики
    public Equipment equipment; // снаряжение

    public float[] border; // границы передвижения

    private Actions actions; // действия

    private bool isGround; // находится ли персонаж на земле?


    private void Start()
    {
        characteristics.SetName(this);

        actions = new Actions();
        actions.FillMovingsDictionary(this);
        actions.FillInteractionsWithEnemyDictionary(this);
    }

    private void Update()
    {
        characteristics.UpdateCharacteristics(this);
        equipment.UpdateEquipment(this);

        InteractWithEnemy();
        Die();
    }

    private void FixedUpdate()
    {
        Move();
    }


    /// <summary>
    /// передвигаться
    /// </summary>
    private void Move()
    {
        if (isGround)
            actions.movingsDictionary[characteristics.typeOfMoving].Move();
    }

    /// <summary>
    /// взаимодействовать с противником
    /// </summary>
    private void InteractWithEnemy()
    {
        actions.interactionsWithEnemyDictionary["DetectionEnemy"].InteractWithEnemy();
        if (characteristics.type != "Peaceful")
            actions.interactionsWithEnemyDictionary["AttackingEnemy"].InteractWithEnemy();
    }

    /// <summary>
    /// нанести урон
    /// </summary>
    private void DealDamage()
    {
        if (enemy != null && characteristics.damage != 0)
        {
            if (enemy.characteristics.armor > 0)
                enemy.equipment.armor.data.durability -= characteristics.damage;
            else
                enemy.characteristics.healthPoints -= characteristics.damage;

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
    public string type; // тип
    public string typeOfMoving; // тип перемещения

    public float healthPoints; // очки здоровья
    public float maxHealthPoints; // максимальное количество очков здоровья

    public float armor; // броня
    public float damage; // урон

    public float detectionRange; // дальность обнаружения
    public float attackRange; // дальность атаки

    public float moveSpeed; // скорость передвижения
    public float attackSpeed; // скорость атаки

    public float waitingTime; // время ожидания


    /// <summary>
    /// установить имя персонажа
    /// </summary>
    /// <param name="character">персонаж</param>
    public void SetName(Character character)
    {
        character.transform.Find("UI/Name").GetComponent<TMP_Text>().text = character.name;
    }

    /// <summary>
    /// обновить характеристики персонажа
    /// </summary>
    /// <param name="character">персонаж</param>
    public void UpdateCharacteristics(Character character)
    {
        UpdateType(character);
        UpdateTypeOfMoving(character);
        UpdateHealthPoints(character);
        UpdateArmor(character.equipment);
        UpdateDamage(character.equipment);

        UpdateCharacteristicsCard(character);
    }

    /// <summary>
    /// обновить карточку характеристик персонажа
    /// </summary>
    /// <param name="character">персонаж</param>
    private void UpdateCharacteristicsCard(Character character)
    {
        GameObject characteristicsCard = character.Card.transform.Find("Characteristics").gameObject;

        if (characteristicsCard.activeInHierarchy)
        {
            characteristicsCard.transform.Find("Name").GetComponentInChildren<TMP_Text>().text = character.name;
            characteristicsCard.transform.Find("Type").GetComponentInChildren<TMP_Text>().text = type;
            characteristicsCard.transform.Find("Action").GetComponentInChildren<TMP_Text>().text = typeOfMoving;
            characteristicsCard.transform.Find("HealthPoints").GetComponentInChildren<TMP_Text>().text = Math.Round(healthPoints, 1).ToString();
            characteristicsCard.transform.Find("Armor").GetComponentInChildren<TMP_Text>().text = Math.Round(armor, 1).ToString();
            characteristicsCard.transform.Find("Damage").GetComponentInChildren<TMP_Text>().text = Math.Round(damage, 1).ToString();
            characteristicsCard.transform.Find("AttackSpeed").GetComponentInChildren<TMP_Text>().text = Math.Round(attackSpeed, 1).ToString();
            characteristicsCard.transform.Find("DetectionRange").GetComponentInChildren<TMP_Text>().text = Math.Round(detectionRange, 1).ToString();
            characteristicsCard.transform.Find("MoveSpeed").GetComponentInChildren<TMP_Text>().text = Math.Round(moveSpeed, 1).ToString();
        }
    }

    /// <summary>
    /// обновить тип персонажа
    /// </summary>
    /// <param name="character">персонаж</param>
    private void UpdateType(Character character)
    {
        if (character.equipment.weapon == null)
        {
            type = "Peaceful";
            character.anim.SetInteger("type", 0);
        }
        else
        {
            type = "Swordsman";
            character.anim.SetInteger("type", 1);
        }
    }

    /// <summary>
    /// обновить тип перемещения персонажа
    /// </summary>
    private void UpdateTypeOfMoving(Character character)
    {
        if (character.enemy == null)
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
    /// обновить очки здоровья персонажа
    /// </summary>
    /// <param name="character">персонаж</param>
    private void UpdateHealthPoints(Character character)
    {
        HealthRegen(character);
        character.healthPoints.fillAmount = healthPoints / maxHealthPoints;
    }

    /// <summary>
    /// восстановление здоровья персонажа
    /// </summary>
    /// <param name="character">персонаж</param>
    private void HealthRegen(Character character)
    {
        UseHealingPotion(character);

        if (healthPoints < maxHealthPoints)
            healthPoints += 0.005f * maxHealthPoints * Time.deltaTime;
        else
            healthPoints = maxHealthPoints;
    }

    /// <summary>
    /// использовать целебное зелье
    /// </summary>
    /// <param name="character">персонаж</param>
    private void UseHealingPotion(Character character)
    {
        Inventory inventory = character.Card.transform.Find("Inventory").GetComponent<Inventory>();
        List<Cell> cells = inventory.cells;

        foreach (Cell cell in cells)
        {
            if (cell.item != null)
                if (cell.item.gameObject.activeInHierarchy && cell.item.data.name == "Healing potion" && healthPoints < maxHealthPoints - cell.item.data.durability)
                {
                    if (int.Parse(cell.item.count.text) > 1)
                    {
                        healthPoints += cell.item.data.durability;
                        cell.item.count.text = (int.Parse(cell.item.count.text) - 1).ToString();
                    }
                    else
                    {
                        healthPoints += cell.item.data.durability;
                        cell.item.gameObject.SetActive(false);
                    }
                }
        }
    }

    /// <summary>
    /// обновить броню
    /// </summary>
    /// <param name="equipment">снаряжение</param>
    private void UpdateArmor(Equipment equipment)
    {
        if (equipment.armor == null)
            armor = 0;
        else
            armor = equipment.armor.data.durability;
    }

    /// <summary>
    /// обновить урон
    /// </summary>
    /// <param name="equipment">снаряжение</param>
    private void UpdateDamage(Equipment equipment)
    {
        if (equipment.weapon == null)
            damage = 0;
        else
            damage = equipment.weapon.data.damage;
    }
}

/// <summary>
/// снаряжение
/// </summary>
[System.Serializable]
public class Equipment
{
    public Item weapon; // оружие
    public Item armor; // броня


    /// <summary>
    /// обновить снаряжение персонажа
    /// </summary>
    /// <param name="character">персонаж</param>
    public void UpdateEquipment(Character character)
    {
        Inventory equipment = character.Card.transform.Find("Equipment").GetComponent<Inventory>();
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
        if (cell.tag == "Weapon" && cell.gameObject.activeInHierarchy)
        {
            if (cell.item == null || !cell.item.gameObject.activeInHierarchy)
                weapon = null;
            else
                weapon = cell.item;
        }
    }

    /// <summary>
    /// обновить броню
    /// </summary>
    /// <param name="cell">ячейка</param>
    private void UpdateArmor(Cell cell)
    {
        if (cell.tag == "Armor" && cell.gameObject.activeInHierarchy)
        {
            if (cell.item == null || !cell.item.gameObject.activeInHierarchy)
                armor = null;
            else
                armor = cell.item;
        }
    }
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