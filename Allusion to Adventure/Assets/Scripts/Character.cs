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

    public GameObject workObject; // рабочий объект

    public GameObject Card; // карточка персонажа

    public Characteristics characteristics; // характеристики
    public Equipment equipment; // снаряжение

    public float[] border; // границы передвижения

    private Actions actions; // действия

    private bool isGround; // находится ли персонаж на земле?


    private void Start()
    {
        actions = new Actions();
        actions.FillDictionaries(this);
    }

    private void Update()
    {
        if (!anim.GetBool("isDie"))
        {
            characteristics.UpdateCharacteristics(this);
            equipment.UpdateEquipment(this);

            InteractWithObject();
            Die();
        }
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
        if (isGround && !anim.GetBool("isDie") && characteristics.typeOfMoving != "")
            actions.movingsDictionary[characteristics.typeOfMoving].Move();
    }

    /// <summary>
    /// взаимодействовать с объектом
    /// </summary>
    private void InteractWithObject()
    {
        actions.interactionsWithObjectDictionary["DetectionObject"].InteractWithObject();
        if (characteristics.type != "Peaceful")
            actions.interactionsWithObjectDictionary["AttackingObject"].InteractWithObject();
    }

    /// <summary>
    /// нанести урон
    /// </summary>
    private void DealDamage()
    {
        if (enemy != null)
            DealDamageEnemy();
        else if (workObject != null)
            DealDamageWood();
    }

    /// <summary>
    /// нанести урон врагу
    /// </summary>
    private void DealDamageEnemy()
    {
        if (enemy.characteristics.armor > 0)
            enemy.equipment.armor.data.durability -= characteristics.damage;
        else
            enemy.characteristics.healthPoints -= characteristics.damage;

        equipment.weapon.data.durability -= 1;
    }

    /// <summary>
    /// нанести урон дереву
    /// </summary>
    private void DealDamageWood()
    {
        workObject.GetComponent<Wood>().healthPoints -= characteristics.damage;
        equipment.weapon.data.durability -= 1;
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
    /// обновить имя персонажа
    /// </summary>
    /// <param name="character">персонаж</param>
    public void UpdateName(Character character)
    {
        character.transform.Find("UI/Name").GetComponent<TMP_Text>().text = character.name;
    }

    /// <summary>
    /// обновить характеристики персонажа
    /// </summary>
    /// <param name="character">персонаж</param>
    public void UpdateCharacteristics(Character character)
    {
        UpdateName(character);
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
            if (character.equipment.weapon.data.id == 0)
            {
                type = "Swordsman";
                character.anim.SetInteger("type", 1);
            }
            else if (character.equipment.weapon.data.id == 3)
            {
                type = "Woodman";
                character.anim.SetInteger("type", 2);
            }
        }
    }

    /// <summary>
    /// обновить тип перемещения персонажа
    /// </summary>
    private void UpdateTypeOfMoving(Character character)
    {
        if (character.enemy == null)
        {
            if (character.workObject != null && Timer.hour >= 8 && Timer.hour <= 20)
                typeOfMoving = "GoToWork";
            else
                typeOfMoving = "Patrolling";
        }
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
                if (cell.item.gameObject.activeInHierarchy && cell.item.data.id == 2 && healthPoints < maxHealthPoints - cell.item.data.durability)
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

        UpdateWeapon(equipment.cells[0]);
        UpdateArmor(equipment.cells[1]);
    }

    /// <summary>
    /// обновить оружие
    /// </summary>
    /// <param name="cell">ячейка</param>
    private void UpdateWeapon(Cell cell)
    {
        if (cell.gameObject.activeInHierarchy)
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
        if (cell.gameObject.activeInHierarchy)
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
    public Dictionary<string, IInteractionWithObject> interactionsWithObjectDictionary = new Dictionary<string, IInteractionWithObject>(); // словарь взаимодействий с объектом


    /// <summary>
    /// заполнить словари
    /// </summary>
    /// <param name="character">персонаж</param>
    public void FillDictionaries(Character character)
    {
        FillMovingsDictionary(character);
        FillInteractionsWithObjectDictionary(character);
    }

    /// <summary>
    /// заполнить словарь перемещений
    /// </summary>
    /// <param name="character">персонаж</param>
    private void FillMovingsDictionary(Character character)
    {
        movingsDictionary.Clear();

        movingsDictionary.Add("Patrolling", new Patrolling(character));
        movingsDictionary.Add("Following", new Following(character));
        movingsDictionary.Add("Escape", new Escape(character));
        movingsDictionary.Add("GoToWork", new GoToWork(character));
    }

    /// <summary>
    /// заполнить словарь взаимодействий с объектом
    /// </summary>
    /// <param name="character">персонаж</param>
    private void FillInteractionsWithObjectDictionary(Character character)
    {
        interactionsWithObjectDictionary.Clear();

        interactionsWithObjectDictionary.Add("DetectionObject", new DetectionObject(character));
        interactionsWithObjectDictionary.Add("AttackingObject", new AttackingObject(character));
    }
}