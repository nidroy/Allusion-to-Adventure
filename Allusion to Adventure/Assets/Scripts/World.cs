using UnityEngine;

/// <summary>
/// ���
/// </summary>
public class World : MonoBehaviour
{
    public static bool isNewGame; // �������� �� ���� �����?
    public static bool isSpawnEnemies; // �������� �� ����������?
    public static bool isSendData; // ��������� �� ������ � ���� �� ������?

    public float[] worldBorder; // ������� ���� 

    public Wood wood; // ������
    public float[] forestBorder; // ������� ���� 

    public Character character; // ��������
    public float[] cityBorder; // ������� ������

    public float cameraMoveSpeed; // �������� ����������� ������

    private int direction = 0; // ����������� �����������


    private void Start()
    {
        if (isNewGame)
        {
            SpawnWoods();
            SpawnCharacters();

            isSendData = true;
        }
    }

    private void Update()
    {
        Camera camera = Camera.main;

        IMoving typeOfMoving = new MovingCamera(camera, worldBorder, direction, cameraMoveSpeed / 100);
        typeOfMoving.Move();

        if (isSpawnEnemies)
            SpawnEnemies();

        SendData();
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

    /// <summary>
    /// ����� �� ����
    /// </summary>
    public void ExitGame()
    {
        Proxy.SendMessage("LogOut");

        Application.Quit();
    }

    /// <summary>
    /// ��������� ��������
    /// </summary>
    private void SpawnWoods()
    {
        float spawnPoint = forestBorder[0];

        while (spawnPoint < forestBorder[1])
        {
            spawnPoint += Random.Range(2, 5);
            Wood wood = Instantiate(this.wood);
            wood.transform.position = new Vector3(spawnPoint, 2.1f, 4);
            WorldStocks.trees += 1;
        }
    }

    /// <summary>
    /// ��������� ����������
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
    /// ��������� �����������
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
    /// ��������� �����
    /// </summary>
    /// <returns>���</returns>
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
    /// ��������� ����
    /// </summary>
    /// <param name="equipment">����������</param>
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

    /// <summary>
    /// ��������� ������ � ����
    /// </summary>
    private void SendData()
    {
        if (isSendData)
        {
            Timer.SendTime();
            if (Proxy.ReceiveMessage() == "Time updated")
                WorldStocks.SendResources();
            if (Proxy.ReceiveMessage() == "Resources updated")
                SendCharacters();

            isSendData = false;
        }
    }

    /// <summary>
    /// ��������� ����������
    /// </summary>
    private void SendCharacters()
    {
        int peaceful = 0;
        int swordsman = 0;
        int woodman = 0;
        int enemy = 0;

        Character[] characters = FindObjectsOfType<Character>();
        foreach (Character character in characters)
        {
            if (character.tag == "Enemy")
                enemy++;
            else
            {
                if (character.characteristics.type == "Peaceful")
                    peaceful++;
                else if (character.characteristics.type == "Swordsman")
                    swordsman++;
                else if (character.characteristics.type == "Woodman")
                    woodman++;
            }
        }

        Proxy.SendMessage(string.Format("UpdateWorld\t{0}\t{1}\t{2}\t{3}", peaceful, swordsman, woodman, enemy));
    }
}
