using System;
using System.Linq;
using NUnit.Framework;
using Quoridor;

namespace Tests
{
    [TestFixture]
    public class PossiblePlaceWallMovesTests
    {
        private Cell[,] c;

        private Player player1;
        private Player player2;
        private Game game;


        [SetUp]
        public void SetUp()
        {
            c = new Cell[10, 10];
            for (var x = 1; x <= 9; ++x)
                for (var y = 1; y <= 9; ++y)
                    c[x, y] = new Cell(x, y);

            player1 = new Human("player1");
            player2 = new Human("player2");

            game = new Game(player1, player2);
        }

        [Test]
        public void Test1()
        {
            player1.CurrentPosition = c[5, 7];
            player2.CurrentPosition = c[5, 5];

            var wallDescriptions = new[] {"13h", "33h", "53h", "63v", "62h"};

            foreach (var wallDescription in wallDescriptions)
            {
                var wall = new Wall(wallDescription);
                var rejectedWalls = game.GetWallsRejectedBy(wall);

                var placeWallMove = new PlaceWallMove(game, player1, wall, rejectedWalls);
                placeWallMove.Apply();
            }

            Console.WriteLine(game);

            Assert.That(player1.GetPossiblePlaceWallMoves()
                .Select(move => move.PlacedWall)
                .Contains(new Wall("82h")), Is.False);

            Assert.That(player1.GetPossiblePlaceWallMoves()
                .Select(move => move.PlacedWall)
                .Contains(new Wall("82v")), Is.True);
        }
    }
}