using Xadrez;
using Xadrez.BoardLayer;
using Xadrez.ChessLayer;

try
{
    ChessMatch match = new ChessMatch();

    while (!match.Finished)
    {
        Console.Clear();
        Screen.PrintBoard(match.Board);

        Console.WriteLine();

        Console.Write("Origem: ");
        Position origin = Screen.ReadChessPosition().ToPosition();

        Console.Write("Destino: ");
        Position destination = Screen.ReadChessPosition().ToPosition();

        match.PerformMovement(origin, destination);
    }
}
catch (BoardException e)
{
    Console.WriteLine(e.Message);
}