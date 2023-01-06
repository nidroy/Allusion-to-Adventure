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

    /// <summary>
    /// обновление времени
    /// </summary>
    public class UpdateTime : ICommand
    {
        UpdateTimeReceiver receiver; // получатель


        /// <summary>
        /// конструктор обновления времени
        /// </summary>
        /// <param name="receiver">получатель</param>
        public UpdateTime(UpdateTimeReceiver receiver)
        {
            this.receiver = receiver;
        }


        /// <summary>
        /// выполнить обновление времени
        /// </summary>
        /// <param name="parameters">параметры команды</param>
        /// <returns>результат</returns>
        public string Execute(string[] parameters)
        {
            receiver.parameters = parameters;
            return receiver.UpdateTime();
        }
    }

    /// <summary>
    /// обновление ресурсов
    /// </summary>
    public class UpdateResources : ICommand
    {
        UpdateResourcesReceiver receiver; // получатель


        /// <summary>
        /// конструктор обновления ресурсов
        /// </summary>
        /// <param name="receiver">получатель</param>
        public UpdateResources(UpdateResourcesReceiver receiver)
        {
            this.receiver = receiver;
        }


        /// <summary>
        /// выполнить обновление ресурсов
        /// </summary>
        /// <param name="parameters">параметры команды</param>
        /// <returns>результат</returns>
        public string Execute(string[] parameters)
        {
            receiver.parameters = parameters;
            return receiver.UpdateResources();
        }
    }

    // <summary>
    /// обновление мира
    /// </summary>
    public class UpdateWorld : ICommand
    {
        UpdateWorldReceiver receiver; // получатель


        /// <summary>
        /// конструктор обновления мира
        /// </summary>
        /// <param name="receiver">получатель</param>
        public UpdateWorld(UpdateWorldReceiver receiver)
        {
            this.receiver = receiver;
        }


        /// <summary>
        /// выполнить обновление мира
        /// </summary>
        /// <param name="parameters">параметры команды</param>
        /// <returns>результат</returns>
        public string Execute(string[] parameters)
        {
            receiver.parameters = parameters;
            return receiver.UpdateWorld();
        }
    }
}
