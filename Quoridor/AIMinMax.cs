using System;
using System.Collections.Generic;

namespace Quoridor
{
    public class AIMinMax : Player
    {
        private readonly int maxDepth;
        private const int WinPoints = 10000;

        public AIMinMax(string name, int maxDepth) : base(name)
        {
            if (maxDepth < 1)
                throw new QuoridorException("maxDepth cannot be less than 1");
            this.maxDepth = maxDepth;
            bestMoves = new List<Move>();
        }

        public AIMinMax(Player player, int maxDepth) : this(player.Name, maxDepth)
        {
            Id = player.Id;
            TargetY = player.TargetY;
            CurrentPosition = player.CurrentPosition;
            WallsRemaining = player.WallsRemaining;
            Game = player.Game;
        }



        private int Heuristic()
        {
            return Opponent.GetShortestPathLength() - GetShortestPathLength();
        }

        private readonly List<Move> bestMoves;
        
        private int DFS(int depth)
        {
            if (depth == 0)
                bestMoves.Clear();

            if (CurrentPosition.Y == TargetY)
                return WinPoints;
            if (Opponent.CurrentPosition.Y == Opponent.TargetY)
                return -WinPoints;

            if (depth == maxDepth)
            {
                return Heuristic();
            }

            // max for me
            if (depth % 2 == 0)
            {
                int maxHeuristic = int.MinValue;
                foreach (var myMove in GetAllPossibleMoves())
                {
                    myMove.Apply();

                    int moveResult = DFS(depth + 1);

                    if (depth == 0 && moveResult == maxHeuristic)
                        bestMoves.Add(myMove);

                    if (moveResult > maxHeuristic)
                    {
                        maxHeuristic = moveResult;
                        if (depth == 0)
                        {
                            bestMoves.Clear();
                            bestMoves.Add(myMove);
                        }
                    }
                    myMove.Rollback();
                }
                return maxHeuristic;
            }
            //min for opponent
            else
            {
                int minHeuristic = int.MaxValue;
                foreach (var opponentMove in Opponent.GetAllPossibleMoves())
                {
                    opponentMove.Apply();

                    int moveResult = DFS(depth + 1);
                    
                    if (moveResult < minHeuristic)
                    {
                        minHeuristic = moveResult;
                    }
                    opponentMove.Rollback();
                }
                return minHeuristic;
            }

        }


        public override Move CreateMove()
        {
            DFS(0);
            return Helper.PickRandomElement(bestMoves);
        }
    }
}