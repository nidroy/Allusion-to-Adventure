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

    public GameObject workObject; // ������� ������

    public GameObject Card; // �������� ���������

    public Characteristics characteristics; // ��������������
    public Equipment equipment; // ����������

    public float[] border; // ������� ������������

    private Actions actions; // ��������

    private bool isGround; // ��������� �� �������� �� �����?


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
    /// �������������
    /// </summary>
    private void Move()
    {
        if (isGround && !anim.GetBool("isDie") && characteristics.typeOfMoving != "")
            actions.movingsDictionary[characteristics.typeOfMoving].Move();
    }

    /// <summary>
    /// ����������������� � ��������
    /// </summary>
    private void InteractWithObject()
    {
        actions.interactionsWithObjectDictionary["DetectionObject"].InteractWithObject();
        if (characteristics.type != "Peaceful")
            actions.interactionsWithObjectDictionary["AttackingObject"].InteractWithObject();
    }

    /// <summary>
    /// ������� ����
    /// </summary>
    private void DealDamage()
    {
        if (enemy != null)
            DealDamageEnemy();
        else if (workObject != null)
            DealDamageWood();
    }

    /// <summary>
    /// ������� ���� �����
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
    /// ������� ���� ������
    /// </summary>
    private void DealDamageWood()
    {
        workObject.GetComponent<Wood>().healthPoints -= characteristics.damage;
        equipment.weapon.data.durability -= 1;
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
    /// �������� ��� ���������
    /// </summary>
    /// <param name="character">��������</param>
    public void UpdateName(Character character)
    {
        character.transform.Find("UI/Name").GetComponent<TMP_Text>().text = character.name;
    }

    /// <summary>
    /// �������� �������������� ���������
    /// </summary>
    /// <param name="character">��������</param>
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
    /// �������� ��� ����������� ���������
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

        UpdateWeapon(equipment.cells[0]);
        UpdateArmor(equipment.cells[1]);
    }

    /// <summary>
    /// �������� ������
    /// </summary>
    /// <param name="cell">������</param>
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
    /// �������� �����
    /// </summary>
    /// <param name="cell">������</param>
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
/// ��������
/// </summary>
public class Actions
{
    public Dictionary<string, IMoving> movingsDictionary = new Dictionary<string, IMoving>(); // ������� �����������
    public Dictionary<string, IInteractionWithObject> interactionsWithObjectDictionary = new Dictionary<string, IInteractionWithObject>(); // ������� �������������� � ��������


    /// <summary>
    /// ��������� �������
    /// </summary>
    /// <param name="character">��������</param>
    public void FillDictionaries(Character character)
    {
        FillMovingsDictionary(character);
        FillInteractionsWithObjectDictionary(character);
    }

    /// <summary>
    /// ��������� ������� �����������
    /// </summary>
    /// <param name="character">��������</param>
    private void FillMovingsDictionary(Character character)
    {
        movingsDictionary.Clear();

        movingsDictionary.Add("Patrolling", new Patrolling(character));
        movingsDictionary.Add("Following", new Following(character));
        movingsDictionary.Add("Escape", new Escape(character));
        movingsDictionary.Add("GoToWork", new GoToWork(character));
    }

    /// <summary>
    /// ��������� ������� �������������� � ��������
    /// </summary>
    /// <param name="character">��������</param>
    private void FillInteractionsWithObjectDictionary(Character character)
    {
        interactionsWithObjectDictionary.Clear();

        interactionsWithObjectDictionary.Add("DetectionObject", new DetectionObject(character));
        interactionsWithObjectDictionary.Add("AttackingObject", new AttackingObject(character));
    }
}