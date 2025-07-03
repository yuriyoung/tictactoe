using System.Collections;
using System.Collections.Generic;

namespace TicTacToe
{
    public static class Util
    {
        /**
         *    0   1   2   3   4   ...
         *  0 +---+---+---+---+---+--
         *    |   |   |   |   |   |
         *  1 +---+---+---+---+---+---
         *    |   |   |   |   |   |
         *  2 +---+---+---+---+---+---
         *    |   |   |   |   |   |
         *  3 +---+---+---+---+---+---
         *    |   |   |   |   |   |
         *  4 +---+---+---+---+---+---
         *    |   |   |   |   |   |
         * ...+---+---+---+---+---+---
         *    |   |   |   |   |   |
         *    
         *  (-1, -1), (-1, 0), (-1, 1),
         *  (0, -1),  [origin] (0, 1),
         *  (1, -1), (1, 0),   (1, 1)
         */
        // 周围的8个矩形位置
        public static readonly Cell[] Surroundings =
        {
            new(1, 0),  // 上
            new(-1, 0), // 下
            new(0, -1), // 左
            new(0, 1),  // 右
            new(-1, -1),// 左上
            new(-1, 1), // 右上
            new(1, -1), // 左下
            new(1, 1)   // 右下
        };

        public static readonly Cell[] Directions =
        {
            new (0, 1),  // 水平
            new (1, 0),  // 垂直 
            new (1, 1),  // 正对角线
            new (1, -1)  // 斜对角线
        };

        public static readonly (Cell, Block)[] DefaultBlocks =
        {
            (new Cell(0, 0), new TicTacToeBlock()),
            (new Cell(0, 1), new TicTacToeBlock()),
            (new Cell(0, 2), new TicTacToeBlock()),
            (new Cell(1, 0), new TicTacToeBlock()),
            (new Cell(1, 1), new TicTacToeBlock()),
            (new Cell(1, 2), new TicTacToeBlock()),
            (new Cell(2, 0), new TicTacToeBlock()),
            (new Cell(2, 1), new TicTacToeBlock()),
            (new Cell(2, 2), new TicTacToeBlock()),
        };

        public static string CellToString(Cell cell) => CellToString(cell.row, cell.column);

        public static string CellToString(int row, int column) => $"{row}{column}";

        public static Cell StringToCell(string cellString)
        {
            if (string.IsNullOrEmpty(cellString) || cellString.Length != 2)
                return Cell.Invalid;
            if (int.TryParse(cellString[0].ToString(), out int row) && int.TryParse(cellString[1].ToString(), out int column))
            {
                return new Cell(row, column);
            }
            return Cell.Invalid;
        }
    }
} // namespace TicTacToe
