using UnityEngine;

/// <summary>
/// ��������� �����������
/// </summary>
public interface IMoving
{
    /// <summary>
    /// �������������
    /// </summary>
    public void Move();
}


/// <summary>
/// ��������������
/// </summary>
public class Patrolling : Moving, IMoving
{
    private float movingPoint; // ����� �����������
    private float waitingTime; // ����� ��������


    /// <summary>
    /// ����������� ��������������
    /// </summary>
    /// <param name="character">��������</param>
    public Patrolling(Character character)
    {
        this.character = character;
    }


    private void Start()
    {
        UpdateMoveParams();
    }


    /// <summary>
    /// �������������
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
    /// �������� ��������� �����������
    /// </summary>
    private void UpdateMoveParams()
    {
        waitingTime = Random.Range(1, character.characteristics.waitingTime);
        movingPoint = UpdateMovingPoint();
    }

    /// <summary>
    /// ��������
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
    /// �������� ����� �����������
    /// </summary>
    /// <returns>����� �����������</returns>
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
    /// �������� ����������� �����������
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
/// �������������
/// </summary>
public class Following : Moving, IMoving
{
    /// <summary>
    /// ����������� �������������
    /// </summary>
    /// <param name="character">��������</param>
    public Following(Character character)
    {
        this.character = character;
    }


    /// <summary>
    /// �������������
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
/// �����
/// </summary>
public class Escape : Moving, IMoving
{
    /// <summary>
    /// ����������� ������
    /// </summary>
    /// <param name="character">��������</param>
    public Escape(Character character)
    {
        this.character = character;
    }


    /// <summary>
    /// �������������
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
/// ������ �� ������
/// </summary>
public class GoToWork : Moving, IMoving
{
    /// <summary>
    /// ����������� �������� �� ������
    /// </summary>
    /// <param name="character">��������</param>
    public GoToWork(Character character)
    {
        this.character = character;
    }


    /// <summary>
    /// �������������
    /// </summary>
    public void Move()
    {
        if (character.workObject != null)
            if (character.characteristics.type == "Woodman")
                GoToWood();
    }

    /// <summary>
    /// ���� � ������
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
/// ����������� ��������
/// </summary>
public class MovingItem : IMoving
{
    public Item item; // �������


    /// <summary>
    /// ����������� ����������� ��������
    /// </summary>
    /// <param name="item">�������</param>
    public MovingItem(Item item)
    {
        this.item = item;
    }


    /// <summary>
    /// �������������
    /// </summary>
    public void Move()
    {
        item.transform.position = Input.mousePosition;
    }
}

/// <summary>
/// ����������� ������
/// </summary>
public class MovingCamera : IMoving
{
    public Camera camera; // ������

    public float[] border; // ������� �����������
    public int direction; // ����������� �����������
    public float speed; // �������� �����������


    /// <summary>
    /// ����������� ����������� ������
    /// </summary>
    /// <param name="camera">������</param>
    /// <param name="direction">����������� �����������</param>
    /// <param name="speed">�������� �����������</param>
    public MovingCamera(Camera camera, float[] border, int direction, float speed)
    {
        this.camera = camera;
        this.border = border;
        this.direction = direction;
        this.speed = speed;
    }


    /// <summary>
    /// �������������
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
/// �����������
/// </summary>
public abstract class Moving : MonoBehaviour
{
    public Character character; // ��������


    /// <summary>
    /// ��������� ������
    /// </summary>
    public void MoveRight()
    {
        Rotation(0);
        MovementAnim(true);
        Movement(1);
    }

    /// <summary>
    /// ��������� �����
    /// </summary>
    public void MoveLeft()
    {
        Rotation(180);
        MovementAnim(true);
        Movement(-1);
    }

    /// <summary>
    /// ���������� ��������
    /// </summary>
    public void StopMove()
    {
        MovementAnim(false);
        Movement(0);
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="angle">���� ��������</param>
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
    /// ������������
    /// </summary>
    /// <param name="direction">����������� ��������</param>
    private void Movement(int direction)
    {
        character.rb.velocity = new Vector2(direction * character.characteristics.moveSpeed, character.rb.velocity.y);
    }

    /// <summary>
    /// �������� ������������
    /// </summary>
    /// <param name="isMove">����������� �� ������������?</param>
    private void MovementAnim(bool isMove)
    {
        character.anim.speed = 1f;
        character.anim.SetBool("isRun", isMove);
    }
}