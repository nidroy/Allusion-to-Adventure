using UnityEngine;

/// <summary>
/// интерфейс перемещения
/// </summary>
public interface IMoving
{
    /// <summary>
    /// передвигаться
    /// </summary>
    public void Move();
}


/// <summary>
/// патрулирование
/// </summary>
public class Patrolling : Moving, IMoving
{
    private float movingPoint; // точка перемещения
    private float waitingTime; // время ожидания


    /// <summary>
    /// конструктор патрулирования
    /// </summary>
    /// <param name="character">персонаж</param>
    public Patrolling(Character character)
    {
        this.character = character;
    }


    private void Start()
    {
        UpdateMoveParams();
    }


    /// <summary>
    /// передвигаться
    /// </summary>
    public void Move()
    {
        float position = character.transform.position.x;
        float direction = character.transform.rotation.y;

        if (position < movingPoint && direction == 0)
            MoveRight();
        else if (position > movingPoint && direction != 0)
            MoveLeft();
        else
            Waiting();
    }

    /// <summary>
    /// обновить параметры перемещения
    /// </summary>
    private void UpdateMoveParams()
    {
        waitingTime = Random.Range(1, character.characteristics.waitingTime);
        movingPoint = UpdateMovingPoint();
    }

    /// <summary>
    /// ожидание
    /// </summary>
    private void Waiting()
    {
        StopMove();
        waitingTime -= Time.fixedDeltaTime;

        if (waitingTime <= 0)
        {
            UpdateMoveDirection();
            UpdateMoveParams();
        }
    }

    /// <summary>
    /// обновить точку перемещения
    /// </summary>
    /// <returns>точка перемещения</returns>
    private float UpdateMovingPoint()
    {
        float borderDistance0 = Vector2.Distance(character.transform.position, new Vector2(character.border[0], character.transform.position.y));
        float borderDistance1 = Vector2.Distance(character.transform.position, new Vector2(character.border[1], character.transform.position.y));

        float position = character.transform.position.x;

        if (borderDistance0 > borderDistance1)
            return Random.Range(character.border[0], position);
        else
            return Random.Range(position, character.border[1]);
    }

    /// <summary>
    /// обновить направление перемещения
    /// </summary>
    private void UpdateMoveDirection()
    {
        float borderDistance0 = Vector2.Distance(character.transform.position, new Vector2(character.border[0], character.transform.position.y));
        float borderDistance1 = Vector2.Distance(character.transform.position, new Vector2(character.border[1], character.transform.position.y));

        float direction = character.transform.rotation.y;

        if (borderDistance0 > borderDistance1 && direction == 0)
            Rotation(180);
        else if (borderDistance0 <= borderDistance1 && direction != 0)
            Rotation(0);
    }
}

/// <summary>
/// преследование
/// </summary>
public class Following : Moving, IMoving
{
    /// <summary>
    /// конструктор преследования
    /// </summary>
    /// <param name="character">персонаж</param>
    public Following(Character character)
    {
        this.character = character;
    }


    /// <summary>
    /// передвигаться
    /// </summary>
    public void Move()
    {
        if (character.enemy != null)
        {
            float enemyDistance = Vector2.Distance(character.transform.position, new Vector2(character.enemy.transform.position.x, character.transform.position.y));

            if (enemyDistance <= character.characteristics.attackRange)
                StopMove();
            else
            {
                if (character.enemy.transform.position.x < character.transform.position.x)
                    MoveLeft();
                else
                    MoveRight();
            }
        }
    }
}

/// <summary>
/// побег
/// </summary>
public class Escape : Moving, IMoving
{
    /// <summary>
    /// конструктор побега
    /// </summary>
    /// <param name="character">персонаж</param>
    public Escape(Character character)
    {
        this.character = character;
    }


    /// <summary>
    /// передвигаться
    /// </summary>
    public void Move()
    {
        if (character.enemy.transform.position.x < character.transform.position.x)
            MoveRight();
        else
            MoveLeft();
    }
}

/// <summary>
/// ходить на работу
/// </summary>
public class GoToWork : Moving, IMoving
{
    /// <summary>
    /// конструктор хождения на работу
    /// </summary>
    /// <param name="character">персонаж</param>
    public GoToWork(Character character)
    {
        this.character = character;
    }


    /// <summary>
    /// передвигаться
    /// </summary>
    public void Move()
    {
        if (character.workObject != null)
            if (character.characteristics.type == "Woodman")
                GoToWood();
    }

    /// <summary>
    /// идти к дереву
    /// </summary>
    private void GoToWood()
    {
        float woodDistance = Vector2.Distance(character.transform.position, new Vector2(character.workObject.transform.position.x, character.transform.position.y));

        if (woodDistance <= character.characteristics.attackRange)
            StopMove();
        else
        {
            if (character.workObject.transform.position.x < character.transform.position.x)
                MoveLeft();
            else
                MoveRight();
        }
    }
}

/// <summary>
/// перемещение предмета
/// </summary>
public class MovingItem : IMoving
{
    public Item item; // предмет


    /// <summary>
    /// конструктор перемещения предмета
    /// </summary>
    /// <param name="item">предмет</param>
    public MovingItem(Item item)
    {
        this.item = item;
    }


    /// <summary>
    /// передвигаться
    /// </summary>
    public void Move()
    {
        item.transform.position = Input.mousePosition;
    }
}

/// <summary>
/// перемещение камеры
/// </summary>
public class MovingCamera : IMoving
{
    public Camera camera; // камера

    public float[] border; // границы перемещения
    public int direction; // направление перемещения
    public float speed; // скорость перемещения


    /// <summary>
    /// конструктор перемещения камеры
    /// </summary>
    /// <param name="camera">камера</param>
    /// <param name="direction">направление перемещения</param>
    /// <param name="speed">скорость перемещения</param>
    public MovingCamera(Camera camera, float[] border, int direction, float speed)
    {
        this.camera = camera;
        this.border = border;
        this.direction = direction;
        this.speed = speed;
    }


    /// <summary>
    /// передвигаться
    /// </summary>
    public void Move()
    {
        if (camera.transform.position.x < border[0])
            camera.transform.position = new Vector3(border[0], camera.transform.position.y, camera.transform.position.z);
        else if (camera.transform.position.x > border[1])
            camera.transform.position = new Vector3(border[1], camera.transform.position.y, camera.transform.position.z);
        else
        {
            Vector2 direction = new Vector2(this.direction, 0);
            camera.transform.Translate(direction.normalized * speed);
        }
    }
}


/// <summary>
/// перемещение
/// </summary>
public abstract class Moving : MonoBehaviour
{
    public Character character; // персонаж


    /// <summary>
    /// двигаться вправо
    /// </summary>
    public void MoveRight()
    {
        Rotation(0);
        MovementAnim(true);
        Movement(1);
    }

    /// <summary>
    /// двигаться влево
    /// </summary>
    public void MoveLeft()
    {
        Rotation(180);
        MovementAnim(true);
        Movement(-1);
    }

    /// <summary>
    /// остановить движение
    /// </summary>
    public void StopMove()
    {
        MovementAnim(false);
        Movement(0);
    }

    /// <summary>
    /// поворот
    /// </summary>
    /// <param name="angle">угол поворота</param>
    public void Rotation(int angle)
    {
        Transform name = character.transform.Find("UI/Name");
        Transform health = character.transform.Find("UI/Health");
        Transform button = character.transform.Find("UI/Button");

        character.transform.rotation = Quaternion.Euler(0, angle, 0);

        name.rotation = Quaternion.Euler(0, 0, 0);
        health.rotation = Quaternion.Euler(0, 0, 0);
        button.rotation = Quaternion.Euler(0, 0, 0);
    }

    /// <summary>
    /// передвижение
    /// </summary>
    /// <param name="direction">направление движения</param>
    private void Movement(int direction)
    {
        character.rb.velocity = new Vector2(direction * character.characteristics.moveSpeed, character.rb.velocity.y);
    }

    /// <summary>
    /// анимации передвижения
    /// </summary>
    /// <param name="isMove">совершается ли передвижение?</param>
    private void MovementAnim(bool isMove)
    {
        character.anim.speed = 1f;
        character.anim.SetBool("isRun", isMove);
    }
}