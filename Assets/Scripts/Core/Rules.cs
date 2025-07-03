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
            // 遍历整个棋盘是否存在符合3个连续被Owner(Side.Cross和Side.Circle)占用的棋子，如果有直接返回true,否则返回false
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
                    // 行&列
                    if (++rowCounts[ownerIndex, row] == size) return true;
                    if (++colCounts[ownerIndex, col] == size) return true;

                    // 正对角线
                    if (row == col)
                        if (++diagCounts[ownerIndex] == size) return true;

                    // 斜对角线
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
            // 正对角
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

            // 斜对角
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
            // 起始位置算1个
            int count = 0;

            // 向前检查
            Cell current = start;
            while (current.IsValid() && board[current].Owner == side)
            {
                count++;
                current += direction;
            }

            // 反方向
            current = start - direction;
            while (current.IsValid() && board[current].Owner == side)
            {
                count++;
                current -= direction;
            }

            // TODO: 不要使用棋盘的大小作为依据
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
        /// 从placement.start开始向周围8个方向检查所有具有相同Side的Cells能够连成获胜条件的链:
        /// 8个方向指的是上下左右和四个对角线方向（虽然是4个方向实际上检查了8个方向,因为每个方向都是双向的）
        /// 获胜条件是指在某个方向上有连续的3个相同Side的Cells
        /// TODO: 让Borad记录连成链的Cells，以便向玩家展示哪些Cells形成了链（在游戏场景中显示连接Cells的特效）
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