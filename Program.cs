using Xadrez;
using Xadrez.BoardLayer;
using Xadrez.XadrezLayer;

Board Board = new Board(8, 8);

Board.PutPiece(new King(Board, Color.White), new Position(2, 4));
Board.PutPiece(new Tower(Board, Color.White), new Position(5, 2));

Screen.PrintBoard(Board);