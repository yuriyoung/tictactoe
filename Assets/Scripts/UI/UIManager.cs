using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    public class UIManager : MonoBehaviourSingleton<UIManager>
    {
        [SerializeField] private Image m_resultMessageBox = null;
        [SerializeField] private TextMeshProUGUI m_mssageBoxText = null;
        [SerializeField] private Image m_messageBoxIcon = null;
        [SerializeField] private Image m_crossTurnIndicator = null;
        [SerializeField] private Image m_circleTurnIndicator = null;

        public void StartNewGame() => GameManager.Instance.StartNewGame();
        public void ExitGame() => Application.Quit();
        public void SetSupperAIEnabled(bool enable) => GameManager.Instance.SetSupperAIEnabled(enable);
        

        private void Start()
        {
            GameManager.NewGameStartedEvent += OnNewGameStarted;
            GameManager.NewGameFinishedEvent += OnNewGameFinished;
            GameManager.GameExecutedEvent += OnPlacementExecuted;
        }

        private void OnNewGameStarted()
        {
            UpdateTurnIndicators();

            m_resultMessageBox.gameObject.SetActive(false);
        }

        private void OnNewGameFinished() 
        {
            // 直接显示O还是X玩家即可
            var state = GameManager.Instance.CurrentState;
            if(state.CausedWin /*&& state.Side == GameManager.Instance.StartingSide*/)
            {
                m_messageBoxIcon.gameObject.SetActive(true);
                m_messageBoxIcon.sprite = state.Side == Side.Circle ? m_circleTurnIndicator.sprite : m_crossTurnIndicator.sprite;
                m_mssageBoxText.text = $"{state.Side} 赢了！";
            }
            else
            {
                m_messageBoxIcon.gameObject.SetActive(false);
                m_mssageBoxText.text = "平局！";
            }
            
            m_resultMessageBox.gameObject.SetActive(true);
        }

        private void OnPlacementExecuted()
        {
            UpdateTurnIndicators();
        }

        private void UpdateTurnIndicators()
        {
            Side turn = GameManager.Instance.TurnSide;
            m_circleTurnIndicator.enabled = turn == Side.Circle;
            m_crossTurnIndicator.enabled = turn == Side.Cross;
        }
    }
} // namespace  TicTacToe