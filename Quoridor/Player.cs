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

        protected Player(string name)
        {
            Name = name;
        }
        

        private Game game;

    }
}