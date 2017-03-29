using NUnit.Framework;
using Quoridor;

namespace Tests
{
    [TestFixture]
    public class WallTests
    {
        private Cell[,] c;

        private WallSegment segmentA;
        private WallSegment segmentB;
        private WallSegment segmentC;
        private WallSegment segmentD;
        private WallSegment segmentE;

        private Wall wall_A_B;
        private Wall wall_A_B_Copy;
        private Wall wall_B_A;
        private Wall wall_B_C;
        private Wall wall_D_E;
        private Wall wall_D_E_Copy;
        private Wall wall_E_D;


        [SetUp]
        public void SetUp()
        {
            c = new Cell[10, 10];
            for (var x = 1; x <= 9; ++x)
                for (var y = 1; y <= 9; ++y)
                    c[x, y] = new Cell(x, y);

            segmentA = new WallSegment(c[2, 5], c[3, 5]);
            segmentB = new WallSegment(c[3, 4], c[2, 4]);
            segmentC = new WallSegment(c[2, 3], c[3, 3]);
            segmentD = new WallSegment(c[2, 5], c[2, 4]);
            segmentE = new WallSegment(c[3, 5], c[3, 4]);

            wall_A_B = new Wall(segmentA, segmentB);
            wall_B_C = new Wall(segmentB, segmentC);
            wall_D_E = new Wall(segmentD, segmentE);

            wall_A_B_Copy = new Wall(segmentA, segmentB);
            wall_B_A = new Wall(segmentB, segmentA);

            wall_D_E_Copy = new Wall(segmentD, segmentE);
            wall_E_D = new Wall(segmentE, segmentD);
        }

        [Test]
        public void SegmentsOrder()
        {
            Assert.That(wall_A_B.SegmentA.Equals(segmentB));
            Assert.That(wall_A_B.SegmentB.Equals(segmentA));

            Assert.That(wall_B_A.SegmentA.Equals(segmentB));
            Assert.That(wall_B_A.SegmentB.Equals(segmentA));

            Assert.That(wall_D_E.SegmentA.Equals(segmentD));
            Assert.That(wall_D_E.SegmentB.Equals(segmentE));

            Assert.That(wall_E_D.SegmentA.Equals(segmentD));
            Assert.That(wall_E_D.SegmentB.Equals(segmentE));
        }


        [Test]
        public void Intersection()
        {
            Assert.That(wall_A_B.DoesIntersect(wall_D_E));
            Assert.That(wall_A_B.DoesIntersect(wall_B_C));
            Assert.That(wall_A_B.DoesIntersect(wall_A_B_Copy));

            Assert.That(wall_D_E.DoesNotIntersect(wall_B_C));
        }

        [Test]
        public void WallsEquality()
        {
            Assert.That(wall_A_B.Equals(wall_A_B_Copy));
            Assert.That(wall_A_B.Equals(wall_B_A));

            Assert.That(wall_A_B.Equals(wall_D_E), Is.False);

            Assert.That(wall_A_B.Equals(wall_B_C), Is.False);
        }


        [Test]
        public void HashCodeTest()
        {
            Assert.That(wall_A_B.GetHashCode(), Is.EqualTo(wall_A_B_Copy.GetHashCode()));
            Assert.That(wall_A_B.GetHashCode(), Is.EqualTo(wall_B_A.GetHashCode()));

            Assert.That(wall_D_E.GetHashCode(), Is.EqualTo(wall_D_E_Copy.GetHashCode()));
            Assert.That(wall_D_E.GetHashCode(), Is.EqualTo(wall_E_D.GetHashCode()));
        }

        [Test]
        public void ConstructorFromCells()
        {
            var wall_A_B_Cells = new Wall(new[] {c[2, 4], c[3, 5], c[3, 4], c[2, 5]}, Orientation.Vertical);
            var wall_B_C_Cells = new Wall(new[] { c[2, 4], c[3, 4], c[3, 3], c[2, 3] }, Orientation.Vertical);
            var wall_D_E_Cells = new Wall(new[] { c[2, 4], c[2, 5], c[3, 4], c[3, 5] }, Orientation.Horizontal);

            Assert.That(wall_A_B.Equals(wall_A_B_Cells));
            Assert.That(wall_B_C.Equals(wall_B_C_Cells));
            Assert.That(wall_D_E.Equals(wall_D_E_Cells));
            
            Assert.That(wall_A_B.SegmentA.Equals(wall_A_B_Cells.SegmentA));
            Assert.That(wall_A_B.SegmentB.Equals(wall_A_B_Cells.SegmentB));

            Assert.That(wall_B_C.SegmentA.Equals(wall_B_C_Cells.SegmentA));
            Assert.That(wall_B_C.SegmentB.Equals(wall_B_C_Cells.SegmentB));

            Assert.That(wall_D_E.SegmentA.Equals(wall_D_E_Cells.SegmentA));
            Assert.That(wall_D_E.SegmentB.Equals(wall_D_E_Cells.SegmentB));
            
        }
    }
}