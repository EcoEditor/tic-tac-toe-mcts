namespace Tic_Tac_Toe
{
	/// <summary>
	///  The state is an object that keeps track of the board, the current player,
	///  the number of times the node has been visited, and the win score of the node.
	/// </summary>
	public class GameState {
		private Board board;
		private int playerNo;
		private int winScore;
		private int visitCount;

		public GameState() {
			board = new Board();
		}

		public GameState(GameState state) {
			this.board = new Board(state.board);
			this.playerNo = state.playerNo;
			this.visitCount = state.visitCount;
			this.winScore = state.winScore;
		}

		public GameState(Board board) {
			this.board = board;
		}

		public GameState(Board board, int playerNo) {
			this.board = board;
			this.playerNo = playerNo;
		}

		public List<GameState> GetAllPossibleStates() {
			var possibleStates = new List<GameState>();
			var availablePositions = this.board.GetEmptyPositions();

			for (int i = 0; i < availablePositions.Count; i++) {
				var newBoard = new Board(this.board);
				var newState = new GameState(newBoard);
				newState.PlayerNo = 3 - this.playerNo;
				newState.Board.PerformMove(newState.PlayerNo, availablePositions[i]);
				newState.Board.PrintBoard();
                Console.WriteLine();
                possibleStates.Add(newState);
			}

			return possibleStates;
		}

		public int GetOpponent() {
			return 3 - playerNo;
		}

		public void TogglePlayer() {
			this.playerNo = 3 - this.playerNo;
		}

		public void RandomPlay() {
			var availablePositions = board.GetEmptyPositions();
			var random = new Random();
			var randomIndex = random.Next(availablePositions.Count);
			board.PerformMove(playerNo, availablePositions[randomIndex]);
		}

		public void AddScore(int score) {
			if (this.winScore != int.MinValue) {
				winScore += score;
			}
		}

		public Board Board {
			get {
				return board;
			}
			set {
				board = value;
			}
		}

		public int PlayerNo {
			get {
				return playerNo;
			}
			set {
				playerNo = value;
			}
		}

		public int VisitCount {
			get {
				return visitCount;
			}
			set {
				visitCount = value;
			}
		}

		public int WinScore {
			get {
				return winScore;
			}
			set {
				winScore = value;
			}
		}
	}
}