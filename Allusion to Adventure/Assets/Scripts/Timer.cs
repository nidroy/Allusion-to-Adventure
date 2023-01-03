using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float minute; // минута
    public int hour; // час
    public int day; // день
    public int month; // месяц
    public int year; // год

    public TMP_Text time; // время

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
    /// отсчет времени
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
}
