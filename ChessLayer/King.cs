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
    }
}
