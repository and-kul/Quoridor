using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Quoridor;

namespace Tests
{
    [TestFixture]
    class AStarTests
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
        public void InitialPosition()
        {
            Assert.That(player1.GetShortestPathLength(), Is.EqualTo(8));
        }

        [Test]
        public void PathDoesNotExist()
        {
            var wallDescriptions = new [] {"12h", "32h", "52h", "72h", "81v"};

            foreach (var wallDescription in wallDescriptions)
            {
                var wall = new Wall(wallDescription);
                var rejectedWalls = game.GetWallsRejectedBy(wall);

                var placeWallMove = new PlaceWallMove(game, player1, wall, rejectedWalls);
                placeWallMove.Apply();
            }
            
            Console.WriteLine(game);

            Assert.That(player1.GetShortestPathLength(), Is.EqualTo(int.MaxValue));
        }

        [Test]
        public void WithJumpAtTheStart()
        {
            player1.CurrentPosition = c[5, 5];
            player2.CurrentPosition = c[5, 6];

            var wallBehind = new Wall("56h");
            var rejectedWallsByWallBehind = game.GetWallsRejectedBy(wallBehind);
            //Assert.That(rejectedWallsByWallBehind.Length == 4);

            var placeWallBehindMove = new PlaceWallMove(game, player1, wallBehind, rejectedWallsByWallBehind);
            placeWallBehindMove.Apply();

            var wallOnTheRight = new Wall("55v");
            var rejectedWallsByWallOnTheRight = game.GetWallsRejectedBy(wallOnTheRight);
            //Assert.That(rejectedWallsByWallOnTheRight.Length == 3);

            var placeWallOnTheRightMove = new PlaceWallMove(game, player1, wallOnTheRight, rejectedWallsByWallOnTheRight);
            placeWallOnTheRightMove.Apply();

            Console.WriteLine(game);

            Assert.That(player1.GetShortestPathLength(), Is.EqualTo(4));
            Assert.That(player2.GetShortestPathLength(), Is.EqualTo(4));
        }

        [Test]
        public void TrickyPath()
        {
            player1.CurrentPosition = c[5, 5];

            var wallDescriptions = new[] { "55h", "44v", "65v", "77h", "46h" };

            foreach (var wallDescription in wallDescriptions)
            {
                var wall = new Wall(wallDescription);
                var rejectedWalls = game.GetWallsRejectedBy(wall);

                var placeWallMove = new PlaceWallMove(game, player1, wall, rejectedWalls);
                placeWallMove.Apply();
            }

            Console.WriteLine(game);

            Assert.That(player1.GetShortestPathLength(), Is.EqualTo(9));
        }

        [Test]
        public void UsingAStarSeveralTimesForOnePlayer()
        {
            player1.CurrentPosition = c[5, 5];

            var wallDescriptions = new[] { "55h", "44v", "65v", "77h", "46h" };

            foreach (var wallDescription in wallDescriptions)
            {
                var wall = new Wall(wallDescription);
                var rejectedWalls = game.GetWallsRejectedBy(wall);

                var placeWallMove = new PlaceWallMove(game, player1, wall, rejectedWalls);
                placeWallMove.Apply();
            }

            Console.WriteLine(game);

            Assert.That(player1.GetShortestPathLength(), Is.EqualTo(9));

            player1.CurrentPosition = c[5, 3];

            Console.WriteLine(game);
            Assert.That(player1.GetShortestPathLength(), Is.EqualTo(8));


        }

    }
}
