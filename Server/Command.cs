namespace Server
{
    /// <summary>
    /// интерфейс команды 
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// выполнить команду
        /// </summary>
        /// <param name="parameters">параметры команды</param>
        /// <returns>результат</returns>
        public string Execute(string[] parameters);
    }


    /// <summary>
    /// авторизация
    /// </summary>
    public class Authorization : ICommand
    {
        AuthorizationReceiver receiver; // получатель


        /// <summary>
        /// конструктор авторизации
        /// </summary>
        /// <param name="receiver">получатель</param>
        public Authorization(AuthorizationReceiver receiver)
        {
            this.receiver = receiver;
        }


        /// <summary>
        /// выполнить авторизацию
        /// </summary>
        /// <param name="parameters">параметры команды</param>
        /// <returns>результат</returns>
        public string Execute(string[] parameters)
        {
            receiver.parameters = parameters;
            return receiver.Authorization();
        }
    }

    /// <summary>
    /// регистрация
    /// </summary>
    public class Registration : ICommand
    {
        RegistrationReceiver receiver; // получатель


        /// <summary>
        /// конструктор регистрации
        /// </summary>
        /// <param name="receiver">получатель</param>
        public Registration(RegistrationReceiver receiver)
        {
            this.receiver = receiver;
        }


        /// <summary>
        /// выполнить регистрацию
        /// </summary>
        /// <param name="parameters">параметры команды</param>
        /// <returns>результат</returns>
        public string Execute(string[] parameters)
        {
            receiver.parameters = parameters;
            return receiver.Registration();
        }
    }

    /// <summary>
    /// выход из аккаунта
    /// </summary>
    public class LogOut : ICommand
    {
        LogOutReceiver receiver; // получатель


        /// <summary>
        /// конструктор выхода из аккаунта
        /// </summary>
        /// <param name="receiver">получатель</param>
        public LogOut(LogOutReceiver receiver)
        {
            this.receiver = receiver;
        }


        /// <summary>
        /// выполнить выход из аккаунта
        /// </summary>
        /// <param name="parameters">параметры команды</param>
        /// <returns>результат</returns>
        public string Execute(string[] parameters)
        {
            receiver.parameters = parameters;
            return receiver.LogOut();
        }
    }
}
