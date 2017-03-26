using NUnit.Framework;
using Quoridor;

namespace Tests
{
    [TestFixture]
    public class WallTests
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
        public void Intersection()
        {
            var segmentA = new WallSegment(c[2, 5], c[3, 5]);
            var segmentB = new WallSegment(c[3, 4], c[2, 4]);
            var segmentC = new WallSegment(c[2, 3], c[3, 3]);

            var wall_A_B = new Wall(segmentA, segmentB);
            var wall_B_C = new Wall(segmentB, segmentC);

            Assert.That(wall_A_B.DoesIntersect(wall_B_C));

            var segmentD = new WallSegment(c[2, 5], c[2, 4]);
            var segmentE = new WallSegment(c[3, 5], c[3, 4]);

            var wall_D_E = new Wall(segmentD, segmentE);

            Assert.That(wall_A_B.DoesIntersect(wall_D_E));

            Assert.That(wall_D_E.DoesNotIntersect(wall_B_C));

            var wall_A_B_Copy = new Wall(segmentA, segmentB);

            Assert.That(wall_A_B.DoesIntersect(wall_A_B_Copy));
        }
    }
}