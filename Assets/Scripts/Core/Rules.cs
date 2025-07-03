using System.Collections;
using System.Collections.Generic;

namespace TicTacToe
{
    public static class Rules
    {
        public static bool TestContiguousBlocks(Board board, Side side)
        {
            int[] rowCounts = new int[Board.Size];
            int[] colCounts = new int[Board.Size];
            int diagonal = 0, antiDiagonal = 0;

            for(int row = 0; row < Board.Size; row++)
            {
                for(int col = 0; col < Board.Size; col++)
                {
                    if (board.IsOccupiedBySide(row, col, side))
                    {
                        rowCounts[row]++;
                        colCounts[col]++;
                        if (row == col)
                            diagonal++;
                        if (row + col == Board.Size - 1)
                            antiDiagonal++;
                        //if (++rowCounts[row] == Board.Size) return true;
                        //if (++colCounts[col] == Board.Size) return true;
                        //if (row == col && ++diagonal == Board.Size) return true;
                        //if (row + col == Board.Size - 1 && ++antiDiagonal == Board.Size) return true;
                    }
                }
            }

            for (int i = 0; i < Board.Size; i++)
            {
                if (rowCounts[i] >= Board.Size || colCounts[i] >= Board.Size)
                    return true;
            }
            return diagonal >= Board.Size || antiDiagonal >= Board.Size;
            //return false;
        }

        public static bool TestContiguousBlocks(Board board)
        {
            // �������������Ƿ���ڷ���3��������Owner(Side.Cross��Side.Circle)ռ�õ����ӣ������ֱ�ӷ���true,���򷵻�false
            int sideCount = 3;
            int size = Board.Size;

            int[,] rowCounts = new int[Board.Size, size];
            int[,] colCounts = new int[Board.Size, size];
            int[] diagCounts = new int[sideCount];
            int[] antiDiagCounts = new int[sideCount];

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    Block block = board[row, col];
                    if (block == null || !block.IsOccupied) continue;

                    int ownerIndex = (int)block.Owner;
                    // ��&��
                    if (++rowCounts[ownerIndex, row] == size) return true;
                    if (++colCounts[ownerIndex, col] == size) return true;

                    // ���Խ���
                    if (row == col)
                        if (++diagCounts[ownerIndex] == size) return true;

                    // б�Խ���
                    if (row + col == size - 1)
                        if (++antiDiagCounts[ownerIndex] == size) return true;
                }
            }

            return false;
        }

        public static bool IsRowContiguous(Board board, Side side, int row)
        {
            int count = 0;
            for (int col = 0; col < Board.Size; col++)
            {
                if (board.IsOccupiedBySide(row, col, side))
                {
                    count++;
                }
            }
            return count >= Board.Size;
        }

        public static bool IsColumnContiguous(Board board, Side side, int column)
        {
            int count = 0;
            for (int row = 0; row < Board.Size; row++)
            {
                if (board.IsOccupiedBySide(row, column, side))
                {
                    count++;
                }
            }
            return count >= Board.Size;
        }

        public static bool IsDiagonalContiguous(Board board, Side side)
        {
            // ���Խ�
            int countMain = 0;
            for (int i = 0; i < Board.Size; i++)
            {
                if (board.IsOccupiedBySide(i, i, side))
                {
                    countMain++;
                }
            }
            if (countMain >= Board.Size)
                return true;

            // б�Խ�
            int countAnti = 0;
            for (int i = 0; i < Board.Size; i++)
            {
                if (board.IsOccupiedBySide(i, Board.Size - 1 - i, side))
                {
                    countAnti++;
                }
            }
            return countAnti >= Board.Size;
        }

        public static bool HasContiguousBlock(Cell start, Cell direction, Board board, Side side)
        {
            // ��ʼλ����1��
            int count = 0;

            // ��ǰ���
            Cell current = start;
            while (current.IsValid() && board[current].Owner == side)
            {
                count++;
                current += direction;
            }

            // ������
            current = start - direction;
            while (current.IsValid() && board[current].Owner == side)
            {
                count++;
                current -= direction;
            }

            // TODO: ��Ҫʹ�����̵Ĵ�С��Ϊ����
            return count >= Board.Size;
        }

        public static IList<Cell> collectCorrelationBlocks(Cell start, Cell direction, Board board, Side side)
        {
            List<Cell> cells = new List<Cell>();
            Cell current = start;

            // forward direction
            while (current.IsValid() && board[current].Owner == side)
            {
                cells.Add(current);
                current += direction;
            }
            // reverse direction
            current = start - direction;
            while (current.IsValid() && board[current].Owner == side)
            {
                cells.Add(current);
                current -= direction;
            }

            return cells;
        }

        /// <summary>
        /// ��placement.start��ʼ����Χ8�����������о�����ͬSide��Cells�ܹ����ɻ�ʤ��������:
        /// 8������ָ�����������Һ��ĸ��Խ��߷�����Ȼ��4������ʵ���ϼ����8������,��Ϊÿ��������˫��ģ�
        /// ��ʤ������ָ��ĳ����������������3����ͬSide��Cells
        /// TODO: ��Borad��¼��������Cells���Ա������չʾ��ЩCells�γ�����������Ϸ��������ʾ����Cells����Ч��
        /// </summary>
        public static bool IsContiguousBlocks(Cell cellQuestion, Board board, Side friendlySide)
        {
            //Side enemySide = friendlySide.Opponent();
            //Cell neighbor = cellQuestion + offset;

            if (board[cellQuestion]?.Owner != friendlySide)
                return false;

            foreach (Cell offset in Util.Directions)
            {
                if (HasContiguousBlock(cellQuestion, offset, board, friendlySide))
                {
                    return true;
                }

            }
            return false;
        }

    }
}// namespace TicTacToe