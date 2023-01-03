using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������� �������������� � ��������
/// </summary>
public interface IInteractionWithObject
{
    /// <summary>
    /// ����������������� � ��������
    /// </summary>
    public void InteractWithObject();
}


/// <summary>
/// ����������� �������
/// </summary>
public class DetectionObject : IInteractionWithObject
{
    public Character character; // ��������


    /// <summary>
    /// ����������� ����������� ������
    /// </summary>
    /// <param name="character">��������</param>
    public DetectionObject(Character character)
    {
        this.character = character;
    }


    /// <summary>
    /// ����������������� � ��������
    /// </summary>
    public void InteractWithObject()
    {
        Character character = DetectEnemy();

        if (character == null)
        {
            this.character.enemy = null;

            if (this.character.characteristics.type == "Woodman")
            {
                Wood wood = DetectWood();

                if (wood == null)
                    this.character.workObject = null;
                else
                    this.character.workObject = wood.gameObject;
            }
            else
                this.character.workObject = null;
        }
        else
            this.character.enemy = character;
    }

    /// <summary>
    /// ���������� ����������
    /// </summary>
    /// <returns>��������� ���������</returns>
    private Character DetectEnemy()
    {
        List<Character> detectedEnemies = DetectEnemies();

        if (detectedEnemies.Count > 0)
        {
            Character detectedEnemy = detectedEnemies[0];

            foreach (Character enemy in detectedEnemies)
            {
                float detectedEnemyDistance = Vector2.Distance(character.transform.position, detectedEnemy.transform.position);
                float enemyDistance = Vector2.Distance(character.transform.position, enemy.transform.position);

                if (enemyDistance < detectedEnemyDistance)
                    detectedEnemy = enemy;
            }

            return detectedEnemy;
        }
        else
            return null;
    }

    /// <summary>
    /// ���������� ������
    /// </summary>
    /// <returns>��������� ������</returns>
    private Wood DetectWood()
    {
        Wood[] woods = Object.FindObjectsOfType<Wood>();
        woods = System.Array.FindAll(woods, wood => !wood.isStump);

        if (woods.Length > 0)
        {
            Wood detectedWood = woods[0];

            foreach (Wood wood in woods)
            {
                float detectedWoodDistance = Vector2.Distance(character.transform.position, detectedWood.transform.position);
                float woodDistance = Vector2.Distance(character.transform.position, wood.transform.position);

                if (woodDistance < detectedWoodDistance)
                    detectedWood = wood;
            }

            return detectedWood;
        }
        else
            return null;
    }

    /// <summary>
    /// ���������� �����������
    /// </summary>
    /// <returns>���� �����������</returns>
    private List<Character> DetectEnemies()
    {
        Character[] characters = Object.FindObjectsOfType<Character>();
        List<Character> detectedEnemies = new List<Character>();

        foreach (Character character in characters)
        {
            float characterDistance = Vector2.Distance(character.transform.position, this.character.transform.position);
            if (character.gameObject.tag != this.character.gameObject.tag && characterDistance <= this.character.characteristics.detectionRange && !character.anim.GetBool("isDie"))
                detectedEnemies.Add(character);
        }

        return detectedEnemies;
    }
}

/// <summary>
/// ����� �������
/// </summary>
public class AttackingObject : IInteractionWithObject
{
    public Character character; // ��������


    /// <summary>
    /// ����������� ����� �������
    /// </summary>
    /// <param name="character">��������</param>
    public AttackingObject(Character character)
    {
        this.character = character;
    }


    /// <summary>
    /// ����������������� � ��������
    /// </summary>
    public void InteractWithObject()
    {
        if (character.enemy == null)
        {
            if (character.workObject == null)
                StopAttack();
            else
            {
                float objectDistance = Vector2.Distance(character.transform.position, character.workObject.transform.position);

                if (objectDistance <= character.characteristics.attackRange)
                {
                    RotationToObject(character.workObject.transform.position);
                    StartAttack();
                }
                else
                    StopAttack();
            }
        }
        else
        {
            float enemyDistance = Vector2.Distance(character.transform.position, character.enemy.transform.position);

            if (enemyDistance <= character.characteristics.attackRange)
            {
                RotationToObject(character.enemy.transform.position);
                StartAttack();
            }
            else
                StopAttack();
        }
    }

    /// <summary>
    /// ������ �����
    /// </summary>
    private void StartAttack()
    {
        character.anim.speed = character.characteristics.attackSpeed / 10;
        character.anim.SetBool("isAttack", true);
    }

    /// <summary>
    /// ���������� �����
    /// </summary>
    private void StopAttack()
    {
        character.anim.SetBool("isAttack", false);
    }

    /// <summary>
    /// ������� � �������
    /// </summary>
    /// <param name="objectPosition">������� �������</param>
    private void RotationToObject(Vector2 objectPosition)
    {
        if (objectPosition.x < character.transform.position.x)
            Rotation(180);
        else
            Rotation(0);
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="angle">���� ��������</param>
    private void Rotation(int angle)
    {
        Transform name = character.transform.Find("UI/Name");
        Transform health = character.transform.Find("UI/Health");
        Transform button = character.transform.Find("UI/Button");

        character.transform.rotation = Quaternion.Euler(0, angle, 0);

        name.rotation = Quaternion.Euler(0, 0, 0);
        health.rotation = Quaternion.Euler(0, 0, 0);
        button.rotation = Quaternion.Euler(0, 0, 0);
    }
}