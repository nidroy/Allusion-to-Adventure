using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// интерфейс взаимодействия с противником
/// </summary>
public interface IInteractionWithEnemy
{
    /// <summary>
    /// взаимодействовать с противником
    /// </summary>
    public void InteractWithEnemy();
}


/// <summary>
/// обнаружение противника
/// </summary>
public class DetectionEnemy : IInteractionWithEnemy
{
    public Character character; // персонаж


    /// <summary>
    /// конструктор обнаружения противника
    /// </summary>
    /// <param name="character">персонаж</param>
    public DetectionEnemy(Character character)
    {
        this.character = character;
    }


    /// <summary>
    /// взаимодействовать с противником
    /// </summary>
    public void InteractWithEnemy()
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

            character.enemy = detectedEnemy;
        }
        else
            character.enemy = null;
    }

    /// <summary>
    /// обнаружить противников
    /// </summary>
    /// <returns>лист противников</returns>
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
/// атака противника
/// </summary>
public class AttackingEnemy : IInteractionWithEnemy
{
    public Character character; // персонаж


    /// <summary>
    /// конструктор атаки противника
    /// </summary>
    /// <param name="character">персонаж</param>
    public AttackingEnemy(Character character)
    {
        this.character = character;
    }


    /// <summary>
    /// взаимодействовать с противником
    /// </summary>
    public void InteractWithEnemy()
    {
        if (character.enemy == null)
            StopAttack();
        else
        {
            float enemyDistance = Vector2.Distance(character.transform.position, character.enemy.transform.position);

            if (enemyDistance <= character.characteristics.attackRange)
            {
                RotationToEnemy();
                StartAttack();
            }
            else
                StopAttack();
        }
    }

    /// <summary>
    /// начать атаку
    /// </summary>
    private void StartAttack()
    {
        character.anim.speed = character.characteristics.attackSpeed / 10;
        character.anim.SetBool("isAttack", true);
    }

    /// <summary>
    /// остановить атаку
    /// </summary>
    private void StopAttack()
    {
        character.anim.SetBool("isAttack", false);
    }

    /// <summary>
    /// поворот к противнику
    /// </summary>
    private void RotationToEnemy()
    {
        if (character.enemy.transform.position.x < character.transform.position.x)
            Rotation(180);
        else
            Rotation(0);
    }

    /// <summary>
    /// поворот
    /// </summary>
    /// <param name="angle">угол поворота</param>
    private void Rotation(int angle)
    {
        Transform health = character.transform.Find("UI/Health");
        Transform button = character.transform.Find("UI/Button");

        character.transform.rotation = Quaternion.Euler(0, angle, 0);

        health.rotation = Quaternion.Euler(0, 0, 0);
        button.rotation = Quaternion.Euler(0, 0, 0);
    }
}