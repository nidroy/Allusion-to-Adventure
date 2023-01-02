using UnityEngine;

public class Timer : MonoBehaviour
{
    public static float minute; // минута
    public static float hour = 9; // час

    private static Timer instance;

    public static Timer getInstance()
    {
        if (instance == null)
            instance = new Timer();
        return instance;
    }
}
