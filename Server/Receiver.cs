using Microsoft.Data.Sqlite;

namespace Server
{
    /// <summary>
    /// получатель авторизации
    /// </summary>
    public class AuthorizationReceiver : Receiver
    {
        public string[] parameters; // параметры получателя


        /// <summary>
        /// авторизация
        /// </summary>
        /// <returns>ответ</returns>
        public string Authorization()
        {
            if (parameters.Length == 2)
                if (parameters[0] != "" || parameters[1] != "")
                {
                    string[] usernames = ExecuteRequest("SELECT Username FROM [User];").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string username in usernames)
                        if (username == parameters[0])
                        {
                            string[] password = ExecuteRequest(string.Format("SELECT Password FROM [User] WHERE Username = '{0}';", username)).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                            if (password[0] == parameters[1])
                                return "Successful";
                            else
                                return "Incorrect password";
                        }

                    return "User not found";
                }

            return "Incorrect username or password";
        }
    }

    /// <summary>
    /// получатель регистрации
    /// </summary>
    public class RegistrationReceiver : Receiver
    {
        public string[] parameters; // параметры получателя


        /// <summary>
        /// регистрация
        /// </summary>
        /// <returns>ответ</returns>
        public string Registration()
        {
            if (parameters.Length == 2)
                if (parameters[0] != "" || parameters[1] != "")
                {
                    string[] usernames = ExecuteRequest("SELECT Username FROM [User];").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string username in usernames)
                        if (username == parameters[0])
                            return "User exists";

                    ExecuteRequest(string.Format("INSERT INTO [User] (Username, Password) VALUES ('{0}', '{1}');", parameters[0], parameters[1]));
                    return "Successful";
                }

            return "Incorrect username or password";
        }
    }

    /// <summary>
    /// получатель выхода из аккаунта
    /// </summary>
    public class LogOutReceiver
    {
        public string[] parameters; // параметры получателя


        /// <summary>
        /// выход из аккаунта
        /// </summary>
        /// <returns>ответ</returns>
        public string LogOut()
        {
            return "LogOut";
        }
    }

    /// <summary>
    /// получатель обновления времени
    /// </summary>
    public class UpdateTimeReceiver : Receiver
    {
        public string[] parameters; // параметры получателя


        /// <summary>
        /// обновить время
        /// </summary>
        /// <returns>ответ</returns>
        public string UpdateTime()
        {
            ExecuteRequest(string.Format("INSERT INTO [Time] (Minute, Hour, Day, Month, Year) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');",
                parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]));
            return "Time updated";
        }
    }

    /// <summary>
    /// получатель обновления ресурсов
    /// </summary>
    public class UpdateResourcesReceiver : Receiver
    {
        public string[] parameters; // параметры получателя


        /// <summary>
        /// обновить ресурсы
        /// </summary>
        /// <returns>ответ</returns>
        public string UpdateResources()
        {
            ExecuteRequest(string.Format("INSERT INTO [WorldStocks] (Sword, Armor, HealingPotion, Axe, Logs, Coins, Trees) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');",
                parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5], parameters[6]));
            return "Resources updated";
        }
    }

    /// <summary>
    /// получатель обновления мира
    /// </summary>
    public class UpdateWorldReceiver : Receiver
    {
        public string username; // имя пользователя
        public string[] parameters; // параметры получателя


        /// <summary>
        /// конструктор получателя обновления мира
        /// </summary>
        /// <param name="username">имя пользователя</param>
        public UpdateWorldReceiver(string username)
        {
            this.username = username;
        }


        /// <summary>
        /// обновить мир
        /// </summary>
        /// <returns>ответ</returns>
        public string UpdateWorld()
        {
            string userID = ExecuteRequest(string.Format("SELECT Id FROM [User] WHERE Username = '{0}';", username));
            string timeID = ExecuteRequest("SELECT Id FROM [Time] ORDER BY Id DESC LIMIT 1;");
            string worldStocksID = ExecuteRequest("SELECT Id FROM [WorldStocks] ORDER BY Id DESC LIMIT 1;");

            ExecuteRequest(string.Format("INSERT INTO [World] (UserID, TimeID, Peaceful, Swordsman, Woodman, Enemy, WorldStocksID) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');",
                userID, timeID, parameters[0], parameters[1], parameters[2], parameters[3], worldStocksID));
            return "World updated";
        }
    }


    /// <summary>
    /// получатель
    /// </summary>
    public abstract class Receiver
    {
        /// <summary>
        /// выполнить запрос
        /// </summary>
        /// <param name="request">запрос</param>
        /// <returns>результат</returns>
        public string ExecuteRequest(string request)
        {
            string connectionString = "Data Source=C:\\Users\\nidro\\Projects\\UnityProjects\\Allusion-to-Adventure\\Database.db";
            string answer = "";

            SqliteConnection connection = new SqliteConnection(connectionString);

            connection.Open();

            SqliteCommand command = new SqliteCommand(request, connection);

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
                for (int i = 0; i < reader.FieldCount; i++)
                    answer += reader[i].ToString() + "\n";

            reader.Close();
            connection.Close();

            return answer;
        }
    }
}
