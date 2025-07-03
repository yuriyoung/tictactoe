using System.Collections;
using System.Collections.Generic;

namespace TicTacToe
{
    public class Placement
    {
        /// <summary>
        /// contiguous blocks placement on the board
        /// path: start -> second -> third -> ... -> end
        /// </summary>
        public readonly Cell Start;
        public readonly Cell End;
        //public readonly IReadOnlyList<Cell> path;
        public readonly Block Block;

        public enum ActionType
        {
            Place,
            Remove,
            Move,
        }

        public readonly ActionType Action;

        public Placement(Cell position)
        : this(position, Cell.Invalid, null, ActionType.Place)
        {

        }

        public Placement(Cell start, Cell end)
            : this(start, end, null, ActionType.Move)
        {

        }

        public Placement(Cell position, Block block)
            : this(position, Cell.Invalid, block, ActionType.Place)
        {

        }

        internal Placement(Cell start, Cell end, Block block, ActionType action)
        {
            this.Start = start;
            this.End = end;
            this.Block = block;
            this.Action = action;
        }

        internal Placement(Placement other)
            : this(other.Start, other.End, other.Block, other.Action)
        {
        }

        public bool Equals(Placement other)
        {
            return Block == other.Block && Action == other.Action && Start == other.Start && End == other.End;
        }

        public override bool Equals(object obj)
        {
            return obj is Placement other && Equals(other);
        }

        /**
         * @note https://learn.microsoft.com/en-us/dotnet/csharp/misc/cs0659?f1url=%3FappId%3Droslyn%26k%3Dk(CS0659)
         */
        public override int GetHashCode()
        {
            unchecked
            {
                return Start.GetHashCode() * 132 ^ End.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"{Start} -> {End}";
        }
    }

} // namespace TicTacToe
