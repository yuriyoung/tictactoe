using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    /**
     * @note 嗯？c#没有final修饰符？用sealed来替代。
     **/
    public sealed class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public static event Action NewGameStartedEvent;
        public static event Action NewGameFinishedEvent;
        public static event Action GameExecutedEvent;

        public Constants.GameMode GameMode => m_gameMode;

        public Board CurrentBoard => game.CurrentBoard;
        public GameState CurrentState => game.CurrentState;
        public Side TurnSide => game.CurrentContext.TurnSide;
        public Side StartingSide => game.ContextHistory[0].TurnSide;

        private readonly List<(Cell, Block)> m_currentBlocks = new List<(Cell, Block)>();

        private static GameManager m_instance;
        private Constants.GameMode m_gameMode;
        private bool m_isCircleAI;
        private Game game = null;
        private Minimax m_minimaxAI = new Minimax(GameContext.DefaultContext.TurnSide.Opponent());
        private readonly System.Random m_random = new System.Random();

        public List<(Cell, Block)> CurrentBlocks
        {
            get
            {
                m_currentBlocks.Clear();
                for (int row = 0; row < Board.Size; row++)
                {
                    for (int col = 0; col < Board.Size; col++)
                    {
                        Block block = CurrentBoard[row, col];
                        if (block != null)
                        {
                            m_currentBlocks.Add((new Cell(row, col), block));
                        }
                    }
                }

                return m_currentBlocks;
            }
        }

        public void StartNewGame(Constants.GameMode mode = Constants.GameMode.AIMode, bool isCircleAI = true)
        {
            Debug.Log("Starting a new game...");
            if (game != null)
                game = null;
            
            game = new Game();
            m_gameMode = mode;
            m_isCircleAI = isCircleAI;

            NewGameStartedEvent?.Invoke();
        }

        public void SetSupperAIEnabled(bool enable)
        {
            if (m_minimaxAI != null)
                m_minimaxAI = null;

            m_minimaxAI = new(GameContext.DefaultContext.TurnSide.Opponent(), enable);
        }

        private bool IsGameOver()
        {
            return game.CurrentState.CausedWin || game.CurrentState.CausedDraw;
        }

        private bool TryExecute(Placement move)
        {
            if (!game.ExecutePlacement(move))
                return false;

            BoardManager.Instance.SetAllBlocksActive(false);
            GameExecutedEvent?.Invoke();

            return true;
        }

        private async void OnBlockPlacement(Cell cell, Transform from, Transform to)
        {
            if(!game.GetLegalPlacement(cell, out Placement placement))
            {
                Debug.Log("Could no get legal placement");
                return;
            }

            if (!TryExecute(placement))
            {
                Debug.Log("Exuceute placement failed");
                return;
            }

            BoardManager.Instance.UpdateBlockMarker(cell, game.CurrentState.Side);
            if (IsGameOver())
            {
                NewGameFinishedEvent?.Invoke();
                BoardManager.Instance.SetAllBlocksActive(false);
                Debug.Log($"Game Over: state({game.CurrentState})");
                return;
            }

            if (m_gameMode == Constants.GameMode.AIMode && (m_isCircleAI && TurnSide == Side.Circle) && !IsGameOver())
            {
                Placement beastPlacement = await m_minimaxAI.BestPlacement(CurrentBoard, game.CurrentContext, m_random.Next(500, 3000));
                DoAIExecute(beastPlacement);
                if(!IsGameOver())
                    BoardManager.Instance.SetAllBlocksActive(true);
            }
        }

        private void DoAIExecute(Placement move)
        {
            GameObject thisBlock = BoardManager.Instance.GetBlockAtPosition(move.Start);
            GameObject to = BoardManager.Instance.GetBlockAtPosition(move.End); // 如果有
            OnBlockPlacement(move.Start, thisBlock.transform, to?.transform);
        }

        private void OnEnable()
        {
            GameBlock.OnMouseClicked += OnBlockPlacement;
        }

        private void OnDisable()
        {
            GameBlock.OnMouseClicked -= OnBlockPlacement;
        }

        private void Start()
        {
            StartNewGame(Constants.GameMode.AIMode);
            
        }

        private void OnDestroy()
        {
            Debug.Log("GameManager: GameObject destroyed");
        }
    }
}// namespace TicTacToe
