using Xadrez.BoardLayer;

namespace Xadrez.ChessLayer
{
    internal class Pawn : Piece
    {
        private ChessMatch _match;
        public Pawn(Board board, Color color, ChessMatch match) : base(board, color)
        {
            _match = match;
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

                if (Position.Row == 3)
                {
                    Position left = new Position(Position.Row, Position.Column - 1);
                    if (Board.ValidPosition(left) && HasEnemy(left) && Board.Piece(left) == _match.VulnerableEnPassant)
                    {
                        mat[left.Row - 1, left.Column] = true;
                    }
                    Position right = new Position(Position.Row, Position.Column + 1);
                    if (Board.ValidPosition(right) && HasEnemy(right) && Board.Piece(right) == _match.VulnerableEnPassant)
                    {
                        mat[right.Row - 1, right.Column] = true;
                    }
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

                if (Position.Row == 4)
                {
                    Position left = new Position(Position.Row, Position.Column - 1);
                    if (Board.ValidPosition(left) && HasEnemy(left) && Board.Piece(left) == _match.VulnerableEnPassant)
                    {
                        mat[left.Row + 1, left.Column] = true;
                    }
                    Position right = new Position(Position.Row, Position.Column + 1);
                    if (Board.ValidPosition(right) && HasEnemy(right) && Board.Piece(right) == _match.VulnerableEnPassant)
                    {
                        mat[right.Row + 1, right.Column] = true;
                    }
                }
            }
            return mat;
        }
    }
}
