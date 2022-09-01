using Xadrez;
using Xadrez.BoardLayer;
using Xadrez.ChessLayer;

try
{
    ChessMatch match = new ChessMatch();

    while (!match.Finished)
    {
        try
        {
            Console.Clear();
            Screen.PrintMatch(match);

            Console.Write("Origem: ");
            Position origin = Screen.ReadChessPosition().ToPosition();
            match.ValidateOriginPosition(origin);

            bool[,] possiblePositions = match.Board.Piece(origin).PossibleMovements();

            Console.Clear();
            Screen.PrintBoard(match.Board, possiblePositions);

            Console.WriteLine();

            Console.Write("Destino: ");
            Position destination = Screen.ReadChessPosition().ToPosition();
            match.ValidadeDestinationPosition(origin, destination);

            match.MakesMove(origin, destination);
        }
        catch (BoardException e)
        {
            Console.WriteLine(e.Message);
            Console.ReadLine();
        }
    }
}
catch (BoardException e)
{
    Console.WriteLine(e.Message);
}