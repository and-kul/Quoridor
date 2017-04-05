using System;

namespace Quoridor
{
    public class Human : Player
    {
        public Human(string name) : base(name)
        {
        }

        public override Move CreateMove()
        {
            Console.WriteLine(Game);

            while (true)
            {
                Console.Write($"Enter move for {Name}: ");

                var moveDescription = Console.ReadLine();

                if (string.IsNullOrEmpty(moveDescription))
                {
                    continue;
                }

                if (moveDescription[0] == 'm')
                {
                    if (!Cell.IsCorrectCellDescription(moveDescription.Substring(2, 2)))
                    {
                        Console.WriteLine("Cell description must be in the format \"xy\"");
                        continue;
                    }

                    var cellTo = new Cell(moveDescription.Substring(2, 2));
                    if (GetPossibleCellsToMoveTo().Contains(cellTo))
                    {
                        return new PawnMove(Game, this, CurrentPosition, cellTo);
                    }
                    else
                    {
                        Console.WriteLine("You cannot move to this cell from the current position");
                        continue;
                    }
                }
                else if (moveDescription[0] == 'w') {
                    if (!Wall.IsCorrectWallDescription(moveDescription.Substring(2, 3)))
                    {
                        Console.WriteLine("Wall description must be in the format \"xyO\"");
                        continue;
                    }

                    var wall = new Wall(moveDescription.Substring(2, 3));

                    if (GetPossibleWallsToPlace().Contains(wall))
                    {
                        return new PlaceWallMove(Game, this, wall, Game.GetWallsRejectedBy(wall));
                    }
                    else
                    {
                        Console.WriteLine("You cannot place this wall");
                        continue;
                    }

                }
            }
            

        }
    }
}