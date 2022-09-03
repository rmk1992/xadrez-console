using Xadrez.BoardLayer;

namespace Xadrez.ChessLayer
{
    internal class Pawn : Piece
    {
        public Pawn(Board board, Color color) : base(board, color)
        {

        }

        public override string ToString()
        {
            return "P";
        }
       
        private bool HasEnemy(Position position)
        {
            Piece p = Board.Piece(position);
            return p != null && p.Color != Color;
        }

        private bool FreeSpace(Position position)
        {
            return Board.Piece(position) == null;
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] mat = new bool[Board.Rows, Board.Columns];

            Position position = new Position(0, 0);

            if (Color == Color.White)
            {
                position.DefineValues(Position.Row - 1, Position.Column);
                if (Board.ValidPosition(position) && FreeSpace(position))
                {
                    mat[position.Row, position.Column] = true;
                }

                position.DefineValues(Position.Row - 2, Position.Column);
                if (Board.ValidPosition(position) && FreeSpace(position) && MovementsQuantity == 0)
                {
                    mat[position.Row, position.Column] = true;
                }

                position.DefineValues(Position.Row - 1, Position.Column - 1);
                if (Board.ValidPosition(position) && HasEnemy(position))
                {
                    mat[position.Row, position.Column] = true;
                }

                position.DefineValues(Position.Row - 1, Position.Column + 1);
                if (Board.ValidPosition(position) && HasEnemy(position))
                {
                    mat[position.Row, position.Column] = true;
                }
            }
            else
            {
                position.DefineValues(Position.Row + 1, Position.Column);
                if (Board.ValidPosition(position) && FreeSpace(position))
                {
                    mat[position.Row, position.Column] = true;
                }

                position.DefineValues(Position.Row + 2, Position.Column);
                if (Board.ValidPosition(position) && FreeSpace(position) && MovementsQuantity == 0)
                {
                    mat[position.Row, position.Column] = true;
                }

                position.DefineValues(Position.Row + 1, Position.Column - 1);
                if (Board.ValidPosition(position) && HasEnemy(position))
                {
                    mat[position.Row, position.Column] = true;
                }

                position.DefineValues(Position.Row + 1, Position.Column + 1);
                if (Board.ValidPosition(position) && HasEnemy(position))
                {
                    mat[position.Row, position.Column] = true;
                }
            }
            return mat;
        }
    }
}
