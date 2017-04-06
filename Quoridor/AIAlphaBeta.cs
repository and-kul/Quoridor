using System;
using System.Collections.Generic;

namespace Quoridor
{
    public class AIAlphaBeta : Player
    {
        private readonly int maxDepth;
        private const int WinPoints = 10000;

        public AIAlphaBeta(string name, int maxDepth) : base(name)
        {
            this.maxDepth = maxDepth;
            bestMoves = new List<Move>();
        }

        private readonly List<Move> bestMoves;

        private int Heuristic()
        {
            return Opponent.GetShortestPathLength() - GetShortestPathLength();
        }


        private int DFS(int depth, int alpha, int beta)
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
                int maxResult = int.MinValue;
                foreach (var myMove in GetAllPossibleMoves())
                {
                    myMove.Apply();

                    int moveResult = DFS(depth + 1, alpha, beta);
                    alpha = Math.Max(alpha, moveResult);

                    if (depth == 0 && moveResult == maxResult)
                        bestMoves.Add(myMove);

                    if (moveResult > maxResult)
                    {
                        maxResult = moveResult;
                        if (depth == 0)
                        {
                            bestMoves.Clear();
                            bestMoves.Add(myMove);
                        }
                    }
                    myMove.Rollback();

                    if (maxResult > beta)
                        break;
                }
                return maxResult;
            }
            //min for opponent
            else
            {
                int minResult = int.MaxValue;
                foreach (var opponentMove in Opponent.GetAllPossibleMoves())
                {
                    opponentMove.Apply();

                    int moveResult = DFS(depth + 1, alpha, beta);
                    beta = Math.Min(beta, moveResult);

                    if (moveResult < minResult)
                    {
                        minResult = moveResult;
                    }
                    opponentMove.Rollback();

                    if (minResult < alpha)
                        break;
                }
                return minResult;
            }
        }
        
        public override Move CreateMove()
        {
            DFS(0, int.MinValue, int.MaxValue);
            return Helpers.PickRandomElement(bestMoves);
        }
    }
}
