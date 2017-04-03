using System;
using System.Linq;
using NUnit.Framework;
using Quoridor;

namespace Tests
{
    [TestFixture]
    public class PossiblePawnMovesTests
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
            var answerForPlayer1 = new[] {c[4, 1], c[5, 2], c[6, 1]};
            var answerForPlayer2 = new[] {c[4, 9], c[5, 8], c[6, 9]};
            
            Assert.That(player1.GetPossiblePawnMoves().Select(move => move.From).All(cell => cell == player1.CurrentPosition));
            Assert.That(player2.GetPossiblePawnMoves().Select(move => move.From).All(cell => cell == player2.CurrentPosition));

            Assert.That(player1.GetPossiblePawnMoves().Select(move => move.To), Is.EquivalentTo(answerForPlayer1));
            Assert.That(player2.GetPossiblePawnMoves().Select(move => move.To), Is.EquivalentTo(answerForPlayer2));
            
        }

        [Test]
        public void OpenSpace()
        {
            player1.CurrentPosition = c[5, 5];

            var answer = new[] { c[4, 5], c[5, 6], c[6, 5], c[5, 4] };

            Assert.That(player1.GetPossiblePawnMoves().Select(move => move.To), Is.EquivalentTo(answer));
        }



        [Test]
        public void InTheCorner()
        {
            player1.CurrentPosition = c[1, 1];
            
            var answer = new [] { c[2, 1], c[1, 2]};
            
            Assert.That(player1.GetPossiblePawnMoves().Select(move => move.To), Is.EquivalentTo(answer));
           
        }

        [Test]
        public void JumpOver()
        {
            player2.CurrentPosition = c[5, 2];
            
            var answer = new[] { c[4, 1], c[5, 3], c[6, 1] };

            Assert.That(player1.GetPossiblePawnMoves().Select(move => move.To), Is.EquivalentTo(answer));
        }

        [Test]
        public void ToBothSidesOfOpponent()
        {
            player1.CurrentPosition = c[5, 5];
            player2.CurrentPosition = c[5, 6];

            var wallBehind = new Wall("56h");
            var rejectedWalls = game.GetWallsRejectedBy(wallBehind);
            
            var placeWallBehindMove = new PlaceWallMove(game, player1, wallBehind, rejectedWalls);
            placeWallBehindMove.Apply();

            Console.WriteLine(game);
            
            var answer = new[] { c[4, 5], c[5, 4], c[6, 5], c[4, 6], c[6, 6]};

            Assert.That(player1.GetPossiblePawnMoves().Select(move => move.To), Is.EquivalentTo(answer));
        }

        [Test]
        public void ToLeftSideOfOpponent()
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

            var answer = new[] { c[4, 5], c[5, 4], c[4, 6]};

            Assert.That(player1.GetPossiblePawnMoves().Select(move => move.To), Is.EquivalentTo(answer));
        }


    }
}