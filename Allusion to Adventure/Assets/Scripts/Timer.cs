using UnityEngine;

public class Timer : MonoBehaviour
{
    public static float minute; // ������
    public static float hour = 9; // ���

    private static Timer instance;

    public static Timer getInstance()
    {
        if (instance == null)
            instance = new Timer();
        return instance;
    }
}
