using System.Collections.Generic;
using System.Text;

namespace Quoridor
{
    public class Game
    {
        public Cell[,] Board;
        public HashSet<Wall> wallsOnBoard;
        private HashSet<Wall> possibleWalls;
        private Player[] players;
        public Dictionary<Cell, List<Cell>> Neighbors;

        
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

            Neighbors = new Dictionary<Cell, List<Cell>>();

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
                    Neighbors[current] = new List<Cell>();

                    for (var i = 0; i < 4; ++i)
                    {
                        var adjacent = Board[x + dx[i], y + dy[i]];
                        if (adjacent != null)
                        {
                            Neighbors[current].Add(adjacent);
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
                player.Game = this;
            }

            wallsOnBoard = new HashSet<Wall>();
            possibleWalls = GenerateAllPossibleWalls();
            
        }


        public override string ToString()
        {
            var table = new char[19, 19];
            for (int i = 0; i < 19; ++i)
                for (int j = 0; j < 19; ++j)
                    table[i, j] = ' ';
            
            // Horizontal numbers
            for (int j = 1; j <= 17; j += 2)
                table[0, j] = ((j + 1) / 2).ToString()[0];
            for (int j = 1; j <= 17; j += 2)
                table[18, j] = ((j + 1) / 2).ToString()[0];

            // Vertical numbers
            for (int i = 1; i <= 17; i += 2)
                table[i, 0] = (10 - (i + 1) / 2).ToString()[0];
            for (int i = 1; i <= 17; i += 2)
                table[i, 18] = (10 - (i + 1) / 2).ToString()[0];

            // Dots for empty cells
            for (int i = 1; i <= 17; i += 2)
                for (int j = 1; j <= 17; j += 2)
                    table[i, j] = '.';

            // Helpful functions
            int Get_I_From_Y(int y) => 19 - 2 * y;
            int Get_J_From_X(int x) => 2 * x - 1;
            

            // '0' for the first player and '1' for the second
            foreach (var player in players)
            {
                table[Get_I_From_Y(player.CurrentPosition.Y), Get_J_From_X(player.CurrentPosition.X)] = player.Id.ToString()[0];
            }

            foreach (var wall in wallsOnBoard)
            {
                var upperLeftCell = wall.SegmentB.Cell1;
                if (wall.Orientation == Orientation.Vertical)
                {
                    table[Get_I_From_Y(upperLeftCell.Y), Get_J_From_X(upperLeftCell.X) + 1] = 'W';
                    table[Get_I_From_Y(upperLeftCell.Y) + 1, Get_J_From_X(upperLeftCell.X) + 1] = 'W';
                    table[Get_I_From_Y(upperLeftCell.Y) + 2, Get_J_From_X(upperLeftCell.X) + 1] = 'W';
                }
                else
                {
                    table[Get_I_From_Y(upperLeftCell.Y) + 1, Get_J_From_X(upperLeftCell.X)] = 'W';
                    table[Get_I_From_Y(upperLeftCell.Y) + 1, Get_J_From_X(upperLeftCell.X) + 1] = 'W';
                    table[Get_I_From_Y(upperLeftCell.Y) + 1, Get_J_From_X(upperLeftCell.X) + 2] = 'W';
                }
            }



            

            var builder = new StringBuilder();

            for (int i = 0; i < 19; ++i)
            {
                for (int j = 0; j < 19; ++j)
                    builder.Append(table[i, j]);
                builder.AppendLine();
            }

            return builder.ToString();
            
        }
    }
}