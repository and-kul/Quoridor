using NUnit.Framework;
using Quoridor;


namespace Tests
{
    [TestFixture]
    public class CellTests
    {
        [Test]
        public void Adjacency()
        {
            var c_2_3 = new Cell(2, 3);
            var c_2_3_Copy = new Cell(2, 3);

            var c_2_4 = new Cell(2, 4);
            var c_2_5 = new Cell(2, 5);

            var c_1_3 = new Cell(1, 3);
            var c_5_3 = new Cell(5, 3);

            Assert.That(Cell.AreAdjacent(c_2_3, c_2_3_Copy), Is.False);

            Assert.That(Cell.AreAdjacent(c_2_3, c_2_4), Is.True);
            Assert.That(Cell.AreAdjacent(c_2_3, c_2_5), Is.False);

            Assert.That(Cell.AreAdjacent(c_1_3, c_2_3), Is.True);
            Assert.That(Cell.AreAdjacent(c_5_3, c_2_3), Is.False);
            
            Assert.That(Cell.AreAdjacent(c_1_3, c_2_4), Is.False);

        }
    }
}