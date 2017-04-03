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

            if (!WallSegment.AreAdjacent(segmentA, segmentB))
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

        public Wall(Cell[] cells, Orientation orientation)
        {
            if (cells == null) throw new ArgumentNullException(nameof(cells));

            if (cells.Length != 4) throw new ArgumentException("There must be exactly 4 cells in the array");

            Cells = new Cell[4];
            Array.Copy(cells, Cells, 4);
            Array.Sort(Cells);

            Orientation = orientation;

            if (Orientation == Orientation.Vertical)
            {
                SegmentA = new WallSegment(Cells[0], Cells[2]);
                SegmentB = new WallSegment(Cells[1], Cells[3]);
            }
            else
            {
                SegmentA = new WallSegment(Cells[0], Cells[1]);
                SegmentB = new WallSegment(Cells[2], Cells[3]);
            }
        }

        public static bool IsCorrectWallDescription(string wallDescription)
        {
            if (wallDescription == null) return false;
            if (wallDescription.Length != 3) return false;

            if (wallDescription[0] < '1' || wallDescription[0] > '8') return false;
            if (wallDescription[1] < '1' || wallDescription[1] > '8') return false;

            var orientation = wallDescription[2];

            if (orientation != 'v' && orientation != 'h') return false;

            return true;

        }



        /// <summary>
        /// Creates Wall from short string description
        /// </summary>
        /// 
        /// <param name="wallDescription">
        /// 3-character description in the format "xyO",
        /// where 'x' and 'y' - coordinates of the lower left cell out of 4,
        /// and 'O' is a wall orientation (can be either 'v' or 'h')
        /// </param>
        public Wall(String wallDescription)
        {
            if (!IsCorrectWallDescription(wallDescription))
                throw new ArgumentException("Incorrect wall description");
            
            var x = int.Parse(wallDescription.Substring(0, 1));
            var y = int.Parse(wallDescription.Substring(1, 1));

            var dx = new[] { 0, 0, 1, 1 };
            var dy = new[] { 0, 1, 0, 1 };

            var cellsList = new List<Cell>();
            for (var i = 0; i < 4; ++i)
                cellsList.Add(new Cell(x + dx[i], y + dy[i]));

            Cells = cellsList.ToArray();
            

            switch (wallDescription[2])
            {
                case 'v':
                    Orientation = Orientation.Vertical;
                    break;
                case 'h':
                    Orientation = Orientation.Horizontal;
                    break;
                default:
                    Orientation = Orientation.Unknown;
                    break;
            }

            if (Orientation == Orientation.Vertical)
            {
                SegmentA = new WallSegment(Cells[0], Cells[2]);
                SegmentB = new WallSegment(Cells[1], Cells[3]);
            }
            else
            {
                SegmentA = new WallSegment(Cells[0], Cells[1]);
                SegmentB = new WallSegment(Cells[2], Cells[3]);
            }

        }

        public Wall[] Get4IntersectedWalls()
        {
            var result = new List<Wall>();
            result.Add(this);

            if (Orientation == Orientation.Vertical)
            {
                var up = new Vector(0, 1);
                var upperWall = new Wall(Cells.Select(cell => cell + up).ToArray(), Orientation.Vertical);

                var down = new Vector(0, -1);
                var lowerWall = new Wall(Cells.Select(cell => cell + down).ToArray(), Orientation.Vertical);

                result.Add(upperWall);
                result.Add(lowerWall);

                var turnedWall = new Wall(Cells, Orientation.Horizontal);
                result.Add(turnedWall);
            }
            else
            {
                var left = new Vector(-1, 0);
                var leftWall = new Wall(Cells.Select(cell => cell + left).ToArray(), Orientation.Horizontal);

                var right = new Vector(1, 0);
                var rightWall = new Wall(Cells.Select(cell => cell + right).ToArray(), Orientation.Horizontal);

                result.Add(leftWall);
                result.Add(rightWall);

                var turnedWall = new Wall(Cells, Orientation.Vertical);
                result.Add(turnedWall);
            }

            return result.ToArray();
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

                hashCode = (hashCode * 401) ^ (int) Orientation;

                //var hashCode = SegmentA.GetHashCode();
                //hashCode = (hashCode * 397) ^ SegmentB.GetHashCode();
                //hashCode = (hashCode * 397) ^ (int)Orientation;
                return hashCode;
            }
        }
    }
}