namespace Quoridor
{
    public class PawnMove : Move
    {
        public Cell From;
        public Cell To;

        public PawnMove(Game game, Player player, Cell from, Cell to) : base(game, player)
        {
            From = from;
            To = to;
        }

        public override void Apply()
        {
            Player.CurrentPosition = To;
        }

        public override void Rollback()
        {
            Player.CurrentPosition = From;
        }
    }
}