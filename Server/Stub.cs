using System.Net.Sockets;
using System.Net;

namespace Server
{
    /// <summary>
    /// заглушка
    /// </summary>
    public class Stub
    {
        public static List<Client> clients = new List<Client>(); // все подключения

        private TcpListener listener = new TcpListener(IPAddress.Any, 8888); // сервер для прослушивания


        /// <summary>
        /// работа сервера
        /// </summary>
        public void Process()
        {
            try
            {
                listener.Start();

                Console.WriteLine("The server is running. Waiting for connections...");

                while (true)
                {
                    TcpClient tcpClient = listener.AcceptTcpClient();

                    Client client = new Client(tcpClient);
                    clients.Add(client);

                    Thread thread = new Thread(new ThreadStart(client.Process));
                    thread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// закрыть соединение сервера
        /// </summary>
        private void Disconnect()
        {
            foreach (Client client in clients)
                client.Disconnect();

            listener.Stop();
            Environment.Exit(0);
        }
    }
}
