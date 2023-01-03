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

    public bool isStump; // является ли дерево пнем?


    private void Update()
    {
        BecomeStump();
        GrowthTimer();
    }


    /// <summary>
    /// стать пнем
    /// </summary>
    private void BecomeStump()
    {
        if (healthPoints <= 0)
            isStump = true;
        else
            isStump = false;
    }

    /// <summary>
    /// таймер роста
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
