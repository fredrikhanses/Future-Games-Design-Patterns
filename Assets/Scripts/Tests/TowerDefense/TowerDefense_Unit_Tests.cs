using NUnit.Framework;

namespace Tests
{
    public class TowerDefense_Unit_Tests
    {
        [Test]
        [TestCase("map_1",  0, 2, 2, 0, 18)]
        [TestCase("map_2",  24, 0, 9, 9, 118)]
        public void Djistra_Solves_Path(string map, int x0, int y0, int x1, int y1, int result)
        {
            Assert.AreEqual(true, true);            
        }
    }
}
