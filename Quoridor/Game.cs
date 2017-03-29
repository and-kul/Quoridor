using System.Collections.Generic;

namespace Quoridor
{
    public class Game
    {
        public Cell[,] Board;
        private HashSet<Wall> wallsOnBoard;
        private HashSet<Wall> possibleWalls;
        private Player[] players;


        private HashSet<Wall> GenerateAllPossibleWalls()
        {
            var result = new HashSet<Wall>();

            var dx = new[] {0, 0, 1, 1};
            var dy = new[] {0, 1, 0, 1};

            for (var x = 1; x <= 8; ++x)
                for (var y = 1; y <= 8; ++y)
                {
                    var cells = new List<Cell>();
                    for (var i = 0; i < 4; ++i)
                        cells.Add(Board[x + dx[i], y + dy[i]]);

                    result.Add(new Wall(cells.ToArray(), Orientation.Vertical));
                    result.Add(new Wall(cells.ToArray(), Orientation.Horizontal));
                    
                }
            
            return result;

        }



        public Game(Player firstPlayer, Player secondPlayer)
        {
            Board = new Cell[11, 11];

            for (var x = 1; x <= 9; ++x)
                for (var y = 1; y <= 9; ++y)
                {
                    Board[x, y] = new Cell(x, y);
                }

            var dx = new[] {1, 0, -1, 0};
            var dy = new[] {0, 1, 0, -1};

            for (var x = 1; x <= 9; ++x)
                for (var y = 1; y <= 9; ++y)
                {
                    var current = Board[x, y];
                    for (var i = 0; i < 4; ++i)
                    {
                        var adjacent = Board[x + dx[i], y + dy[i]];
                        if (adjacent != null)
                        {
                            current.Neighbors.Add(adjacent);
                        }
                    }
                }

            
            players = new Player[2];

            players[0] = firstPlayer;
            players[1] = secondPlayer;

            firstPlayer.Id = 0;
            secondPlayer.Id = 1;

            firstPlayer.TargetY = 9;
            secondPlayer.TargetY = 1;
            
            foreach (var player in players)
            {
                player.CurrentPosition = player.Id == 0 ? Board[5, 1] : Board[5, 9];
                player.WallsRemaining = 10;
            }

            wallsOnBoard = new HashSet<Wall>();
            possibleWalls = GenerateAllPossibleWalls();
            

        }
    }
}