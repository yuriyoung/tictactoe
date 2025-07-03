using System.Collections;
using System.Collections.Generic;

namespace TicTacToe
{
    /// <summary>
    /// TODO: ͳһ�쳣���� public static class GameExceptions {}
    /// TODO: ��־ϵͳ public static class GameLogger
    /// TODO: �����л���Ϸ״̬
    /// </summary>

    public class Game
    {
        // �����ڽ�����ʾ��ʷ���������Լ���������(�����л�)
        public Board CurrentBoard => BoardHistory.Count > 0 ? BoardHistory[^1] : null;
       
        public GameContext CurrentContext => ContextHistory.Count > 0 ? ContextHistory[^1] : GameContext.DefaultContext;

        public GameState CurrentState => GameStates[^1];

        public IList<Board> BoardHistory { get; }
        public IList<GameContext> ContextHistory { get; }
        public IList<Dictionary<Block, Dictionary<Cell, Placement>>> PlacementsHistory { get; } // ��չ����������ϷԤ���Ľӿ�
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
        /// �������Ӳ��л���һ����ҵĻغ�
        /// </summary>
        public bool ExecutePlacement(Placement placement)
        {
            if (!ValidatePlacement(placement))
                return false;

            Board beforeBoard = BoardHistory[^1];
            GameContext beforeCtx = ContextHistory[^1];

            // �Ѿ��ֳ�ʤ�������ټ������ò���������������ʤ��Ӧ�ɵ����߸�����Ϸ״̬
            //if (CurrentState.CausedDraw || CurrentState.CausedWin)
            //{
            //    if(Rules.TestContiguousBlocks(beforeBoard))
            //        return false;
            //}

            // ����󱣴���ʷ����
            Board afterBoard = new Board(beforeBoard);
            afterBoard.PlaceBlock(beforeCtx, placement);
            BoardHistory.Add(afterBoard);
            bool wining = Rules.IsContiguousBlocks(placement.Start, afterBoard, beforeCtx.TurnSide);

            // ��󽻻�����
            GameContext afterCtx = beforeCtx.Advance(afterBoard);
            ContextHistory.Add(afterCtx);

            // �������Ƿ�����壬����չ�������͵���Ϸ����
            //Dictionary<Block, Dictionary<Cell, Placement>> placeables = GetLegalPlacements(afterBoard, afterCtx);
            //PlacementsHistory.Add(placeables);

            // ���浱ǰ״̬
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
            // ��ȡ���̵�����״̬
            if (!cell.IsValid() || BoardHistory[^1].IsOccupied(cell))
                return false;

            placement = new Placement(cell);
            return true;
        }

        public bool GetLegalPlacementMove(Cell cell, out Placement placement)
        {
            placement = null;
            // ��ȡ���̵�����״̬
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
        /// ��չ����������ϷԤ���Ľӿ�
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