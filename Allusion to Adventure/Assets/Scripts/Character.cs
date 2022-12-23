using System.Collections.Generic;
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

    public string type; // ���
    public string typeOfMoving; // ��� �����������

    public Inventory inventory; // ���������
    public Characteristics characteristics; // ��������������
    public Equipment equipment; // ����������

    public float[] border; // ������� ������������

    private Actions actions; // ��������

    private float maxHealthPoints; // ������������ ���������� ����� ��������

    private bool isGround; // ��������� �� �������� �� �����?


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
    /// ���������� ������������ ���������� ����� ��������
    /// </summary>
    public void SetMaxHealthPoints(float healthPoints)
    {
        maxHealthPoints = healthPoints;
    }

    /// <summary>
    /// �������� ���� ��������
    /// </summary>
    private void UpdateHealthPoints()
    {
        HealthRegen();
        healthPoints.fillAmount = characteristics.healthPoints / maxHealthPoints;
    }

    /// <summary>
    /// �������������� ��������
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
    /// ������������ �������� �����
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
    /// �������� ��� ���������
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
    /// �������� �������� ���� ���������
    /// </summary>
    private void UpdateTypeAnim()
    {
        if (type == "Peaceful")
            anim.SetInteger("type", 0);
        else if (type == "Swordsman")
            anim.SetInteger("type", 1);
    }

    /// <summary>
    /// �������� ����������
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
    /// �������� ������
    /// </summary>
    /// <param name="cell">������</param>
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
    /// �������� �����
    /// </summary>
    /// <param name="cell">������</param>
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
    /// �������� ��� �����������
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
    /// �������������
    /// </summary>
    private void Move()
    {
        if (isGround)
            actions.movingsDictionary[typeOfMoving].Move();
    }

    /// <summary>
    /// ����������������� � �����������
    /// </summary>
    private void InteractWithEnemy()
    {
        actions.interactionsWithEnemyDictionary["DetectionEnemy"].InteractWithEnemy();
        if (type != "Peaceful")
            actions.interactionsWithEnemyDictionary["AttackingEnemy"].InteractWithEnemy();
    }

    /// <summary>
    /// ������� ����
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
    public float healthPoints; // ���� ��������

    public float detectionRange; // ��������� �����������
    public float attackRange; // ��������� �����

    public float moveSpeed; // �������� ������������
    public float attackSpeed; // �������� �����

    public float waitingTime; // ����� ��������
}

/// <summary>
/// ����������
/// </summary>
[System.Serializable]
public class Equipment
{
    public Item weapon; // ������
    public Item armor; // �����
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