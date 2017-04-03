using System;
using System.Collections.Generic;

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

        public abstract Move CreateMove();

        protected Player Opponent => Game.Players[Id ^ 1];

        public List<PawnMove> GetPossiblePawnMoves()
        {
            var result = new List<PawnMove>();

            foreach (var neighbor in Game.Neighbors[CurrentPosition])
            {
                if (neighbor != Opponent.CurrentPosition)
                {
                    result.Add(new PawnMove(Game, this, CurrentPosition, neighbor));
                }
                else
                {
                    var vector = new Vector(CurrentPosition, neighbor);
                    var behindOpponent = neighbor + vector;

                    if (Game.Neighbors[neighbor].Contains(behindOpponent))
                    {
                        result.Add(new PawnMove(Game, this, CurrentPosition, behindOpponent));
                        continue;
                    }

                    var toTheLeftOfOpponent = neighbor + vector.TurnLeft();
                    var toTheRightOfOpponent = neighbor + vector.TurnRight();

                    if (Game.Neighbors[neighbor].Contains(toTheLeftOfOpponent))
                        result.Add(new PawnMove(Game, this, CurrentPosition, toTheLeftOfOpponent));

                    if (Game.Neighbors[neighbor].Contains(toTheRightOfOpponent))
                        result.Add(new PawnMove(Game, this, CurrentPosition, toTheRightOfOpponent));
                    
                }
            }

            return result;
        }




    }
}