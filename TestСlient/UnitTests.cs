namespace TestСlient
{
    public class UnitTests
    {
        [Fact]
        public void TestTree()
        {
            Wood wood = new Wood();
            wood.maxHealthPoints = 10;
            wood.healthPoints = 10;
            wood.BecomeStump();

        }
    }
}