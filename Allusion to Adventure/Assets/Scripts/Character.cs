using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��������
/// </summary>
public class Character : MonoBehaviour
{
    public Rigidbody2D rb; // ������
    public Animator anim; // ��������

    public Image healthPoints; // ���� ��������

    public Character enemy; // ����

    public GameObject Card; // �������� ���������

    public Characteristics characteristics; // ��������������
    public Equipment equipment; // ����������

    public float[] border; // ������� ������������

    private Actions actions; // ��������

    private bool isGround; // ��������� �� �������� �� �����?


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
    /// �������������
    /// </summary>
    private void Move()
    {
        if (isGround)
            actions.movingsDictionary[characteristics.typeOfMoving].Move();
    }

    /// <summary>
    /// ����������������� � �����������
    /// </summary>
    private void InteractWithEnemy()
    {
        actions.interactionsWithEnemyDictionary["DetectionEnemy"].InteractWithEnemy();
        if (characteristics.type != "Peaceful")
            actions.interactionsWithEnemyDictionary["AttackingEnemy"].InteractWithEnemy();
    }

    /// <summary>
    /// ������� ����
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
    /// �������
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
    /// ���������� ���������
    /// </summary>
    private void DestroyCharacter()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// ��������������� � ��������� �������
    /// </summary>
    /// <param name="collision">�������� �������</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGround = true;
    }

    /// <summary>
    /// ���������� ������� � ��������� �������
    /// </summary>
    /// <param name="collision">�������� �������</param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGround = false;
    }
}


/// <summary>
/// ��������������
/// </summary>
[System.Serializable]
public class Characteristics
{
    public string type; // ���
    public string typeOfMoving; // ��� �����������

    public float healthPoints; // ���� ��������
    public float maxHealthPoints; // ������������ ���������� ����� ��������

    public float armor; // �����
    public float damage; // ����

    public float detectionRange; // ��������� �����������
    public float attackRange; // ��������� �����

    public float moveSpeed; // �������� ������������
    public float attackSpeed; // �������� �����

    public float waitingTime; // ����� ��������


    /// <summary>
    /// ���������� ��� ���������
    /// </summary>
    /// <param name="character">��������</param>
    public void SetName(Character character)
    {
        character.transform.Find("UI/Name").GetComponent<TMP_Text>().text = character.name;
    }

    /// <summary>
    /// �������� �������������� ���������
    /// </summary>
    /// <param name="character">��������</param>
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
    /// �������� �������� ������������� ���������
    /// </summary>
    /// <param name="character">��������</param>
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
    /// �������� ��� ���������
    /// </summary>
    /// <param name="character">��������</param>
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
    /// �������� ��� ����������� ���������
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
    /// �������� ���� �������� ���������
    /// </summary>
    /// <param name="character">��������</param>
    private void UpdateHealthPoints(Character character)
    {
        HealthRegen(character);
        character.healthPoints.fillAmount = healthPoints / maxHealthPoints;
    }

    /// <summary>
    /// �������������� �������� ���������
    /// </summary>
    /// <param name="character">��������</param>
    private void HealthRegen(Character character)
    {
        UseHealingPotion(character);

        if (healthPoints < maxHealthPoints)
            healthPoints += 0.005f * maxHealthPoints * Time.deltaTime;
        else
            healthPoints = maxHealthPoints;
    }

    /// <summary>
    /// ������������ �������� �����
    /// </summary>
    /// <param name="character">��������</param>
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
    /// �������� �����
    /// </summary>
    /// <param name="equipment">����������</param>
    private void UpdateArmor(Equipment equipment)
    {
        if (equipment.armor == null)
            armor = 0;
        else
            armor = equipment.armor.data.durability;
    }

    /// <summary>
    /// �������� ����
    /// </summary>
    /// <param name="equipment">����������</param>
    private void UpdateDamage(Equipment equipment)
    {
        if (equipment.weapon == null)
            damage = 0;
        else
            damage = equipment.weapon.data.damage;
    }
}

/// <summary>
/// ����������
/// </summary>
[System.Serializable]
public class Equipment
{
    public Item weapon; // ������
    public Item armor; // �����


    /// <summary>
    /// �������� ���������� ���������
    /// </summary>
    /// <param name="character">��������</param>
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
    /// �������� ������
    /// </summary>
    /// <param name="cell">������</param>
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
    /// �������� �����
    /// </summary>
    /// <param name="cell">������</param>
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
/// ��������
/// </summary>
public class Actions
{
    public Dictionary<string, IMoving> movingsDictionary = new Dictionary<string, IMoving>(); // ������� �����������
    public Dictionary<string, IInteractionWithEnemy> interactionsWithEnemyDictionary = new Dictionary<string, IInteractionWithEnemy>(); // ������� �������������� � �����������


    /// <summary>
    /// ��������� ������� �����������
    /// </summary>
    /// <param name="character">��������</param>
    public void FillMovingsDictionary(Character character)
    {
        movingsDictionary.Clear();

        movingsDictionary.Add("Patrolling", new Patrolling(character));
        movingsDictionary.Add("Following", new Following(character));
        movingsDictionary.Add("Escape", new Escape(character));
    }

    /// <summary>
    /// ��������� ������� �������������� � �����������
    /// </summary>
    /// <param name="character">��������</param>
    public void FillInteractionsWithEnemyDictionary(Character character)
    {
        interactionsWithEnemyDictionary.Clear();

        interactionsWithEnemyDictionary.Add("DetectionEnemy", new DetectionEnemy(character));
        interactionsWithEnemyDictionary.Add("AttackingEnemy", new AttackingEnemy(character));
    }
}