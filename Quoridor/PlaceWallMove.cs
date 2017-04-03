using System;

namespace Quoridor
{
    public class PlaceWallMove : Move
    {
        public Wall PlacedWall;
        public Wall[] RejectedWalls;

        public PlaceWallMove(Game game, Player player, Wall placedWall, Wall[] rejectedWalls) : base(game, player)
        {
            PlacedWall = placedWall;
            RejectedWalls = rejectedWalls;
        }

        public override void Apply()
        {
            if (Player.WallsRemaining == 0)
                throw new Exception("Player " + Player.Name + " can no longer place walls");

            Player.WallsRemaining--;

            Game.Neighbors[PlacedWall.SegmentA.Cell1].Remove(PlacedWall.SegmentA.Cell2);
            Game.Neighbors[PlacedWall.SegmentA.Cell2].Remove(PlacedWall.SegmentA.Cell1);

            Game.Neighbors[PlacedWall.SegmentB.Cell1].Remove(PlacedWall.SegmentB.Cell2);
            Game.Neighbors[PlacedWall.SegmentB.Cell2].Remove(PlacedWall.SegmentB.Cell1);

            Game.WallsOnBoard.Add(PlacedWall);
            foreach (var rejectedWall in RejectedWalls)
            {
                Game.PossibleWalls.Remove(rejectedWall);
            }
        }

        public override void Rollback()
        {
            foreach (var rejectedWall in RejectedWalls)
            {
                Game.PossibleWalls.Add(rejectedWall);
            }
            Game.WallsOnBoard.Remove(PlacedWall);

            Game.Neighbors[PlacedWall.SegmentA.Cell1].Add(PlacedWall.SegmentA.Cell2);
            Game.Neighbors[PlacedWall.SegmentA.Cell2].Add(PlacedWall.SegmentA.Cell1);
                                                      
            Game.Neighbors[PlacedWall.SegmentB.Cell1].Add(PlacedWall.SegmentB.Cell2);
            Game.Neighbors[PlacedWall.SegmentB.Cell2].Add(PlacedWall.SegmentB.Cell1);

            Player.WallsRemaining++;
        }
    }
}