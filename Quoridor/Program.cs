using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor
{
    class Program
    {
        

        static void Main(string[] args)
        {
            var player1 = new Human("Andrey");
            var player2 = new AIAlphaBeta("AI_AlphaBeta", 3);

            var game = new Game(player1, player2);
            game.Play();

            
            Console.WriteLine($"The winner is {game.Winner.Name}");


            Console.ReadLine();
        }
    }
}
