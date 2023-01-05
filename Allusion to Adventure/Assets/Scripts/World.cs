using UnityEngine;

/// <summary>
/// мир
/// </summary>
public class World : MonoBehaviour
{
    public static bool isNewGame = true; // является ли игра новой?

    public float[] worldBorder; // границы мира 

    public Wood wood; // дерево
    public float[] forestBorder; // границы леса 

    public Character character; // персонаж
    public float[] cityBorder; // границы города

    public float cameraMoveSpeed; // скорость перемещения камеры

    private int direction = 0; // направление перемещения


    private void Start()
    {
        if (isNewGame)
        {
            SpawnWoods();
            SpawnCharacters();
        }
    }

    private void Update()
    {
        Camera camera = Camera.main;

        IMoving typeOfMoving = new MovingCamera(camera, worldBorder, direction, cameraMoveSpeed / 100);
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

    /// <summary>
    /// появление деревьев
    /// </summary>
    private void SpawnWoods()
    {
        float spawnPoint = forestBorder[0];

        while (spawnPoint < forestBorder[1])
        {
            spawnPoint += Random.Range(2, 5);
            Wood wood = Instantiate(this.wood);
            wood.transform.position = new Vector3(spawnPoint, 2.1f, 4);
        }
    }

    /// <summary>
    /// появление персонажей
    /// </summary>
    private void SpawnCharacters()
    {
        for (int i = 0; i < 6; i++)
        {
            Character character = Instantiate(this.character);
            character.transform.position = new Vector3(Random.Range(cityBorder[0], cityBorder[1]), 0, 1 + (i * 0.1f));
            character.border = cityBorder;

            character.name = NameGeneration();
            character.characteristics.healthPoints = Random.Range(80, 120);
            character.characteristics.maxHealthPoints = character.characteristics.healthPoints;
            character.characteristics.detectionRange = Random.Range(8, 12);
            character.characteristics.attackSpeed = Random.Range(8, 12);
        }
    }

    /// <summary>
    /// генерация имени
    /// </summary>
    /// <returns>имя</returns>
    private string NameGeneration()
    {
        string name = "";
        int nameLength = Random.Range(4, 8);

        for (int i = 0; i < nameLength; i++)
        {
            char letter = System.Convert.ToChar(Random.Range(0, 26) + 65);
            name += letter;
        }

        return name;
    }
}
