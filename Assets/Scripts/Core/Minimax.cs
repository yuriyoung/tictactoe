using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicTacToe
{
    /// <summary>
    /// ʹ�ü���Сֵ�㷨������������λ��
    /// �ο����ϴ������ʵ�ַ���
    /// </summary>
    public class Minimax
    {
        private readonly Side m_aiSide;
        private readonly float m_power;
        private const int MaxDepth = 3;

        public Minimax(Side aiSide, bool enablePower = true)
        {
            m_aiSide = aiSide;
            m_power = enablePower ? 100f : -100f;
        }

        public async Task<Placement> BestPlacement(Board board, GameContext context, int delayMs = 500)
        {
            var result = await Task.Run(() =>
            {
                Placement bestPlacement = null;
                float bestScore = float.NegativeInfinity;

                float alpha = float.NegativeInfinity;
                float beta = float.PositiveInfinity;

                for (int row = 0; row < Board.Size; row++)
                {
                    for (int col = 0; col < Board.Size; col++)
                    {
                        if (board.IsOccupied(row, col))
                            continue;

                        // ģ������
                        Board nextBoard = new Board(board);
                        nextBoard.AIAttemptPlace(new Cell(row, col), m_aiSide);
                        // �����غ�
                        float score = this.DoMinimax(nextBoard, m_aiSide.Opponent(), 0, alpha, beta);

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestPlacement = new Placement(new Cell(row, col));
                        }

                        alpha = Math.Max(alpha, bestScore);
                    }
                }

                return bestPlacement;
            });

            // ģ��AI˼���ӳ�
            if (delayMs > 0)
                await Task.Delay(delayMs);

            return result;
        }

        /// <summary>
        /// ʹ��alpha-beta��֦�Ż�
        /// </summary>
        /// <param name="board"></param>
        /// <param name="maximizing"></param>
        /// <returns></returns>
        private float DoMinimax(Board board, Side turnSide, int depth, float alpha, float beta)
        {
            float score = Evaluate(board, turnSide, out bool full);
            // score != 0  || depth >= MaxDepth
            if (Math.Abs(score) == Math.Abs(m_power)  || full)
                return score;

            //Debug.Log($"DoMinmax: score[{score}], depth[{depth}], turn[{turnSide}], alpha[{alpha}], beta[{beta}]");

            // ����С��
            float bestScore = (turnSide == m_aiSide) ? float.NegativeInfinity : float.PositiveInfinity;
            for (int row = 0; row < Board.Size; row++)
            {
                for (int col = 0; col < Board.Size; col++)
                {
                    if (board.IsOccupied(row, col))
                        continue;

                    // �½�����ģ������
                    Board nextBoard = new Board(board);
                    // �����轻����һ���غ�
                    // TODO:�д��Ż�Board::PlaceBlock�ӿ�
                    nextBoard.AIAttemptPlace(new Cell(row, col), turnSide);
                    //һ����ʤ�����������˳��ݹ�
                    float alphaScore = DoMinimax(nextBoard, turnSide.Opponent(), depth++, alpha, beta);

                    if (turnSide == m_aiSide)
                    {
                        bestScore = MathF.Max(bestScore, alphaScore);
                        alpha = MathF.Max(alpha, bestScore);
                        if (beta <= alpha) // ��֦
                            return bestScore; 
                    }
                    else
                    {
                        bestScore = MathF.Min(bestScore, alphaScore);
                        beta = MathF.Min(beta, bestScore);
                        if (beta <= alpha)// ��֦
                            return bestScore; 
                    }
                }
            }

            return bestScore;
        }

        private float Evaluate(Board board, Side turnSide, out bool full)
        {
            full = false;
            bool crossWin = Rules.TestContiguousBlocks(board, Side.Cross);
            bool circleWin = Rules.TestContiguousBlocks(board, Side.Circle);

            // �����ϲ�������������
            if (crossWin && circleWin)
                return 0;
            // ����cross��circle�ķ�ֵ���ı�AI����
            if (crossWin)
                return m_aiSide == Side.Cross ? m_power: -m_power;
            if (circleWin)
                return m_aiSide == Side.Circle ? m_power : -m_power;
            if (full = board.IsAllCellsOccupied())
                return 0;

            // ��ֹ����ռ�������Խ�λ��
            if (!board[1,1].IsOccupied && m_aiSide == turnSide)
            {
                return -m_power;
            }

            // ������������ֵ�����֣���ʹ�ø����ӵ��㷨����÷���
            int crossCount = 0, circleCount = 0;
            foreach (var block in board.GetBlocks())
            {
                if (block == null) continue;
                if (block.Owner == Side.Cross) crossCount++;
                else if (block.Owner == Side.Circle) circleCount++;
            }
            return (m_aiSide == Side.Cross ? crossCount - circleCount : circleCount - crossCount);
        }
    }
} // namespace TicTacToe
