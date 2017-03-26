using System;
using System.Collections.Generic;
using System.Linq;

namespace Quoridor
{
    public class Wall
    {
        public readonly WallSegment SegmentA;
        public readonly WallSegment SegmentB;
        public readonly Orientation Orientation;

        public Wall(WallSegment segmentA, WallSegment segmentB)
        {
            if (segmentA == null) throw new ArgumentNullException(nameof(segmentA));
            if (segmentB == null) throw new ArgumentNullException(nameof(segmentB));

            if (!WallSegment.AreAdjacent(segmentA,segmentB))
                throw new ArgumentException("Wall segments are not adjacent");

            SegmentA = segmentA;
            SegmentB = segmentB;
            Orientation = segmentA.Orientation;
        }

        public bool DoesIntersect(Wall other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            if (Orientation != other.Orientation)
            {
                var cells1 = new HashSet<Cell>
                {
                    SegmentA.Cell1,
                    SegmentA.Cell2,
                    SegmentB.Cell1,
                    SegmentB.Cell2
                };

                var cells2 = new HashSet<Cell>
                {
                    other.SegmentA.Cell1,
                    other.SegmentA.Cell2,
                    other.SegmentB.Cell1,
                    other.SegmentB.Cell2
                };

                if (cells1.SetEquals(cells2))
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




    }
}