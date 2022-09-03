using Xadrez.BoardLayer;

namespace Xadrez.ChessLayer
{
    internal class ChessMatch
    {
        public Board Board { get; private set; }
        public int Shift { get; private set; }
        public Color CurrentPlayer { get; private set; }
        public bool Finished { get; private set; }
        private HashSet<Piece> _pieces;
        private HashSet<Piece> _captured;
        public bool Check { get; private set; }

        public ChessMatch()
        {
            Board = new Board(8, 8);
            Shift = 1;
            CurrentPlayer = Color.White;
            Finished = false;
            Check = false;
            _pieces = new HashSet<Piece>();
            _captured = new HashSet<Piece>();
            PutPieces();
        }

        public Piece PerformMovement(Position origin, Position destination)
        {
            Piece p = Board.RemovePiece(origin);
            p.IncrementMovementQuantity();
            Piece capturedPiece = Board.RemovePiece(destination);
            Board.PutPiece(p, destination);

            if (capturedPiece != null)
            {
                _captured.Add(capturedPiece);
            }
            return capturedPiece;
        }

        public void UndoMovement(Position origin, Position destination, Piece capturedPiece)
        {
            Piece p = Board.RemovePiece(destination);
            p.DecrementMovementQuantity();
            if (capturedPiece != null)
            {
                Board.PutPiece(capturedPiece, destination);
                _captured.Remove(capturedPiece);
            }
            Board.PutPiece(p, origin);
        }

        public void MakesMove(Position origin, Position destination)
        {
            Piece capturedPiece = PerformMovement(origin, destination);

            if (IsInCheck(CurrentPlayer))
            {
                UndoMovement(origin, destination, capturedPiece);
                throw new BoardException("Você não pode se colocar em xeque!");
            }
            if (IsInCheck(Oponent(CurrentPlayer)))
            {
                Check = true;
            }
            else
            {
                Check = false;
            }
            Shift++;
            ChangePlayer();
        }

        public void ValidateOriginPosition(Position position)
        {
            if (Board.Piece(position) == null)
            {
                throw new BoardException("Não existe peça na posição de origem escolhida!");
            }
            if (CurrentPlayer != Board.Piece(position).Color)
            {
                throw new BoardException("A peça de origem escolhida não é sua!");
            }
            if (!Board.Piece(position).HasPossibleMovements())
            {
                throw new BoardException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void ValidadeDestinationPosition(Position origin, Position destination)
        {
            if (!Board.Piece(origin).CanMoveTo(destination))
            {
                throw new BoardException("Posição de destino inválida!");
            }
        }

        public void ChangePlayer()
        {
            if (CurrentPlayer == Color.White)
            {
                CurrentPlayer = Color.Blue;
            }
            else
            {
                CurrentPlayer = Color.White;
            }
        }

        public HashSet<Piece> CapturedPieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();

            foreach (Piece p in _captured)
            {
                if (p.Color == color)
                {
                    aux.Add(p);
                }
            }
            return aux;
        }

        public HashSet<Piece> PiecesInGame(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();

            foreach (Piece p in _pieces)
            {
                if (p.Color == color)
                {
                    aux.Add(p);
                }
            }
            aux.ExceptWith(CapturedPieces(color));
            return aux;
        }

        private Color Oponent(Color color)
        {
            if (color == Color.White)
            {
                return Color.Blue;
            }
            else
            {
                return Color.White;
            }
        }

        private Piece King(Color color)
        {
            foreach (Piece p in PiecesInGame(color))
            {
                if (p is King)
                {
                    return p;
                }
            }
            return null;
        }

        public bool IsInCheck(Color color)
        {
            Piece k = King(color);

            if (k == null)
            {
                throw new BoardException("Não tem rei da cor " + color + " no tabuleiro!");
            }
            foreach (Piece p in PiecesInGame(Oponent(color)))
            {
                bool[,] mat = p.PossibleMovements();
                if (mat[k.Position.Row, k.Position.Column])
                {
                    return true;
                }
            }
            return false;
        }

        public void PutNewPiece(char column, int row, Piece piece)
        {
            Board.PutPiece(piece, new ChessPosition(column, row).ToPosition());
            _pieces.Add(piece);
        }

        public void PutPieces()
        {
            PutNewPiece('c', 1, new Tower(Board, Color.White));
            PutNewPiece('c', 2, new Tower(Board, Color.White));
            PutNewPiece('e', 1, new Tower(Board, Color.White));
            PutNewPiece('e', 2, new Tower(Board, Color.White));
            PutNewPiece('d', 1, new King(Board, Color.White));
            PutNewPiece('d', 2, new Tower(Board, Color.White));

            PutNewPiece('c', 7, new Tower(Board, Color.Blue));
            PutNewPiece('c', 8, new Tower(Board, Color.Blue));
            PutNewPiece('d', 7, new Tower(Board, Color.Blue));
            PutNewPiece('d', 8, new King(Board, Color.Blue));
            PutNewPiece('e', 7, new Tower(Board, Color.Blue));
            PutNewPiece('e', 8, new Tower(Board, Color.Blue));
        }
    }
}
