using System.Net.Sockets;
using System.Text;

namespace Server
{
    /// <summary>
    /// клиент
    /// </summary>
    public class Client
    {
        private string name = "";
        private Dictionary<string, ICommand> commands;

        private TcpClient tcpClient;
        private NetworkStream stream;


        /// <summary>
        /// конструктор клиента
        /// </summary>
        /// <param name="tcpClient">тсп клиента</param>
        public Client(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            stream = tcpClient.GetStream();
        }


        /// <summary>
        /// работа клиента
        /// </summary>
        public void Process()
        {
            try
            {
                string request = ReceiveMessage();
                string answer = ExecuteCommand(request);
                SendMessage(answer);

                if (answer != "Successful")
                    return;

                string message = name + " connected";
                Console.WriteLine(message);

                while (true)
                {
                    try
                    {
                        request = ReceiveMessage();
                        if (string.IsNullOrEmpty(request))
                            continue;

                        message = name + " : " + request;
                        Console.WriteLine(message);

                        answer = ExecuteCommand(request);

                        if (answer == "LogOut")
                            throw new Exception();

                        message = "Server : " + answer;
                        Console.WriteLine(message);

                        SendMessage(answer);
                    }
                    catch
                    {
                        message = name + " disconnected";
                        Console.WriteLine(message);

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// закрыть соединение клиента
        /// </summary>
        public void Disconnect()
        {
            if (stream != null)
                stream.Close();
            if (tcpClient != null)
                tcpClient.Close();

            Stub.clients.Remove(this);
        }

        /// <summary>
        /// отправить сообщение
        /// </summary>
        /// <param name="message">сообщение</param>
        private void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// получить сообщение
        /// </summary>
        /// <returns>сообщение</returns>
        private string ReceiveMessage()
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

        /// <summary>
        /// выполнить команду
        /// </summary>
        /// <param name="request">запрос</param>
        /// <returns>ответ</returns>
        private string ExecuteCommand(string request)
        {
            FillCommands();
            (string command, string[] parameters) = SplitRequest(request);

            if (name == "")
                name = parameters[0];

            Invoker invoker = new Invoker();
            invoker.SetCommand(commands[command]);
            invoker.SetParameters(parameters);
            return invoker.ExecuteCommand();
        }

        /// <summary>
        /// заполнить словарь команд
        /// </summary>
        private void FillCommands()
        {
            commands = new Dictionary<string, ICommand>();

            commands.Add("Authorization", new Authorization(new AuthorizationReceiver()));
            commands.Add("Registration", new Registration(new RegistrationReceiver()));
            commands.Add("LogOut", new LogOut(new LogOutReceiver()));
        }

        /// <summary>
        /// разделить запрос
        /// </summary>
        /// <param name="request">запрос</param>
        /// <returns>команда и параметры команды</returns>
        private (string, string[]) SplitRequest(string request)
        {
            string[] parameters = request.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

            string command = parameters[0];
            parameters = parameters.Skip(1).ToArray();

            return (command, parameters);
        }
    }
}
