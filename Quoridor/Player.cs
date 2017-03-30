using System;

namespace Quoridor
{
    
    public abstract class Player
    {
        public readonly string Name;

        public int Id;
        public int TargetY;
        public Cell CurrentPosition;
        public int WallsRemaining;

        public Game Game;

        protected Player(string name)
        {
            Name = name;
        }
        

        

    }
}