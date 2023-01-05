using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Proxy : MonoBehaviour
{
    private const int port = 8888; // ����
    private const string address = "127.0.0.1"; // IP �����

    private static TcpClient client;
    private static NetworkStream stream;


    private static Proxy instance;

    public static Proxy getInstance()
    {
        if (instance == null)
            instance = new Proxy();
        return instance;
    }


    /// <summary>
    /// ������� �����������
    /// </summary>
    public static void CreateConnection()
    {
        client = new TcpClient(address, port);
        stream = client.GetStream();
    }

    /// <summary>
    /// ��������� ���������
    /// </summary>
    /// <param name="message">���������</param>
    public static new void SendMessage(string message)
    {
        byte[] data = Encoding.Unicode.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }

    /// <summary>
    /// �������� ���������
    /// </summary>
    /// <returns>���������</returns>
    public static string ReceiveMessage()
    {
        byte[] data = new byte[64];
        StringBuilder builder = new StringBuilder();
        int bytes;

        do
        {
            bytes = stream.Read(data, 0, data.Length);
            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
        }
        while (stream.DataAvailable);

        return builder.ToString();
    }
}
