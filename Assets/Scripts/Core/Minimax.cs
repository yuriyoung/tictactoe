using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicTacToe
{
    /// <summary>
    /// 使用极大极小值算法计算出最佳落棋位置
    /// 参考网上大多数的实现方法
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

                        // 模拟落子
                        Board nextBoard = new Board(board);
                        nextBoard.AIAttemptPlace(new Cell(row, col), m_aiSide);
                        // 交换回合
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

            // 模拟AI思考延迟
            if (delayMs > 0)
                await Task.Delay(delayMs);

            return result;
        }

        /// <summary>
        /// 使用alpha-beta剪枝优化
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

            // 极大极小化
            float bestScore = (turnSide == m_aiSide) ? float.NegativeInfinity : float.PositiveInfinity;
            for (int row = 0; row < Board.Size; row++)
            {
                for (int col = 0; col < Board.Size; col++)
                {
                    if (board.IsOccupied(row, col))
                        continue;

                    // 新建副本模拟落子
                    Board nextBoard = new Board(board);
                    // 这里需交换到一方回合
                    // TODO:有待优化Board::PlaceBlock接口
                    nextBoard.AIAttemptPlace(new Cell(row, col), turnSide);
                    //一方获胜或棋盘已满退出递归
                    float alphaScore = DoMinimax(nextBoard, turnSide.Opponent(), depth++, alpha, beta);

                    if (turnSide == m_aiSide)
                    {
                        bestScore = MathF.Max(bestScore, alphaScore);
                        alpha = MathF.Max(alpha, bestScore);
                        if (beta <= alpha) // 剪枝
                            return bestScore; 
                    }
                    else
                    {
                        bestScore = MathF.Min(bestScore, alphaScore);
                        beta = MathF.Min(beta, bestScore);
                        if (beta <= alpha)// 剪枝
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

            // 理论上不会出现这种情况
            if (crossWin && circleWin)
                return 0;
            // 调整cross和circle的分值来改变AI策略
            if (crossWin)
                return m_aiSide == Side.Cross ? m_power: -m_power;
            if (circleWin)
                return m_aiSide == Side.Circle ? m_power : -m_power;
            if (full = board.IsAllCellsOccupied())
                return 0;

            // 防止先手占据三个对角位置
            if (!board[1,1].IsOccupied && m_aiSide == turnSide)
            {
                return -m_power;
            }

            // 简单用棋子数差值来评分，可使用更复杂的算法处理得分数
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
