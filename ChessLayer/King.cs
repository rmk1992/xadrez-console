using Xadrez.BoardLayer;

namespace Xadrez.ChessLayer
{
    internal class King : Piece
    {
        public King(Board board, Color color) : base(board, color)
        {

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

            return mat;
        }
    }
}
