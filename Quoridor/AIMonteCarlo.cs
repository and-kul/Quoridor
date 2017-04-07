using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace Quoridor
{
    public class AIMonteCarlo : Player
    {
        private static readonly double ExplorationCoefficient = Math.Sqrt(2);
        private readonly int maxIterations;
        

        public AIMonteCarlo(string name, int maxIterations) : base(name)
        {
            this.maxIterations = maxIterations;
            allNodes = new List<Node>();
        }

        private class Node
        {
            public Node Parent;
            public Move BaseMove;
            public Player CurrentPlayer;
            public int Wins;
            public int Simulations;

            public Node(Node parent, Move baseMove, Player currentPlayer)
            {
                Parent = parent;
                BaseMove = baseMove;
                CurrentPlayer = currentPlayer;
                Wins = 0;
                Simulations = 0;
            }

            public List<Move> GetGeneratingMoveSequence()
            {
                var result = new List<Move>();
                var currentNode = this;
                while (currentNode.BaseMove != null)
                {
                    result.Add(currentNode.BaseMove);
                    currentNode = currentNode.Parent;
                }

                result.Reverse();
                return result;
            }

            public Stack<Move> OpenMoves;
        }

        private List<Node> allNodes;
        private Node rootNode;

        /// <summary>
        /// Upper Confidence Bound for a node
        /// </summary>
        private double UCB(Node node)
        {
            return (double) node.Wins / node.Simulations +
                   ExplorationCoefficient * Math.Sqrt(Math.Log(allNodes.Count) / node.Simulations);
        }

        private Node SelectNode()
        {
            if (allNodes.Count == 1)
                return allNodes[0];

            double highestUCB = 0;
            Node nodeWithHighestUCB = null;

            foreach (var node in allNodes)
            {
                var nodeUCB = UCB(node);
                if (nodeUCB > highestUCB)
                {
                    highestUCB = nodeUCB;
                    nodeWithHighestUCB = node;
                }
            }

            return nodeWithHighestUCB;
        }

        /// <summary>
        /// Expands a node, creating one child
        /// </summary>
        /// <param name="baseNode">Node to expand</param>
        /// <returns>New child</returns>
        private Node ExpandNode(Node baseNode)
        {
            var moveToNewNode = baseNode.OpenMoves.Pop();
            var newNode = new Node(baseNode, moveToNewNode, baseNode.CurrentPlayer.Opponent);
            var generatingSequence = newNode.GetGeneratingMoveSequence();
            foreach (var move in generatingSequence)
            {
                move.Apply();
            }

            var possibleMovesFromNewNode = newNode.CurrentPlayer.GetAllPossibleMoves();
            possibleMovesFromNewNode.Shuffle();
            newNode.OpenMoves = new Stack<Move>(possibleMovesFromNewNode);
            

            for (int i = generatingSequence.Count - 1; i >= 0; i--)
            {
                generatingSequence[i].Rollback();
            }

            allNodes.Add(newNode);

            return newNode;
        }

        private void SimulateAndBackpropagate(Node startNode)
        {
            var startNodeGeneratingSequence = startNode.GetGeneratingMoveSequence();

            foreach (var move in startNodeGeneratingSequence)
            {
                move.Apply();
            }

            var currentPlayer = startNode.CurrentPlayer;

            var simpleMoveSequence = new List<Move>();
            Player winner = null;

            while (true)
            {
                var simpleMove = new AIMinMax(currentPlayer, 1).CreateMove();
                simpleMove.Player = currentPlayer;
                simpleMoveSequence.Add(simpleMove);
                simpleMove.Apply();

                //Console.WriteLine(Game);
                //Console.ReadLine();
                
                if (currentPlayer.IsWinner)
                {
                    winner = currentPlayer;
                    //Console.WriteLine("*");
                    break;
                }

                currentPlayer = currentPlayer.Opponent;
            }

            for (int i = simpleMoveSequence.Count - 1; i >= 0; i--)
            {
                simpleMoveSequence[i].Rollback();
            }

            for (int i = startNodeGeneratingSequence.Count - 1; i >= 0; i--)
            {
                startNodeGeneratingSequence[i].Rollback();
            }

            int addToWins = (winner == this) ? 1 : 0;

            var currentNode = startNode;
            while (currentNode != null)
            {
                currentNode.Wins += addToWins;
                currentNode.Simulations++;

                currentNode = currentNode.Parent;
            }

        }


        


        public override Move CreateMove()
        {
            allNodes.Clear();
            rootNode = new Node(null, null, this);
            allNodes.Add(rootNode);
            var possibleMovesFromRoot = GetAllPossibleMoves();
            possibleMovesFromRoot.Shuffle();
            rootNode.OpenMoves = new Stack<Move>(possibleMovesFromRoot);


            for (int i = 0; i < maxIterations; ++i)
            {
                var nodeToExpand = SelectNode();
                var newNode = ExpandNode(nodeToExpand);
                SimulateAndBackpropagate(newNode);
            }

            return allNodes.Where(node => node.Parent == rootNode)
                .OrderByDescending(node => (double)node.Wins / node.Simulations)
                .First().BaseMove;
            
        }
    }
}