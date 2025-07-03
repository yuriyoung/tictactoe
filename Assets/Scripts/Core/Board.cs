using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    /// <summary>
    /// TODO: 对象池管理Block实例
    /// </summary>
    public class Board
    {
        public const int Size = 3;
        public Block Prototype { get; set; } = null;

        private readonly Block[,] blocks;

        public Board()
        {
            blocks = new Block[Size, Size];
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    blocks[r, c] = CreateBlock();
                }
            }
        }

        /// <summary>a copy of the given board</summary>
        public Board(Board board)
        {
            blocks = new Block[Size, Size];
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    Block clone = board[r, c];
                    if (clone != null)
                        this[r, c] = clone.Clone();
                }
            }
        }
        public Board(params (Cell cell, Block block)[] pairs)
        {
            blocks = new Block[Size, Size];
            foreach (var (cell, block) in pairs)
            {
                this[cell] = block;
            }
        }

        public Block this[Cell cell]
        {
            get
            {
                if (cell.IsValid())
                    return blocks[cell.row, cell.column];
                throw new System.ArgumentOutOfRangeException(nameof(cell), "Cell is out of bounds.");
            }
            set
            {
                if (cell.IsValid())
                    blocks[cell.row, cell.column] = value;
                else
                    throw new System.ArgumentOutOfRangeException(nameof(cell), "Cell is out of bounds.");
            }
        }

        public Block this[int row, int column]
        {
            get => this[new Cell(row, column)];
            set => this[new Cell(row, column)] = value;
        }

        public IReadOnlyList<Block> GetBlocks()
        {
            return blocks.Cast<Block>().ToList().AsReadOnly();
        }

        public void PlaceBlock(GameContext context, Placement placement)
        {
            if (this[placement.Start] is not { } blockToPlace)
            {
                throw new System.InvalidOperationException($"Block at {placement.Start} is not initialized.");
            }

            if (placement.Block != null && placement.Block != blockToPlace)
            {
                // 目前并没什么用，扩展其他类型游戏可能会有用
                this[placement.Start] = placement.Block; // 如果指定了不同的块，则替换该位置的块
                blockToPlace = placement.Block; // 替换新块
            }
            blockToPlace.ApplyPlacement(context, this, placement);
        }

        public bool AIAttemptPlace(Cell cell, Side side)
        {
            Block block = this[cell];
            if (block == null || block.IsOccupied)
                return false;
            return block.AIApply(side);
        }

        public void Clear()
        {
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    blocks[r, c] = null;
                }
            }
        }

        public bool IsAllCellsOccupied()
        {
            for (int r = 0; r < Board.Size; r++)
            {
                for (int c = 0; c < Board.Size; c++)
                {
                    if (!IsOccupied(r, c))
                        return false;
                }
            }
            return true;
        }

        public bool IsOccupied(Cell cell)
        {
            if (cell.IsValid())
            {
                Block block = this[cell];
                return block != null && block.IsOccupied;
            }
            throw new System.ArgumentOutOfRangeException(nameof(cell), "Cell is out of bounds.");
        }

        public bool IsOccupied(int row, int column)
        {
            if (IsValidPosition(row, column))
            {
                Block block = this[row, column];
                return block != null && block.IsOccupied;
            }
            throw new System.ArgumentOutOfRangeException($"Cell ({row}, {column}) is out of bounds.");
        }

        public bool IsOccupiedBySide(Cell cell, Side side)
        {
            if (cell.IsValid())
            {
                Block block = this[cell];
                return block != null && block.Owner == side;
            }
            throw new System.ArgumentOutOfRangeException(nameof(cell), "Cell is out of bounds.");
        }

        public bool IsOccupiedBySide(int row, int column, Side side)
        {
            if (IsValidPosition(row, column))
            {
                Block block = this[row, column];
                return block != null && block.Owner == side;
            }
            throw new System.ArgumentOutOfRangeException($"Cell ({row}, {column}) is out of bounds.");   
        }

        public bool IsValidPosition(int row, int column)
        {
            return row >= 0 && row < Size && column >= 0 && column < Size;
        }

        private Block CreateBlock()
        {
            return Prototype != null ? Prototype.Clone() : new TicTacToeBlock();
        }
    }
} // namespace TicTacToe
