using Xadrez;
using Xadrez.BoardLayer;
using Xadrez.ChessLayer;

try
{
    Board board = new Board(8, 8);

    board.PutPiece(new King(board, Color.White), new Position(2, 4));
    board.PutPiece(new Tower(board, Color.Blue), new Position(5, 2));
    board.PutPiece(new Tower(board, Color.White), new Position(6, 4));

    Screen.PrintBoard(board);
}
catch (BoardException e)
{
    Console.WriteLine(e.Message);
}