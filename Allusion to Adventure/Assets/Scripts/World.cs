using UnityEngine;

/// <summary>
/// мир
/// </summary>
public class World : MonoBehaviour
{
    public static bool isNewGame; // является ли игра новой?
    public static bool isSpawnEnemies; // появятся ли противники?

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

        if (isSpawnEnemies)
            SpawnEnemies();
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
            character.tag = "Human";
            character.characteristics.healthPoints = Random.Range(80, 120);
            character.characteristics.maxHealthPoints = character.characteristics.healthPoints;
            character.characteristics.detectionRange = Random.Range(8, 12);
            character.characteristics.attackSpeed = Random.Range(8, 12);
        }
    }

    /// <summary>
    /// появление противников
    /// </summary>
    private void SpawnEnemies()
    {
        int enemiesCount = Random.Range(1, Timer.day / 10);
        if (enemiesCount > 5)
            enemiesCount = 5;

        for (int i = 0; i < enemiesCount; i++)
        {
            Character character = Instantiate(this.character);
            character.transform.position = new Vector3(worldBorder[0], 0, 2 + (i * 0.1f));
            character.border = cityBorder;

            character.name = NameGeneration();
            character.tag = "Enemy";
            character.characteristics.healthPoints = Random.Range(80, 120);
            character.characteristics.maxHealthPoints = character.characteristics.healthPoints;
            character.characteristics.detectionRange = Random.Range(10, 20);
            character.characteristics.attackSpeed = Random.Range(8, 12);

            Transform button = character.transform.Find("UI/Button");
            button.gameObject.SetActive(false);

            character.Card.SetActive(true);
            Inventory equipment = character.Card.transform.Find("Equipment").GetComponent<Inventory>();
            SpawnSword(equipment);
            character.equipment.UpdateEquipment(character);
            character.Card.SetActive(false);
        }

        isSpawnEnemies = false;
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

    /// <summary>
    /// появление меча
    /// </summary>
    /// <param name="equipment">снаряжение</param>
    private void SpawnSword(Inventory equipment)
    {
        equipment.cells[0].item.data.id = equipment.data.itemsData[0].id;
        equipment.cells[0].item.data.name = equipment.data.itemsData[0].name;
        equipment.cells[0].item.data.type = equipment.data.itemsData[0].type;
        equipment.cells[0].item.data.durability = equipment.data.itemsData[0].durability;
        equipment.cells[0].item.data.damage = equipment.data.itemsData[0].damage;
        equipment.cells[0].item.data.sprite = equipment.data.itemsData[0].sprite;

        equipment.cells[0].item.image.sprite = equipment.cells[0].item.data.sprite;
        equipment.cells[0].item.count.text = "1";
        equipment.cells[0].item.maxDurability = equipment.cells[0].item.data.durability;

        equipment.cells[0].item.gameObject.SetActive(true);
    }
}
