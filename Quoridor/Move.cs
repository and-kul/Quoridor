namespace Quoridor
{
    public abstract class Move
    {
        protected Game Game;
        protected Player Player;
        
        protected Move(Game game, Player player)
        {
            Game = game;
            Player = player;
        }

        public abstract void Apply();
        public abstract void Rollback();
        
    }
}