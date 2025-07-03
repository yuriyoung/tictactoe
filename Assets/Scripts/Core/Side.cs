using System.ComponentModel;

namespace TicTacToe
{
    public enum Side
    {
        None,
        Cross, // A
        Circle,// B
    }

    public static class SideExtensions
    {
        public static Side Opponent(this Side side)
        {
            return side switch
            {
                Side.Cross => Side.Circle,
                Side.Circle => Side.Cross,
                _ => throw new InvalidEnumArgumentException($"Invalid side value '{nameof(side)}'", (int)side, typeof(Side))
            };
        }

        public static int Shift(this Side side)
        {
            return side switch
            {
                Side.Cross => 1,
                Side.Circle => -1,
                _ => throw new InvalidEnumArgumentException($"Invalid side value '{nameof(side)}'", (int)side, typeof(Side))
            };
        }
    }
} // namespace TicTacToe
