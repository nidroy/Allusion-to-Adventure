using UnityEngine;

/// <summary>
/// дерево
/// </summary>
public class Wood : MonoBehaviour
{
    public float healthPoints; // очки здоровья
    public float maxHealthPoints; // максимальное количество очков здоровья

    public float growthTime; // время роста
    public float maxGrowthTime; // максимальное время роста

    public Logs logs; // бревна

    public bool isStump; // является ли дерево пнем?

    private bool isLogs; // выпали ли бревна?


    private void Update()
    {
        BecomeStump();
        SpawnLogs();
        GrowthTimer();
    }


    /// <summary>
    /// стать пнем
    /// </summary>
    private void BecomeStump()
    {
        if (healthPoints <= 0)
        {
            transform.Find("Tree").gameObject.SetActive(false);
            transform.Find("Stump").gameObject.SetActive(true);

            isStump = true;
            isLogs = true;
        }
        else if (healthPoints == maxHealthPoints)
        {
            transform.Find("Tree").gameObject.SetActive(true);
            transform.Find("Stump").gameObject.SetActive(false);

            isStump = false;
        }
    }

    /// <summary>
    /// таймер роста
    /// </summary>
    private void GrowthTimer()
    {
        if (isStump)
        {
            growthTime += Time.deltaTime;
            healthPoints = 1;

            if (growthTime >= maxGrowthTime)
            {
                growthTime = 0;
                healthPoints = maxHealthPoints;
                WorldStocks.trees += 1;
                isStump = false;
            }
        }
    }

    /// <summary>
    /// появление бревен
    /// </summary>
    private void SpawnLogs()
    {
        if (isLogs)
        {
            Logs logs = Instantiate(this.logs, transform);
            logs.transform.position = new Vector3(logs.transform.position.x, logs.transform.position.y, logs.transform.position.z - 2);
            WorldStocks.trees -= 1;
            isLogs = false;
        }
    }
}
