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
        // ��Χ��8������λ��
        public static readonly Cell[] Surroundings =
        {
            new(1, 0),  // ��
            new(-1, 0), // ��
            new(0, -1), // ��
            new(0, 1),  // ��
            new(-1, -1),// ����
            new(-1, 1), // ����
            new(1, -1), // ����
            new(1, 1)   // ����
        };

        public static readonly Cell[] Directions =
        {
            new (0, 1),  // ˮƽ
            new (1, 0),  // ��ֱ 
            new (1, 1),  // ���Խ���
            new (1, -1)  // б�Խ���
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