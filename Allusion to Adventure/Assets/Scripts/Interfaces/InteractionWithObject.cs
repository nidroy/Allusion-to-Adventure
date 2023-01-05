using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// интерфейс взаимодействия с объектом
/// </summary>
public interface IInteractionWithObject
{
    /// <summary>
    /// взаимодействовать с объектом
    /// </summary>
    public void InteractWithObject();
}


/// <summary>
/// обнаружение объекта
/// </summary>
public class DetectionObject : IInteractionWithObject
{
    public Character character; // персонаж


    /// <summary>
    /// конструктор обнаружения объекта
    /// </summary>
    /// <param name="character">персонаж</param>
    public DetectionObject(Character character)
    {
        this.character = character;
    }


    /// <summary>
    /// взаимодействовать с объектом
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
    /// обнаружить противника
    /// </summary>
    /// <returns>ближайший противник</returns>
    private Character DetectEnemy()
    {
        List<Character> detectedEnemies = DetectEnemies();

        if (detectedEnemies.Count > 0)
        {
            Character detectedEnemy = detectedEnemies[0];

            foreach (Character enemy in detectedEnemies)
            {
                float detectedEnemyDistance = Vector2.Distance(character.transform.position, new Vector2(detectedEnemy.transform.position.x, character.transform.position.y));
                float enemyDistance = Vector2.Distance(character.transform.position, new Vector2(enemy.transform.position.x, character.transform.position.y));

                if (enemyDistance < detectedEnemyDistance)
                    detectedEnemy = enemy;
            }

            return detectedEnemy;
        }
        else
            return null;
    }

    /// <summary>
    /// обнаружить дерево
    /// </summary>
    /// <returns>ближайшее дерево</returns>
    private Wood DetectWood()
    {
        Wood[] woods = Object.FindObjectsOfType<Wood>();
        woods = System.Array.FindAll(woods, wood => !wood.isStump);

        if (woods.Length > 0)
        {
            Wood detectedWood = woods[0];

            foreach (Wood wood in woods)
            {
                float detectedWoodDistance = Vector2.Distance(character.transform.position, new Vector2(detectedWood.transform.position.x, character.transform.position.y));
                float woodDistance = Vector2.Distance(character.transform.position, new Vector2(wood.transform.position.x, character.transform.position.y));

                if (woodDistance < detectedWoodDistance)
                    detectedWood = wood;
            }

            return detectedWood;
        }
        else
            return null;
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
            float characterDistance = Vector2.Distance(this.character.transform.position, new Vector2(character.transform.position.x, this.character.transform.position.y));
            if (character.gameObject.tag != this.character.gameObject.tag && characterDistance <= this.character.characteristics.detectionRange && !character.anim.GetBool("isDie"))
                detectedEnemies.Add(character);
        }

        return detectedEnemies;
    }
}

/// <summary>
/// атака объекта
/// </summary>
public class AttackingObject : IInteractionWithObject
{
    public Character character; // персонаж


    /// <summary>
    /// конструктор атаки объекта
    /// </summary>
    /// <param name="character">персонаж</param>
    public AttackingObject(Character character)
    {
        this.character = character;
    }


    /// <summary>
    /// взаимодействовать с объектом
    /// </summary>
    public void InteractWithObject()
    {
        if (character.enemy == null)
        {
            if (character.workObject != null && character.characteristics.typeOfMoving == "GoToWork")
            {
                float objectDistance = Vector2.Distance(character.transform.position, new Vector2(character.workObject.transform.position.x, character.transform.position.y));

                if (objectDistance <= character.characteristics.attackRange)
                {
                    RotationToObject(character.workObject.transform.position);
                    StartAttack();
                }
                else
                    StopAttack();
            }
            else
                StopAttack();
        }
        else
        {
            float enemyDistance = Vector2.Distance(character.transform.position, new Vector2(character.enemy.transform.position.x, character.transform.position.y));

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
    /// поворот к объекту
    /// </summary>
    /// <param name="objectPosition">позиция объекта</param>
    private void RotationToObject(Vector2 objectPosition)
    {
        if (objectPosition.x < character.transform.position.x)
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
        Transform name = character.transform.Find("UI/Name");
        Transform health = character.transform.Find("UI/Health");
        Transform button = character.transform.Find("UI/Button");

        character.transform.rotation = Quaternion.Euler(0, angle, 0);

        name.rotation = Quaternion.Euler(0, 0, 0);
        health.rotation = Quaternion.Euler(0, 0, 0);
        button.rotation = Quaternion.Euler(0, 0, 0);
    }
}