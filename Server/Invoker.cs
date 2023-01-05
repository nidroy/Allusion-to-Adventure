namespace Server
{
    /// <summary>
    /// инициатор команды
    /// </summary>
    public class Invoker
    {
        private ICommand command; // команда
        private string[] parameters; // параметры команды


        /// <summary>
        /// установить команду
        /// </summary>
        public void SetCommand(ICommand command)
        {
            this.command = command;
        }

        /// <summary>
        /// установить параметры команды
        /// </summary>
        /// <param name="parameters">параметры команды</param>
        public void SetParameters(string[] parameters)
        {
            this.parameters = parameters;
        }

        /// <summary>
        /// выполнить команду
        /// </summary>
        /// <returns>результат</returns>
        public string ExecuteCommand()
        {
            return command.Execute(parameters);
        }
    }
}
