
using Tic_Tac_Toe;

namespace Tic_tac_toe_test;

public class Tests
{
    private MonteCarloTreeSearch search;

    [SetUp]
    public void Setup()
    {
        search = new MonteCarloTreeSearch();
    }


    [Test]
    public void GivenEmptyBoard_WhenSimulateInterAIPlay_thenGameDraw() {
        var board = new Board();

        var player = Board.P2;

        var totalMoves = Board.ROWS * Board.COLUMNS;

        for (int i = 0; i < totalMoves; i++) {
            board = search.FindNextMove(board, player);

            if (board.CheckStatus() != -1) {
                break;
            }

            player = 3 - player;
        }

        var winStatus = board.CheckStatus();

        Assert.That(winStatus, Is.EqualTo(Board.DRAW));
    }
}