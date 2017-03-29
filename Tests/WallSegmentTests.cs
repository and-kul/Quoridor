using NUnit.Framework;
using Quoridor;

namespace Tests
{
    [TestFixture]
    public class WallSegmentTests
    {
        private Cell c_2_5;
        private Cell c_3_5;
        private Cell c_2_4;

        private Cell[,] c;


        [SetUp]
        public void SetUp()
        {
            c_2_5 = new Cell(2, 5);
            c_3_5 = new Cell(3, 5);
            c_2_4 = new Cell(2, 4);

            c = new Cell[10, 10];
            for (var x = 1; x <= 9; ++x)
                for (var y = 1; y <= 9; ++y)
                    c[x, y] = new Cell(x, y);
        }


        [Test]
        public void BlocksAdjacentCells()
        {
            Assert.That(() =>
            {
                var block = new WallSegment(c_2_5, c_3_5);
            }, Throws.Nothing);
            Assert.That(() =>
            {
                var block = new WallSegment(c_3_5, c_2_4);
            }, Throws.ArgumentException);
        }


        [Test]
        public void WallSegmentsEquality()
        {
            var c_2_5_Copy = new Cell(2, 5);
            var c_3_5_Copy = new Cell(3, 5);

            Assert.That(new WallSegment(c_2_5, c_3_5), Is.EqualTo(new WallSegment(c_3_5, c_2_5)));
            Assert.That(new WallSegment(c_2_5, c_3_5), Is.EqualTo(new WallSegment(c_2_5_Copy, c_3_5_Copy)));
            Assert.That(new WallSegment(c_2_5, c_3_5), Is.EqualTo(new WallSegment(c_3_5_Copy, c_2_5_Copy)));
        }

        [Test]
        public void SegmentsAdjacency()
        {
            var segmentA = new WallSegment(c[2, 5], c[3, 5]);
            var segmentB = new WallSegment(c[3, 4], c[2, 4]);
            var segmentC = new WallSegment(c[2, 3], c[3, 3]);

            Assert.That(WallSegment.AreAdjacent(segmentA, segmentB));
            Assert.That(WallSegment.AreAdjacent(segmentA, segmentC), Is.False);

            var segmentD = new WallSegment(c[2, 5], c[2, 4]);
            var segmentE = new WallSegment(c[3, 5], c[3, 4]);

            Assert.That(WallSegment.AreAdjacent(segmentD, segmentE));
        }
    }
}