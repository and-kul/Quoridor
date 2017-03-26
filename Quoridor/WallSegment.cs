using System;

namespace Quoridor
{
    public class WallSegment : IEquatable<WallSegment>
    {
        public readonly Cell Cell1;
        public readonly Cell Cell2;
        public readonly Orientation Orientation;

        public WallSegment(Cell cell1, Cell cell2)
        {
            if (cell1 == null)
                throw new ArgumentNullException(nameof(cell1));

            if (cell2 == null)
                throw new ArgumentNullException(nameof(cell2));

            if (!Cell.AreAdjacent(cell1, cell2))
                throw new ArgumentException("Cells are not located side by side");
            
            if (cell1 > cell2)
            {
                var tmp = cell1;
                cell1 = cell2;
                cell2 = tmp;
            }
            
            Cell1 = cell1;
            Cell2 = cell2;

            Orientation = cell1.Y == cell2.Y ? Orientation.Vertical : Orientation.Horizontal;
        }

        public static bool AreAdjacent(WallSegment segmentA, WallSegment segmentB)
        {
            if (segmentA.Orientation != segmentB.Orientation)
                return false;

            if (segmentA.Orientation == Orientation.Vertical)
            {
                var sameX = segmentA.Cell1.X == segmentB.Cell1.X;
                var consecutiveY = Math.Abs(segmentA.Cell1.Y - segmentB.Cell1.Y) == 1;

                return sameX && consecutiveY;
            }
            else
            {
                var sameY = segmentA.Cell1.Y == segmentB.Cell1.Y;
                var consecutiveX = Math.Abs(segmentA.Cell1.X - segmentB.Cell1.X) == 1;

                return sameY && consecutiveX;

            }
            
        }

        

        public bool Equals(WallSegment other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Cell1.Equals(other.Cell1) && Cell2.Equals(other.Cell2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WallSegment) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Cell1.GetHashCode() * 397) ^ Cell2.GetHashCode();
            }
        }
    }
}