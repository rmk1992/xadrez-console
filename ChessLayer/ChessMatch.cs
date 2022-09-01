using Xadrez.BoardLayer;

namespace Xadrez.ChessLayer
{
    internal class ChessMatch
    {
        public Board Board { get; private set; }
        private int _shift;
        private Color _currentPlayer;
        public bool Finished { get; private set; }

        public ChessMatch()
        {
            Board = new Board(8, 8);
            _shift = 1;
            _currentPlayer = Color.White;
            Finished = false;
            PutPieces();
        }

        public void PerformMovement(Position origin, Position destination)
        {
            Piece p = Board.RemovePiece(origin);
            p.IncrementMovementQuantity();
            Piece capturedPiece = Board.RemovePiece(destination);
            Board.PutPiece(p, destination);
        }

        public void PutPieces()
        {
            Board.PutPiece(new Tower(Board, Color.White), new ChessPosition('c', 1).ToPosition());
            Board.PutPiece(new Tower(Board, Color.White), new ChessPosition('c', 2).ToPosition());
            Board.PutPiece(new Tower(Board, Color.White), new ChessPosition('e', 1).ToPosition());
            Board.PutPiece(new Tower(Board, Color.White), new ChessPosition('e', 2).ToPosition());
            Board.PutPiece(new Tower(Board, Color.White), new ChessPosition('d', 2).ToPosition());
            Board.PutPiece(new King(Board, Color.White), new ChessPosition('d', 1).ToPosition());

            Board.PutPiece(new Tower(Board, Color.Blue), new ChessPosition('c', 8).ToPosition());
            Board.PutPiece(new Tower(Board, Color.Blue), new ChessPosition('c', 7).ToPosition());
            Board.PutPiece(new Tower(Board, Color.Blue), new ChessPosition('e', 8).ToPosition());
            Board.PutPiece(new Tower(Board, Color.Blue), new ChessPosition('e', 7).ToPosition());
            Board.PutPiece(new Tower(Board, Color.Blue), new ChessPosition('d', 7).ToPosition());
            Board.PutPiece(new King(Board, Color.Blue), new ChessPosition('d', 8).ToPosition());
        }
    }
}
