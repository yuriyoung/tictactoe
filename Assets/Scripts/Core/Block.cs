using System.Collections;
using System.Collections.Generic;

namespace TicTacToe
{
    public abstract class Block
    {
        public Side Owner { get; protected set; }

        public virtual bool IsOccupied => Owner != Side.None;

        protected Block(Side side)
        {
            Owner = side;
        }

        public abstract Block Clone();

        public abstract Dictionary<Cell, Placement> CreatePlacements(GameContext context, Board board, Cell position);

        public abstract bool ApplyPlacement(GameContext context, Board board, Placement placement);

        public virtual bool AIApply(Side side)
        {
            if (this.IsOccupied)
                return false;

            Owner = side;
            return true;
        }

        public override string ToString()
        {
            return $"{GetType().Name}";
        }
    }

    public abstract class Block<T> : Block where T : Block<T>, new()
    {
        protected Block(Side owner) : base(owner) { }

        public override Block Clone()
        {
            return new T { Owner = this.Owner };
        }
        public override Dictionary<Cell, Placement> CreatePlacements(GameContext context, Board board, Cell position)
        {
            Dictionary<Cell, Placement> results = null;

            if (board[position] is Block block && this == block)
            {
                if (!this.IsOccupied)
                {
                    if (results == null)
                        results = new Dictionary<Cell, Placement>();

                    // TODO: Rules::PlaceableTest(...)

                    results.Add(position, new Placement(position));
                }
            }

            return results;
        }

        public override bool ApplyPlacement(GameContext context, Board board, Placement placement)
        {
            if (context.TurnSide == Side.None || placement?.Action != Placement.ActionType.Place)
            {
                return false;
            }

            if (board[placement.Start] is Block block && this == block)
            {
                Owner = context.TurnSide;
                return true;
            }

            return false;
        }
    }

    public class TicTacToeBlock : Block<TicTacToeBlock>
    {
        public TicTacToeBlock() : base(Side.None) { }
        public TicTacToeBlock(Side owner) : base(owner) { }
    }

    public class  MoveableBlock : Block<MoveableBlock>
    {
        public MoveableBlock() : base(Side.None) { }
        public MoveableBlock(Side owner) : base(owner) { }

        public override Dictionary<Cell, Placement> CreatePlacements(GameContext context, Board board, Cell position)
        {
            Dictionary<Cell, Placement> results = null;

            if (board[position] is Block block && this == block)
            {
                if (Owner == context.TurnSide)
                {
                    bool hasResults = false;
                    if (results == null)
                        results = new Dictionary<Cell, Placement>();

                    foreach (Cell offset in Util.Directions)
                    {
                        Cell targetPosition = position + offset;
                        while (targetPosition.IsValid())
                        {
                            // 如果目标位置被占用，则继续向前查找
                            if (board.IsOccupied(targetPosition))
                                break;

                            // TODO: 使用Rules::MoveableTest(...)检查是否可移动
                            results.Add(targetPosition, new Placement(targetPosition));

                            hasResults = true;
                            targetPosition += offset;
                        }
                    }

                    if (!hasResults)
                        return null;
                }
            }

            return results;
        }
    }

} // namespace TicTacToe
