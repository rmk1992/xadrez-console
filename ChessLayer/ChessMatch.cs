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
        public Piece VulnerableEnPassant { get; private set; }

        public ChessMatch()
        {
            Board = new Board(8, 8);
            Shift = 1;
            CurrentPlayer = Color.White;
            Finished = false;
            Check = false;
            VulnerableEnPassant = null;
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
            if (p is King && destination.Column == origin.Column + 2)
            {
                Position originT = new Position(origin.Row, origin.Column + 3);
                Position destinationT = new Position(origin.Row, origin.Column + 1);
                Piece t = Board.RemovePiece(originT);
                t.IncrementMovementQuantity();
                Board.PutPiece(t, destinationT);
            }
            if (p is King && destination.Column == origin.Column - 2)
            {
                Position originT = new Position(origin.Row, origin.Column - 4);
                Position destinationT = new Position(origin.Row, origin.Column - 1);
                Piece t = Board.RemovePiece(originT);
                t.IncrementMovementQuantity();
                Board.PutPiece(t, destinationT);
            }

            if (p is Pawn)
            {
                if (origin.Column != destination.Column && capturedPiece == null)
                {
                    Position pawnPosition;
                    if (p.Color == Color.White)
                    {
                        pawnPosition = new Position(destination.Row + 1, destination.Column);
                    }
                    else
                    {
                        pawnPosition = new Position(destination.Row - 1, destination.Column);
                    }
                    capturedPiece = Board.RemovePiece(pawnPosition);
                    _captured.Add(capturedPiece);
                }
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

            if (p is King && destination.Column == origin.Column + 2)
            {
                Position originT = new Position(origin.Row, origin.Column + 3);
                Position destinationT = new Position(origin.Row, origin.Column + 1);
                Piece t = Board.RemovePiece(destinationT);
                t.IncrementMovementQuantity();
                Board.PutPiece(t, originT);
            }
            if (p is King && destination.Column == origin.Column - 2)
            {
                Position originT = new Position(origin.Row, origin.Column - 4);
                Position destinationT = new Position(origin.Row, origin.Column - 1);
                Piece t = Board.RemovePiece(destinationT);
                t.IncrementMovementQuantity();
                Board.PutPiece(t, originT);
            }

            if (p is Pawn)
            {
                if (origin.Column != destination.Column && capturedPiece == VulnerableEnPassant)
                {
                    Piece pawn = Board.RemovePiece(destination);
                    Position pawnPosition;
                    if (p.Color == Color.White)
                    {
                        pawnPosition = new Position(3, destination.Column);
                    }
                    else
                    {
                        pawnPosition = new Position(4, destination.Column);
                    }
                    Board.PutPiece(pawn, pawnPosition);
                }
            }
        }

        public void MakesMove(Position origin, Position destination)
        {
            Piece capturedPiece = PerformMovement(origin, destination);

            if (IsInCheck(CurrentPlayer))
            {
                UndoMovement(origin, destination, capturedPiece);
                throw new BoardException("Você não pode se colocar em xeque!");
            }

            Piece p = Board.Piece(destination);

            if (p is Pawn)
            {
                if ((p.Color == Color.White && destination.Row == 0) || (p.Color == Color.Blue && destination.Row == 7))
                {
                    p = Board.RemovePiece(destination);
                    _pieces.Remove(p);
                    Piece queen = new Queen(Board, p.Color);
                    Board.PutPiece(queen, destination);
                    _pieces.Add(queen);
                }
            }

            if (IsInCheck(Oponent(CurrentPlayer)))
            {
                Check = true;
            }
            else
            {
                Check = false;
            }
            if (TestCheckmate(Oponent(CurrentPlayer)))
            {
                Finished = true;
            }
            else
            {
                Shift++;
                ChangePlayer();
            }

            if (p is Pawn && (destination.Row == origin.Row - 2 || destination.Row == origin.Row + 2))
            {
                VulnerableEnPassant = p;
            }
            else
            {
                VulnerableEnPassant = null;
            }
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

        public bool TestCheckmate(Color color)
        {
            if (!IsInCheck(color))
            {
                return false;
            }
            foreach (Piece p in PiecesInGame(color))
            {
                bool[,] mat = p.PossibleMovements();
                for (int i = 0; i < Board.Rows; i++)
                {
                    for (int j = 0; j < Board.Columns; j++)
                    {
                        if (mat[i, j])
                        {
                            Position origin = p.Position;
                            Position destination = new Position(i, j);
                            Piece capturedPiece = PerformMovement(origin, destination);
                            bool testCheck = IsInCheck(color);
                            UndoMovement(origin, destination, capturedPiece);
                            if (!testCheck)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void PutNewPiece(char column, int row, Piece piece)
        {
            Board.PutPiece(piece, new ChessPosition(column, row).ToPosition());
            _pieces.Add(piece);
        }

        public void PutPieces()
        {
            PutNewPiece('a', 1, new Tower(Board, Color.White));
            PutNewPiece('b', 1, new Horse(Board, Color.White));
            PutNewPiece('c', 1, new Bishop(Board, Color.White));
            PutNewPiece('d', 1, new Queen(Board, Color.White));
            PutNewPiece('e', 1, new King(Board, Color.White, this));
            PutNewPiece('f', 1, new Bishop(Board, Color.White));
            PutNewPiece('g', 1, new Horse(Board, Color.White));
            PutNewPiece('h', 1, new Tower(Board, Color.White));
            PutNewPiece('a', 2, new Pawn(Board, Color.White, this));
            PutNewPiece('b', 2, new Pawn(Board, Color.White, this));
            PutNewPiece('c', 2, new Pawn(Board, Color.White, this));
            PutNewPiece('d', 2, new Pawn(Board, Color.White, this));
            PutNewPiece('e', 2, new Pawn(Board, Color.White, this));
            PutNewPiece('f', 2, new Pawn(Board, Color.White, this));
            PutNewPiece('g', 2, new Pawn(Board, Color.White, this));
            PutNewPiece('h', 2, new Pawn(Board, Color.White, this));

            PutNewPiece('a', 8, new Tower(Board, Color.Blue));
            PutNewPiece('b', 8, new Horse(Board, Color.Blue));
            PutNewPiece('c', 8, new Bishop(Board, Color.Blue));
            PutNewPiece('d', 8, new Queen(Board, Color.Blue));
            PutNewPiece('e', 8, new King(Board, Color.Blue, this));
            PutNewPiece('f', 8, new Bishop(Board, Color.Blue));
            PutNewPiece('g', 8, new Horse(Board, Color.Blue));
            PutNewPiece('h', 8, new Tower(Board, Color.Blue));
            PutNewPiece('a', 7, new Pawn(Board, Color.Blue, this));
            PutNewPiece('b', 7, new Pawn(Board, Color.Blue, this));
            PutNewPiece('c', 7, new Pawn(Board, Color.Blue, this));
            PutNewPiece('d', 7, new Pawn(Board, Color.Blue, this));
            PutNewPiece('e', 7, new Pawn(Board, Color.Blue, this));
            PutNewPiece('f', 7, new Pawn(Board, Color.Blue, this));
            PutNewPiece('g', 7, new Pawn(Board, Color.Blue, this));
            PutNewPiece('h', 7, new Pawn(Board, Color.Blue, this));
        }
    }
}
