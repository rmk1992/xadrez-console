namespace Xadrez.BoardLayer
{
    internal abstract class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; protected set; }
        public int MovementsQuantity { get; protected set; }
        public Board Board { get; protected set; }

        public Piece(Board board, Color color)
        {
            Position = null;
            Board = board;
            Color = color;
            MovementsQuantity = 0;
        }

        public void IncrementMovementQuantity()
        {
            MovementsQuantity++;
        }

        public abstract bool[,] PossibleMovements();
    }
}
