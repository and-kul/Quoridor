using System;
using System.Collections.Generic;

namespace Quoridor
{
    public class AStar
    {
        private readonly Player player;
        private readonly Game game;

        private readonly SortedSet<Cell> openSorted;
        private readonly HashSet<Cell> open;
        private readonly HashSet<Cell> closed;

        private readonly Dictionary<Cell, int> gCost;
        private int HCost(Cell cell) => Math.Abs(cell.Y - player.TargetY);
        private int FCost(Cell cell) => gCost[cell] + HCost(cell);

        private readonly Dictionary<Cell, Cell> prev;
        private Cell finishCell;

        private bool searchIsCompleted;

        public List<Cell> GetShortestPath()
        {
            if (!searchIsCompleted)
                FindShortestPath();

            var result = new List<Cell>();

            var current = finishCell;
            while (current != null)
            {
                result.Add(current);
                current = prev[current];
            }

            if (result.Count > 0)
            {
                result.RemoveAt(result.Count - 1);
                result.Reverse();
            }

            return result;
        }

        public int GetShortestPathLength()
        {
            if (!searchIsCompleted) 
                FindShortestPath();

            if (finishCell == null)
                return Int32.MaxValue;

            return gCost[finishCell];
        }

        public bool DoesPathExist()
        {
            if (!searchIsCompleted)
                FindShortestPath();

            return finishCell != null;
        }


        private class OpenCellComparer : Comparer<Cell>
        {
            private readonly AStar aStar;

            public OpenCellComparer(AStar aStar)
            {
                this.aStar = aStar;
            }

            public override int Compare(Cell left, Cell right)
            {
                if (Cell.Equals(left, right)) return 0;

                var left_FCost = aStar.FCost(left);
                var right_FCost = aStar.FCost(right);

                if (left_FCost != right_FCost) return left_FCost.CompareTo(right_FCost);

                var left_HCost = aStar.HCost(left);
                var right_HCost = aStar.HCost(right);

                if (left_HCost != right_HCost) return left_HCost.CompareTo(right_HCost);

                return left.CompareTo(right);
            }
        }

        public AStar(Player player)
        {
            this.player = player;
            game = player.Game;

            openSorted = new SortedSet<Cell>(new OpenCellComparer(this));
            open = new HashSet<Cell>();
            closed = new HashSet<Cell>();
            gCost = new Dictionary<Cell, int>();
            prev = new Dictionary<Cell, Cell>();

            gCost[player.CurrentPosition] = 0;
            prev[player.CurrentPosition] = null;
            closed.Add(player.CurrentPosition);

            foreach (var possibleCell in player.GetPossibleCellsToMoveTo())
            {
                gCost[possibleCell] = 1;
                prev[possibleCell] = player.CurrentPosition;
                open.Add(possibleCell);
                openSorted.Add(possibleCell);
            }

            finishCell = null;
            searchIsCompleted = false;
        }

        public void FindShortestPath()
        {
            if (searchIsCompleted)
                return;

            while (openSorted.Count != 0)
            {
                var current = openSorted.Min;
                open.Remove(current);
                openSorted.Remove(current);
                closed.Add(current);

                if (current.Y == player.TargetY)
                {
                    finishCell = current;
                    searchIsCompleted = true;
                    return;
                }

                var newDistance = gCost[current] + 1;

                foreach (var neighbor in game.Neighbors[current])
                {
                    if (closed.Contains(neighbor))
                        continue;

                    if (!open.Contains(neighbor))
                    {
                        gCost[neighbor] = newDistance;
                        prev[neighbor] = current;
                        open.Add(neighbor);
                        openSorted.Add(neighbor);
                    }
                    else
                    {
                        if (newDistance < gCost[neighbor])
                        {
                            openSorted.Remove(neighbor);
                            gCost[neighbor] = newDistance;
                            prev[neighbor] = current;
                            openSorted.Add(neighbor);
                        }
                    }

                    
                }
            }

            searchIsCompleted = true;
        }
    }
}