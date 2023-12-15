namespace Tic_Tac_Toe
{
    /// <summary>
    /// The board is an object that keeps track of the size of the board, what pieces have been played and where and what values the status of the game means. 
    /// </summary>
    public class Board
	{
		public const int IN_PROGRESS = -1;
		public const int DRAW = 0;
		public const int P1 = 1;
		public const int P2 = 2;
		public const int ROWS = 3;
		public const int COLUMNS = 3;

		private int[,] boardValues;

		public Board() {
			boardValues = new int[ROWS, COLUMNS]; 
		}

		public Board(int boardSize) {
			boardValues = new int[boardSize, boardSize];
		}

		public Board(int[,] boardValues) {
			this.boardValues = boardValues;
		}

		public Board(Board board) {
			var boardLength = board.BoardValues.GetLength(0);
			this.boardValues = new int[boardLength, boardLength];
			int[,] boardValues = board.BoardValues;
			int n = boardValues.GetLength(0);

			for (int i = 0; i < n; i++) {
				int m = boardValues.GetLength(1);
				for (int j = 0; j < m; j++) {
					this.boardValues[i, j] = boardValues[i, j];
				}
			}
		}

		public void PerformMove(int player, Position p) {
			boardValues[p.X, p.Y] = player;
		}

		public bool IsEmptyPosition(int row, int col) {
			return boardValues[row, col] == 0;
		}

        /// <summary>
        // Status, return:
        // -1 if the game is still in progress,
        // 0 if it results in a tie,
        // 1 and 2 depending on which player won the game
        /// </summary>
        /// <returns></returns>
        public int CheckStatus() {
			var boardSize = boardValues.GetLength(0);
            var maxIndex = boardSize - 1;
			int[] diag1 = new int[boardSize];
			int[] diag2 = new int[boardSize];

		// VOVA: the indentation doesn't look good in github
			for (int i = 0; i < boardSize; i++) {
                int[] row = new int[boardSize];
                for (int j = 0; j < boardSize; j++) {
                    row[j] = boardValues[i, j];
                }

                int[] col = new int[boardSize];
                for (int j = 0; j < boardSize; j++) {
                    col[j] = boardValues[j, i];
                }

                int checkRowForWin = CheckForWin(row);
                if (checkRowForWin != 0)
                    return checkRowForWin;

                int checkColForWin = CheckForWin(col);
                if (checkColForWin != 0)
                    return checkColForWin;

                diag1[i] = boardValues[i, i];
                diag2[i] = boardValues[maxIndex - i, i];
            }

            int checkDiag1ForWin = CheckForWin(diag1);
            if (checkDiag1ForWin != 0)
                return checkDiag1ForWin;

            int checkDiag2ForWin = CheckForWin(diag2);
            if (checkDiag2ForWin != 0)
                return checkDiag2ForWin;

			if(GetEmptyPositions().Count > 0) {
				return IN_PROGRESS;
			} else {
				return DRAW;
			}
		}

		private int CheckForWin(int[] row) {
			var isEqual = true;
			var size = row.Length;
			var previous = row[0];
			for (int i = 0; i < size; i++) {
				if (previous != row[i]) {
					isEqual = false;
					break;
				}

				previous = row[i];
			}

			if (isEqual) {
				return previous;
			} else {
				return 0;
			}
		}

		public void PrintBoard() {
			for (int i = 0; i < boardValues.GetLength(0); i++) {
				for (int j = 0; j < boardValues.GetLength(1); j++) {
					Console.Write(boardValues[i, j] + " ");
				}

				Console.WriteLine();
			}
		}

		public List<Position> GetEmptyPositions() {
			var emptyPositions = new List<Position>();
			for (int i = 0; i < boardValues.GetLength(0); i++) {
				for (int j = 0; j < boardValues.GetLength(1); j++) {
					if (boardValues[i, j] == 0) {
						emptyPositions.Add(new Position(i, j));
					}
				}
			}

			return emptyPositions;
		}

		public void PrintStatus() {
			switch (CheckStatus()) {
				case P1:
					Console.WriteLine("Player 1 wins");
					break;
				case P2:
					Console.WriteLine("Player 2 wins");
					break;
				case DRAW:
					Console.WriteLine("Game Draw");
					break;
				case IN_PROGRESS:
					Console.WriteLine("Game in progress");
					break;
			}
		}

		public int[,] BoardValues {
			get {
				return boardValues;
			}
			set {
				boardValues = value;
			}
		}
	}
}
