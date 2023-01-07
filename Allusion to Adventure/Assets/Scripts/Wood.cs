using UnityEngine;

/// <summary>
/// ������
/// </summary>
public class Wood : MonoBehaviour
{
    public float healthPoints; // ���� ��������
    public float maxHealthPoints; // ������������ ���������� ����� ��������

    public float growthTime; // ����� �����
    public float maxGrowthTime; // ������������ ����� �����

    public Logs logs; // ������

    public bool isStump; // �������� �� ������ ����?

    private bool isLogs; // ������ �� ������?


    private void Update()
    {
        BecomeStump();
        SpawnLogs();
        GrowthTimer();
    }


    /// <summary>
    /// ����� ����
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
    /// ������ �����
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
    /// ��������� ������
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
