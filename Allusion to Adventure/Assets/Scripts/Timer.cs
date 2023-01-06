using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static float minute; // ������
    public static int hour; // ���
    public static int day; // ����
    public static int month; // �����
    public static int year; // ���

    public TMP_Text time; // �����


    private static Timer instance;

    public static Timer getInstance()
    {
        if (instance == null)
            instance = new Timer();
        return instance;
    }


    private void Update()
    {
        TimeCounting();
    }


    /// <summary>
    /// ������ �������
    /// </summary>
    private void TimeCounting()
    {
        minute += Time.deltaTime * 2;
        time.text = hour.ToString() + " : " + Convert.ToInt32(minute).ToString();

        if (minute >= 60)
        {
            minute = 0;
            hour++;
        }
        if (hour == 24)
        {
            hour = 0;
            day++;

            if (day % 10 == 0)
                World.isSpawnEnemies = true;

            SendTime();
            WorldStocks.SendResources();
            World.SendData();
        }
        if (day == 31)
        {
            day = 1;
            month++;
        }
        if (month == 13)
        {
            month = 0;
            year++;
        }
    }

    /// <summary>
    /// ��������� �����
    /// </summary>
    public static void SendTime()
    {
        Proxy.SendMessage(string.Format("UpdateTime\t{0}\t{1}\t{2}\t{3}\t{4}", minute, hour, day, month, year));
    }
}
