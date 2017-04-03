namespace Quoridor
{
    public class Vector
    {
        public int X;
        public int Y;

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector(Cell from, Cell to)
        {
            X = to.X - from.X;
            Y = to.Y - from.Y;
        }

        public Vector TurnLeft()
        {
            return new Vector(-Y, X);
        }

        public Vector TurnRight()
        {
            return new Vector(Y, -X);
        }


    }
}