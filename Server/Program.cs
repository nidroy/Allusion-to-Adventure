namespace Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            Stub stub = new Stub();
            Thread thread = new Thread(new ThreadStart(stub.Process));
            thread.Start();
        }
    }
}

