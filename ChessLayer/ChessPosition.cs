﻿using Xadrez.BoardLayer;

namespace Xadrez.ChessLayer
{
    internal class ChessPosition
    {
        public int Row { get; set; }
        public char Column { get; set; }

        public ChessPosition(int row, char column)
        {
            Row = row;
            Column = column;
        }

        public Position ToPosition()
        {
            return new Position(8 - Row, Column - 'a');
        }

        public override string ToString()
        {
            return "" + Column + Row;
        }
    }
}