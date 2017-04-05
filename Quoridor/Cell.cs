using System;
using System.Collections.Generic;

namespace Quoridor
{
    public class Cell : IComparable<Cell>, IEquatable<Cell>
    {
        public readonly int X;
        public readonly int Y;


        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Cell(string cellDescription)
        {
            if (!IsCorrectCellDescription(cellDescription))
                throw new ArgumentException("Incorrect cell description");

            X = int.Parse(cellDescription.Substring(0, 1));
            Y = int.Parse(cellDescription.Substring(1, 1));
        }

        public static bool IsCorrectCellDescription(string cellDescription)
        {
            if (cellDescription == null) return false;
            if (cellDescription.Length != 2) return false;

            if (cellDescription[0] < '1' || cellDescription[0] > '9') return false;
            if (cellDescription[1] < '1' || cellDescription[1] > '9') return false;
            
            return true;
        }


        public static bool AreAdjacent(Cell cell1, Cell cell2)
        {
            if (cell1 == null) throw new ArgumentNullException(nameof(cell1));
            if (cell2 == null) throw new ArgumentNullException(nameof(cell2));

            var sameX = cell1.X == cell2.X;
            var sameY = cell1.Y == cell2.Y;

            var consecutiveX = Math.Abs(cell1.X - cell2.X) == 1;
            var consecutiveY = Math.Abs(cell1.Y - cell2.Y) == 1;

            return sameX && consecutiveY || sameY && consecutiveX;
        }

        public static Cell operator +(Cell cell, Vector vector)
        {
            return new Cell(cell.X + vector.X, cell.Y + vector.Y);
        }



        public int CompareTo(Cell other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            var xComparison = X.CompareTo(other.X);
            return xComparison != 0 ? xComparison : Y.CompareTo(other.Y);
        }

        public static bool operator <(Cell left, Cell right)
        {
            return Comparer<Cell>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(Cell left, Cell right)
        {
            return Comparer<Cell>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(Cell left, Cell right)
        {
            return Comparer<Cell>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(Cell left, Cell right)
        {
            return Comparer<Cell>.Default.Compare(left, right) >= 0;
        }


        public static bool operator ==(Cell left, Cell right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Cell left, Cell right)
        {
            return !Equals(left, right);
        }

        public bool Equals(Cell other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Cell) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }
}