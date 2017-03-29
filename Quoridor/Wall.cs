using System;
using System.Collections.Generic;
using System.Linq;

namespace Quoridor
{
    public class Wall : IEquatable<Wall>
    {
        public readonly WallSegment SegmentA;
        public readonly WallSegment SegmentB;
        public readonly Orientation Orientation;
        public readonly Cell[] Cells;

        public Wall(WallSegment segmentA, WallSegment segmentB)
        {
            if (segmentA == null) throw new ArgumentNullException(nameof(segmentA));
            if (segmentB == null) throw new ArgumentNullException(nameof(segmentB));

            if (!WallSegment.AreAdjacent(segmentA,segmentB))
                throw new ArgumentException("Wall segments are not adjacent");

            Orientation = segmentA.Orientation;

            if (Orientation == Orientation.Vertical)
            {
                if (segmentA.Cell1.Y > segmentB.Cell1.Y)
                {
                    Helper.Swap(ref segmentA, ref segmentB);
                }
            }
            else
            {
                if (segmentA.Cell1.X > segmentB.Cell1.X)
                {
                    Helper.Swap(ref segmentA, ref segmentB);
                }
            }
            

            SegmentA = segmentA;
            SegmentB = segmentB;
            

            Cells = new[]
            {
                segmentA.Cell1,
                segmentA.Cell2,
                segmentB.Cell1,
                segmentB.Cell2
            };

            Array.Sort(Cells);
        }

        


        public bool DoesIntersect(Wall other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            if (Orientation != other.Orientation)
            {
                if (Cells.SequenceEqual(other.Cells))
                    return true;
            }
            else
            {
                var segments1 = new List<WallSegment>
                {
                    SegmentA,
                    SegmentB
                };

                var segments2 = new List<WallSegment>
                {
                    other.SegmentA,
                    other.SegmentB
                };

                if (segments1.Intersect(segments2).Any())
                    return true;
            }

            return false;
        }

        public bool DoesNotIntersect(Wall other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            return !DoesIntersect(other);
        }

        public bool Equals(Wall other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Orientation == other.Orientation && Cells.SequenceEqual(other.Cells);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            
            return Equals((Wall) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 0;
                foreach (var cell in Cells)
                {
                    hashCode = (hashCode * 401) ^ cell.GetHashCode();
                }

                hashCode = (hashCode * 401) ^ (int)Orientation;
                
                //var hashCode = SegmentA.GetHashCode();
                //hashCode = (hashCode * 397) ^ SegmentB.GetHashCode();
                //hashCode = (hashCode * 397) ^ (int)Orientation;
                return hashCode;
            }
        }
    }
}