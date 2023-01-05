using System.Data.SqlClient;

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
                    string[] usernames = ExecuteRequest("SELECT Username FROM Character;").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string username in usernames)
                        if (username == parameters[0])
                            return "Successful";

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
                    string[] usernames = ExecuteRequest("SELECT Username FROM Character;").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string username in usernames)
                        if (username == parameters[0])
                            return "User exists";

                    ExecuteRequest(string.Format("INSERT INTO Character (Username, Password) VALUES ('{0}', '{1}');", parameters[0], parameters[1]));
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
    /// получатель
    /// </summary>
    public abstract class Receiver
    {
        public string ExecuteRequest(string request)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nidro\\Projects\\UnityProjects\\Allusion-to-Adventure\\Database.mdf;Integrated Security=True;Connect Timeout=30";
            string answer = "";

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand command = new SqlCommand(request, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
                for (int i = 0; i < reader.FieldCount; i++)
                    answer += reader[i].ToString() + "\n";

            reader.Close();
            connection.Close();

            return answer;
        }
    }
}
