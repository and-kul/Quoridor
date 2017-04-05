using System;
using System.Collections.Generic;
using System.Linq;

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

        public Player Opponent => Game.Players[Id ^ 1];


        public List<Cell> GetPossibleCellsToMoveTo()
        {
            var result = new List<Cell>();

            foreach (var neighbor in Game.Neighbors[CurrentPosition])
            {
                if (neighbor != Opponent.CurrentPosition)
                {
                    result.Add(neighbor);
                }
                else
                {
                    var vector = new Vector(CurrentPosition, neighbor);
                    var behindOpponent = neighbor + vector;

                    if (Game.Neighbors[neighbor].Contains(behindOpponent))
                    {
                        result.Add(behindOpponent);
                        continue;
                    }

                    var toTheLeftOfOpponent = neighbor + vector.TurnLeft();
                    var toTheRightOfOpponent = neighbor + vector.TurnRight();

                    if (Game.Neighbors[neighbor].Contains(toTheLeftOfOpponent))
                        result.Add(toTheLeftOfOpponent);

                    if (Game.Neighbors[neighbor].Contains(toTheRightOfOpponent))
                        result.Add(toTheRightOfOpponent);
                }
            }

            return result;
        }


        public List<PawnMove> GetPossiblePawnMoves()
        {
            return
                GetPossibleCellsToMoveTo()
                    .Select(possibleCell => new PawnMove(Game, this, CurrentPosition, possibleCell))
                    .ToList();
        }


        public int GetCurrentDistanceToTargetSide()
        {
            var aStar = new AStar(this);

            return aStar.GetShortestPathLength();
        }
    }
}