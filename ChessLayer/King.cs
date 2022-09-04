using Xadrez.BoardLayer;

namespace Xadrez.ChessLayer
{
    internal class King : Piece
    {
        private ChessMatch _match;
        public King(Board board, Color color, ChessMatch match) : base(board, color)
        {
            _match = match;
        }

        public override string ToString()
        {
            return "K";
        }

        public bool CanMove(Position position)
        {
            Piece p = Board.Piece(position);
            return p == null || p.Color != Color;
        }

        private bool TestTowerToRoque(Position position)
        {
            Piece p = Board.Piece(position);
            return p != null && p is Tower && p.Color == Color && p.MovementsQuantity == 0;
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] mat = new bool[Board.Rows, Board.Columns];

            Position position = new Position(0, 0);

            position.DefineValues(Position.Row - 1, Position.Column);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Row, position.Column] = true;
            }

            position.DefineValues(Position.Row - 1, Position.Column + 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Row, position.Column] = true;
            }

            position.DefineValues(Position.Row, Position.Column + 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Row, position.Column] = true;
            }

            position.DefineValues(Position.Row + 1, Position.Column + 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Row, position.Column] = true;
            }

            position.DefineValues(Position.Row + 1, Position.Column);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Row, position.Column] = true;
            }

            position.DefineValues(Position.Row + 1, Position.Column - 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Row, position.Column] = true;
            }

            position.DefineValues(Position.Row, Position.Column - 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Row, position.Column] = true;
            }

            position.DefineValues(Position.Row - 1, Position.Column - 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Row, position.Column] = true;
            }

            if (MovementsQuantity == 0 && !_match.Check)
            {
                Position positionT1 = new Position(Position.Row, Position.Column + 3);
                if (TestTowerToRoque(positionT1))
                {
                    Position p1 = new Position(Position.Row, Position.Column + 1);
                    Position p2 = new Position(Position.Row, Position.Column + 2);
                    if (Board.Piece(p1) == null && Board.Piece(p2) == null)
                    {
                        mat[Position.Row, Position.Column + 2] = true;
                    }
                }
                Position positionT2 = new Position(Position.Row, Position.Column - 4);
                if (TestTowerToRoque(positionT2))
                {
                    Position p1 = new Position(Position.Row, Position.Column - 1);
                    Position p2 = new Position(Position.Row, Position.Column - 2);
                    Position p3 = new Position(Position.Row, Position.Column - 3);
                    if (Board.Piece(p1) == null && Board.Piece(p2) == null && Board.Piece(p3) == null)
                    {
                        mat[Position.Row, Position.Column + 2] = true;
                    }
                }
            }

            return mat;
        }
    }
}
