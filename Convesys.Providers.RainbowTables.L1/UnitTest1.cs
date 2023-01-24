namespace Convesys.Providers.RainbowTables.L1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {

            Generator.RunSync(new char[] { 'a', 'b', 'c', '1', '2', '3' });
            Assert.Pass();
        }
    }
}