using System;
using System.Linq;
using NUnit.Framework;
using Quoridor;


namespace Tests
{
    [TestFixture]
    public class CellTests
    {
        private Cell[,] c;


        [SetUp]
        public void SetUp()
        {
            c = new Cell[10, 10];
            for (var x = 1; x <= 9; ++x)
                for (var y = 1; y <= 9; ++y)
                    c[x, y] = new Cell(x, y);
        }


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

        [Test]
        public void SortTest()
        {
            var sortedSample = new[] {c[0, 0], c[0, 1], c[1, 0], c[1, 1]};

            var cells = new[] {c[0, 0], c[1, 1], c[1, 0], c[0, 1]};
            Array.Sort(cells);

            Assert.That(cells.SequenceEqual(sortedSample));
        }

        [Test]
        public void EqualityOperatorTest()
        {
            var c_2_3 = new Cell(2, 3);
            var c_2_3_Copy = new Cell(2, 3);
            var c_2_4 = new Cell(2, 4);

            Assert.That(c_2_3 == c_2_3_Copy);

            Assert.That(c_2_3 != c_2_4);
        }


    }
}