using System.Collections;
using System.Collections.Generic;

namespace TicTacToe
{
    public struct GameContext
    {
        public readonly Side TurnSide;
        public readonly int TurnNumber;

        // 除非有掷骰子玩法，否则固定玩家先手
        public static GameContext DefaultContext => new GameContext(side: Side.Cross, turns: 1);

        public GameContext(Side side, int turns)
        {
            this.TurnSide = side;
            this.TurnNumber = turns;
        }

        public GameContext Advance(Board borad)
        {
            // DefaultContext总是以Player Cross开始回合，所以只有当 TurnSide 是Player Circle时，才会增加回合数。
            return new GameContext(side: TurnSide.Opponent(), turns: TurnNumber + (TurnSide == Side.Cross ? 0 : 1));
        }
    }

    public struct GameState
    {
        public readonly Cell Cell;
        public readonly Side Side;
        public readonly Block Block;
        public readonly Placement Placement;
        public bool CausedWin { get; private set; }
        public bool CausedDraw { get; private set; }

        public static GameState OriginalState => new GameState(cell: Cell.Invalid, side: Side.None, block: null, placement: null);

        public GameState(Cell cell, Side side, Block block, Placement placement)
        {
            this.Cell = cell;
            this.Side = side;
            this.Block = block;
            this.Placement = placement;
            this.CausedWin = false;
            this.CausedDraw = false;
        }

        public void SetGameState(bool causedWin, bool causedDraw)
        {
            this.CausedWin = causedWin;
            this.CausedDraw = causedDraw;
        }

        public override string ToString()
        {
            return $"Winning:{CausedWin}, Draw: {CausedDraw}, Last Turn: {Side}";
        }
    }
}
