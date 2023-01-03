using UnityEngine;

public class World : MonoBehaviour
{
    public float cameraMoveSpeed; // �������� ����������� ������

    private int direction = 0; // ����������� �����������


    private void Update()
    {
        Camera camera = Camera.main;

        IMoving typeOfMoving = new MovingCamera(camera, direction, cameraMoveSpeed / 100);
        typeOfMoving.Move();
    }


    /// <summary>
    /// �������� ������ ������
    /// </summary>
    public void CameraMoveRight()
    {
        direction = 1;
    }

    /// <summary>
    /// �������� ������ �����
    /// </summary>
    public void CameraMoveLeft()
    {
        direction = -1;
    }

    /// <summary>
    /// ���������� �������� ������
    /// </summary>
    public void StopCameraMove()
    {
        direction = 0;
    }
}
