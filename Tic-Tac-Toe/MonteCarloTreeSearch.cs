using System.Diagnostics;

namespace Tic_Tac_Toe
{
    // http://tim.hibal.org/blog/alpha-zero-how-and-why-it-works/
    // https://github.com/hayoung-kim/mcts-tic-tac-toe/blob/master/VanilaMCTS.py
    // https://medium.com/swlh/tic-tac-toe-at-the-monte-carlo-a5e0394c7bc2
    /// <summary>
    /// The Monte Carlo Tree Search - 4 phases:
    /// Selection - SelectPromisingNode(...)
	/// Expansion - ExpandNode(...)
	/// Simulation - SimulateRunPlayout(...)
	/// Backpropogation - Backpropagation(..)
    /// </summary>
    public class MonteCarloTreeSearch
	{
		private const int WIN_SCORE = 10;
		private float searchTime = 1000f;

        

        public MonteCarloTreeSearch()
		{

		}

		public Board FindNextMove(Board board, int playerNo) {
			var opponent = 3 - playerNo;
			var tree = new MonteCarloTree();
			var rootNode = tree.Root;
			rootNode.State.Board = board;
			rootNode.State.PlayerNo = opponent;

			var stopWatch = new Stopwatch();
			stopWatch.Start();

            while (stopWatch.ElapsedMilliseconds < searchTime) {
				var promisingNode = SelectPromisingNode(rootNode);

				if(promisingNode.State.Board.CheckStatus() == Board.IN_PROGRESS) {
					ExpandNode(promisingNode);
				}

				var nodeToExplore = promisingNode;
				if (nodeToExplore.Children.Count > 0) {
					nodeToExplore = promisingNode.GetRandomChild();
				}

				// returns the index of a winning player
				var playoutResult = SimulateRunPlayout(nodeToExplore, opponent);
				Backpropagation(nodeToExplore, playoutResult);
			}

			stopWatch.Stop();
			var winnerNode = rootNode.GetChildWithMaxScore();
			return winnerNode.State.Board;
		}

        /// <summary>
        /// Traverse the tree starting from the root node and find the most promising leaf node below it.
		/// This is achieved by looking at the UCB values of each child node and selecting the node with the highest value.
		/// The process repeats until a node has no more child nodes.
        /// </summary>
        /// <param name="rootNode"></param>
        /// <returns></returns>
        private MonteCarloTreeNode SelectPromisingNode(MonteCarloTreeNode rootNode) {
			var node = rootNode;

			while(node.Children.Count != 0) {
				node = UCT.FindBestNodeWithUCT(node);

			}
			return node;
		}

		private void ExpandNode(MonteCarloTreeNode node) {
            // Get all possible outcomes from the current nodes state
            var possibleStates = node.State.GetAllPossibleStates();

            // Create a node with every possible move (from the current state) and add the new node into the children list of the current node
            foreach (var state in possibleStates) {
				var newNode = new MonteCarloTreeNode(state);
				newNode.Parent = node;
				newNode.State.PlayerNo = node.State.GetOpponent();
				node.AddChild(newNode);
			}
		}

		private int SimulateRunPlayout(MonteCarloTreeNode node, int opponent) {
			//var tempNode = new MonteCarloTreeNode(null, node);
            var tempNode = new MonteCarloTreeNode(node);
			var tempState = tempNode.State;
			var boardStatus = tempState.Board.CheckStatus();

            // If the node results in an opponent’s victory, it would mean that if the player had made the selected move,
			// his opponent will have a subsequent move that can result in the opponents immediete victory.
			// Because the move the player chose can lead to a definite loss, the function lowers the parents node’s
			// winScore to the lowest possible integer to prevent future moves to that node.
            if (boardStatus == opponent) {
				tempNode.Parent.State.WinScore = int.MinValue;
				return boardStatus;
			}

            // Otherwise the algorithm alternates random moves between the two player until the board results in a game ending state.
			// The function then returns the final game status.
            while (boardStatus == Board.IN_PROGRESS) {
				tempState.TogglePlayer();
				tempState.RandomPlay();
				boardStatus = tempState.Board.CheckStatus();
			}

            return boardStatus;
		}

		private void Backpropagation(MonteCarloTreeNode nodeToExplore, int playoutResult) {
			var tempNode = nodeToExplore;

			while(tempNode != null) {
				tempNode.State.VisitCount++;
				if (tempNode.State.PlayerNo == playoutResult) {
					tempNode.State.AddScore(WIN_SCORE);
				}
				tempNode = tempNode.Parent;
			}
		}
	}

    /// <summary>
	/// // UCT = upper confidence bound tree
    /// The UCB formula balances the exploration/exploitation problem by making sure that
	/// no potential node will be starved of simulated playouts but at the same time
	/// making sure that favorable branches are played more often than their counterparts.
	/// The UCB formula that we will be using is displayed below.
    /// </summary>
    public static class UCT {
        /// <summary>
        /// stands for the exploration parameter. We will be using √2 which is apprx. 1.41
        /// </summary>
        private const double c = 1.41;


        public static MonteCarloTreeNode FindBestNodeWithUCT(MonteCarloTreeNode node) {
			var parentVisitCount = node.State.VisitCount;
			var childUCT = new List<double>();
			foreach (var child in node.Children) {
				var ucbValue = GetUctValue(parentVisitCount, child.State.WinScore, child.State.VisitCount);
				childUCT.Add(ucbValue);
			}

			var maxValue = childUCT.Max();
			var index = childUCT.IndexOf(maxValue);
			return node.Children[index];
		}

		private static double GetUctValue(int totalVisit, int nodeWinScore, int nodeVisit) {
            // If the node has never been visited before, we give its value the maximum integer possible
			// to increase its chances of being selected. This makes sure that every node will be simulated at least once.
            if (nodeVisit == 0) {
				return int.MaxValue;
			}

            //The first part of the equation corresponds to exploitation.
			//The higher the number of wins = higher win ratio = higher number for the first part of the equation.
			//The second part of the equation corresponds to exploration.
			//A large number of total simulations but a small number of simulations for that node
			//will lead to a larger number for the second part of the equation.
			//Together, the two parts of the UCB function creates a balance between prioritizing
			//promising nodes but also exploring unvisited ones.
            var uctValue = (nodeWinScore / nodeVisit) + c * Math.Sqrt(Math.Log(totalVisit / nodeVisit));

			return uctValue;
		}
	}
}