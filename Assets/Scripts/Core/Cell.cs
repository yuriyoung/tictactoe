using System.Collections;
using System.Collections.Generic;

namespace TicTacToe
{
    /// <summary>a cell on a board</summary>
    public readonly struct Cell
    {
        public readonly int row;
        public readonly int column;

        /// <summary>
        ///  无效的位置
        /// </summary>
        public static readonly Cell Invalid = new Cell(-1, -1);

        private const int MinBound = 0;
        private const int MaxBound = 3;

        public Cell(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public Cell(string cellString)
        {
            this = Util.StringToCell(cellString);
        }

        internal Cell(Cell start, int rowOffset, int columnOffset)
        {
            row = start.row + rowOffset;
            column = start.column + columnOffset;
        }

        internal readonly bool IsValid()
        {
            return row >= MinBound && row < MaxBound && column >= MinBound && column < MaxBound;
        }

        public static bool operator ==(Cell lhs, Cell rhs)
        {
            return lhs.row == rhs.row && lhs.column == rhs.column;
        }
        public static bool operator !=(Cell lhs, Cell rhs)
        {
            return !(lhs == rhs);
        }

        public static Cell operator +(Cell lhs, Cell rhs)
        {
            return new Cell(lhs.row + rhs.row, lhs.column + rhs.column);
        }

        public static Cell operator -(Cell lhs, Cell rhs)
        {
            return new Cell(lhs.row - rhs.row, lhs.column - rhs.column);
        }

        public bool Equals(Cell other)
        {
            return row == other.row && column == other.column;
        }
        public bool Equals(int row, int column)
        {
            return this.row == row && this.column == column;
        }

        public bool Equals(int index)
        {
            return index == row * MaxBound + column;
        }

        public override bool Equals(object obj)
        {
            return obj is Cell other && Equals(other);
        }

        /**
         * @note https://learn.microsoft.com/en-us/dotnet/csharp/misc/cs0661?f1url=%3FappId%3Droslyn%26k%3Dk(CS0661)
         */
        public override int GetHashCode()
        {
            unchecked
            {
                return (row * 132) ^ column;
            }
        }

        public override string ToString() => Util.CellToString(row, column);
 
    }
} // namespace TicTacToe
