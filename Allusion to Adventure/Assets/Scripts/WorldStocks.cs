using UnityEngine;

/// <summary>
/// мировые запасы
/// </summary>
public class WorldStocks : MonoBehaviour
{
    public static int sword; // меч
    public static int armor; // броня
    public static int healingPotion; // зелье лечения
    public static int axe; // топор
    public static int logs; // бревна
    public static int coins; // монеты
    public static int trees; // деревья


    private static WorldStocks instance;

    public static WorldStocks getInstance()
    {
        if (instance == null)
            instance = new WorldStocks();
        return instance;
    }


    /// <summary>
    /// отправить ресурсы
    /// </summary>
    public static void SendResources()
    {
        Proxy.SendMessage(string.Format("UpdateResources\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", sword, armor, healingPotion, axe, logs, coins, trees));
    }
}
