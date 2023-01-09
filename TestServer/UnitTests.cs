using Moq;
using Server;

namespace TestServer
{
    public class UnitTests
    {
        [Fact]
        public void TestIncorrectUsername()
        {
            string username = "1234";
            string password = "123";

            string[] parameters = new string[2];
            parameters[0] = username;
            parameters[1] = password;

            Invoker invoker = new Invoker();
            invoker.SetCommand(new Authorization(new AuthorizationReceiver()));
            invoker.SetParameters(parameters);

            Assert.Equal("User not found", invoker.ExecuteCommand());
        }

        [Fact]
        public void TestIncorrectPassword()
        {
            string username = "Name";
            string password = "Pas";

            string[] parameters = new string[2];
            parameters[0] = username;
            parameters[1] = password;

            Invoker invoker = new Invoker();
            invoker.SetCommand(new Authorization(new AuthorizationReceiver()));
            invoker.SetParameters(parameters);

            Assert.Equal("Incorrect password", invoker.ExecuteCommand());
        }

        [Fact]
        public void TestAuthorization()
        {
            string username = "Name";
            string password = "pass";

            string[] parameters = new string[2];
            parameters[0] = username;
            parameters[1] = password;

            Invoker invoker = new Invoker();
            invoker.SetCommand(new Authorization(new AuthorizationReceiver()));
            invoker.SetParameters(parameters);

            Assert.Equal("Successful", invoker.ExecuteCommand());
        }

        [Fact]
        public void TestIncorrectRegistration()
        {
            string username = "Name";
            string password = "pass";

            string[] parameters = new string[2];
            parameters[0] = username;
            parameters[1] = password;

            Invoker invoker = new Invoker();
            invoker.SetCommand(new Registration(new RegistrationReceiver()));
            invoker.SetParameters(parameters);

            Assert.Equal("User exists", invoker.ExecuteCommand());
        }
    }
}