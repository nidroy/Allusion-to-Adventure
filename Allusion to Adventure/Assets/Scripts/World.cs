using UnityEngine;

public class World : MonoBehaviour
{
    public float cameraMoveSpeed; // скорость перемещения камеры

    private int direction = 0; // направление перемещения


    private void Update()
    {
        Camera camera = Camera.main;

        IMoving typeOfMoving = new MovingCamera(camera, direction, cameraMoveSpeed / 100);
        typeOfMoving.Move();
    }


    /// <summary>
    /// движение камеры вправо
    /// </summary>
    public void CameraMoveRight()
    {
        direction = 1;
    }

    /// <summary>
    /// движение камеры влево
    /// </summary>
    public void CameraMoveLeft()
    {
        direction = -1;
    }

    /// <summary>
    /// остановить движение камеры
    /// </summary>
    public void StopCameraMove()
    {
        direction = 0;
    }
}
