// See https://aka.ms/new-console-template for more information
using Tic_Tac_Toe;

var board = new Board();
var mcts = new MonteCarloTreeSearch();
var player = Board.P2;

Console.WriteLine("Hello, Welcome to the TIC TAC TOE super duper cool and fun game!");
Console.WriteLine("Computer player mark is: 1");
Console.WriteLine("Human player mark is: 2");
Console.WriteLine();

board.PrintBoard();

while (board.GetEmptyPositions().Count > 0) {
    int selection = 0;
    int row = 0;
    int col = 0;
    bool isValidInput;

    do {
        selection = GetPlayerInput();
        isValidInput = IsValidInput(selection);

        if (isValidInput) {
            var playerInput = selection - 1;
            row = playerInput / Board.ROWS;
            col = playerInput % Board.COLUMNS;

            if (!board.IsEmptyPosition(row, col)) {
                Console.WriteLine("This cell is already taken, select another");
                isValidInput = false;
            }
        } else {
            Console.WriteLine("Invalid input. Please select a number from 1 to 9.");
        }
    } while (!isValidInput);

    board.PerformMove(player, new Position(row, col));
    board.PrintBoard();
    board.PrintStatus();

    Console.WriteLine();

    if (board.CheckStatus() != -1) {
        break;
    }

    var playerNum = 3 - player;
    board = mcts.FindNextMove(board, playerNum);
    board.PrintBoard();

    if (board.CheckStatus() != -1) {
        break;
    }

    board.PrintStatus();
    Console.WriteLine();
}



int GetPlayerInput() {

    var selection = 0;

    Console.WriteLine("Player, to make a move, select a number from 1 to 9");
    var input = Console.ReadLine();

    if (int.TryParse(input, out selection)) {
        Console.WriteLine($"selected number {selection}");
    }

    return selection;
}

bool IsValidInput(int input) {
    return input >= 1 && input <= 9;
}


Console.ReadLine();