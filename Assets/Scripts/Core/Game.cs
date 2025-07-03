using System.Collections;
using System.Collections.Generic;

namespace TicTacToe
{
    /// <summary>
    /// TODO: 统一异常处理 public static class GameExceptions {}
    /// TODO: 日志系统 public static class GameLogger
    /// TODO: 可序列化游戏状态
    /// </summary>

    public class Game
    {
        // 用于在界面显示历史操作步骤以及悔棋或回溯(落子有悔)
        public Board CurrentBoard => BoardHistory.Count > 0 ? BoardHistory[^1] : null;
       
        public GameContext CurrentContext => ContextHistory.Count > 0 ? ContextHistory[^1] : GameContext.DefaultContext;

        public GameState CurrentState => GameStates[^1];

        public IList<Board> BoardHistory { get; }
        public IList<GameContext> ContextHistory { get; }
        public IList<Dictionary<Block, Dictionary<Cell, Placement>>> PlacementsHistory { get; } // 扩展其他类型游戏预留的接口
        public IList<GameState> GameStates { get; }

        public Game() : this(GameContext.DefaultContext, Util.DefaultBlocks)
        {
        }

        public Game(GameContext context, params (Cell cell, Block block)[] dict)
        {
            Board board = new Board(dict);
            BoardHistory = new List<Board> { board };
            ContextHistory = new List<GameContext> { context };
            GameStates = new List<GameState> { GameState.OriginalState };
            PlacementsHistory = new List<Dictionary<Block, Dictionary<Cell, Placement>>>
            {
                GetLegalPlacements(board, context)
            };
        }

        /// <summary>
        /// 放置棋子并切换下一个玩家的回合
        /// </summary>
        public bool ExecutePlacement(Placement placement)
        {
            if (!ValidatePlacement(placement))
                return false;

            Board beforeBoard = BoardHistory[^1];
            GameContext beforeCtx = ContextHistory[^1];

            // 已经分出胜负，不再继续放置操作，这里无需检查胜负应由调用者负责游戏状态
            //if (CurrentState.CausedDraw || CurrentState.CausedWin)
            //{
            //    if(Rules.TestContiguousBlocks(beforeBoard))
            //        return false;
            //}

            // 落棋后保存历史棋盘
            Board afterBoard = new Board(beforeBoard);
            afterBoard.PlaceBlock(beforeCtx, placement);
            BoardHistory.Add(afterBoard);
            bool wining = Rules.IsContiguousBlocks(placement.Start, afterBoard, beforeCtx.TurnSide);

            // 完后交换对手
            GameContext afterCtx = beforeCtx.Advance(afterBoard);
            ContextHistory.Add(afterCtx);

            // 检查对手是否可落棋，在扩展其他类型的游戏可用
            //Dictionary<Block, Dictionary<Cell, Placement>> placeables = GetLegalPlacements(afterBoard, afterCtx);
            //PlacementsHistory.Add(placeables);

            // 保存当前状态
            var state = new GameState(cell: placement.Start, side:beforeCtx.TurnSide, block: beforeBoard[placement.Start], placement:placement);
            state.SetGameState(wining, !wining && afterBoard.IsAllCellsOccupied());
            GameStates.Add(state);

            return true;
        }

        public bool ValidatePlacement(Placement placement)
        {
            if (placement == null || !placement.Start.IsValid())
                return false;
            return BoardHistory[^1] is { } currentBoard  && currentBoard[placement.Start] is not null;
        }

        public bool GetLegalPlacement(Cell cell, out Placement placement)
        {
            placement = null;
            // 获取棋盘的最新状态
            if (!cell.IsValid() || BoardHistory[^1].IsOccupied(cell))
                return false;

            placement = new Placement(cell);
            return true;
        }

        public bool GetLegalPlacementMove(Cell cell, out Placement placement)
        {
            placement = null;
            // 获取棋盘的最新状态
            if (!cell.IsValid() || BoardHistory[^1].IsOccupied(cell))
                return false;

            placement = new Placement(cell, Cell.Invalid);
            return true;
        }

        public void ClearHistory()
        {
            GameStates.Clear();
            BoardHistory.Clear();
            ContextHistory.Clear();
            PlacementsHistory.Clear();
            BoardHistory.Add(new Board());
            GameStates.Add(GameState.OriginalState);
            ContextHistory.Add(GameContext.DefaultContext);
            PlacementsHistory.Add(GetLegalPlacements(CurrentBoard, CurrentContext));
        }

        /// <summary>
        /// 扩展其他类型游戏预留的接口
        /// </summary>
        /// <param name="board"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Dictionary<Block, Dictionary<Cell, Placement>> GetLegalPlacements(Board board, GameContext context)
        {
            Dictionary<Block, Dictionary<Cell, Placement>> results =  null;

            for (int row = 0; row < Board.Size; row++)
            {
                for (int col = 0; col < Board.Size; col++)
                {
                    if (board[row, col] is Block block)
                    {
                        if(!block.IsOccupied && block.CreatePlacements(context, board, new Cell(row, col)) is { } dict)
                        {
                            if (results == null)
                                results = new Dictionary<Block, Dictionary<Cell, Placement>>();
                            results.Add(block, dict);
                        }
                    }
                }
            }
            return results;
        }
    }
} // namespace TicTacToe
