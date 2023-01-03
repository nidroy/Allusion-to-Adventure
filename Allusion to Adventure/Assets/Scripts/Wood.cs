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

    public bool isStump; // �������� �� ������ ����?


    private void Update()
    {
        BecomeStump();
        GrowthTimer();
    }


    /// <summary>
    /// ����� ����
    /// </summary>
    private void BecomeStump()
    {
        if (healthPoints <= 0)
            isStump = true;
        else
            isStump = false;
    }

    /// <summary>
    /// ������ �����
    /// </summary>
    private void GrowthTimer()
    {
        if (isStump)
        {
            growthTime -= Time.deltaTime;

            if (growthTime <= 0)
                healthPoints = maxHealthPoints;
        }
        else
            growthTime = maxGrowthTime;
    }
}
