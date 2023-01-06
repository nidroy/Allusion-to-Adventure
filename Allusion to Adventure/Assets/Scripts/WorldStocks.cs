using UnityEngine;

/// <summary>
/// ������� ������
/// </summary>
public class WorldStocks : MonoBehaviour
{
    public static int sword; // ���
    public static int armor; // �����
    public static int healingPotion; // ����� �������
    public static int axe; // �����
    public static int logs; // ������
    public static int coins; // ������
    public static int trees; // �������


    private static WorldStocks instance;

    public static WorldStocks getInstance()
    {
        if (instance == null)
            instance = new WorldStocks();
        return instance;
    }


    /// <summary>
    /// ��������� �������
    /// </summary>
    public static void SendResources()
    {
        Proxy.SendMessage(string.Format("UpdateResources\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", sword, armor, healingPotion, axe, logs, coins, trees));
    }
}
