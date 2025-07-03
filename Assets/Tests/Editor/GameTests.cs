using NUnit.Framework;
using TicTacToe;
using UnityEngine;

namespace Tests
{
    public class GameTests
    {
        private Game game;

        private static Board CreateBoard(int occupied)
        {
            var board = new Board();

            for (int i = 0; i < occupied; i++)
            {
                int row = i / Board.Size;
                int col = i % Board.Size;
                board[new Cell(row, col)] = new TicTacToeBlock(Side.Cross);
            }

            return board;
        }

        [SetUp]
        public void Setup()
        {
            game = new Game();
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(8)]
        [TestCase(9)]
        public void TestPlaceableBlock(int numOfoccupied)
        {
            Board board = CreateBoard(numOfoccupied);
            Debug.Log($"Total Board blocks: {board.GetBlocks().Count}");
            foreach (var block in board.GetBlocks())
                Debug.Log($"Block: {block} Side: {block.Owner}");

            var results = Game.GetLegalPlacements(board, game.CurrentContext);

            // placeable blocks should be 9 - numOfoccupied
            if (numOfoccupied != 9)
            {
                int i = 0;
                foreach (var (block, result) in results)
                {
                    i++;
                    Debug.Log($"{i}/{results.Count}.Block: {block}");
                    foreach (var placement in result)
                    {
                        Debug.Log($"Placement: {placement.Value.Start} Block: {placement.Value.Block}");
                    }
                    Debug.Log("=================================");
                }
            }

            if (numOfoccupied == 9)
            {
                Assert.IsNull(results);
                Debug.Log($"Total placements found: 0");
            }
            else
            {
                Debug.Log($"Total placements found: {results.Count}");
                Assert.IsNotNull(results);
                Assert.AreEqual(results.Count, 9 - numOfoccupied);
            }
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(0, 2)]
        [TestCase(1, 0)]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        public void TestExecutePlacement(int row, int col)
        {
            Placement placement = new Placement(new Cell(row, col));
            game = new Game();
            game.ExecutePlacement(placement);
            Debug.Log($"Turn Side: {game.CurrentContext.TurnSide}, Turn Number: {game.CurrentContext.TurnNumber}");
            Board board = game.BoardHistory[^1];
            Assert.IsNotNull(board);
            Assert.IsTrue(board.IsOccupiedBySide(row, col, Side.Cross), 
                $"Expected cell ({row}, {col}) to be occupied by PlayerA, but it was not.");
        }

        [Test]
        public void TestGameContext()
        {
            var board = new Board();
            var context = GameContext.DefaultContext;
            for (int i = 1; i < 10; i++)
            {
                context = context.Advance(board);
                Debug.Log($"Turn {i}: Side: {context.TurnSide}, TurnNumber: {context.TurnNumber}");
            }
        }

        //see: Util.StringToCell
        //"row+column"
        [Test]
        [TestCase("00", "01", "02")]
        [TestCase("10", "11", "12")]
        //[TestCase("20", "21", "22")]
        [TestCase("00", "11", "22")]
        //[TestCase("02", "11", "20")]
        public void TestGame_Win(string cell1, string cell2, string cell3)
        {
            var game = new Game();

            Placement placement1 = new Placement(new Cell(cell1));
            Placement placement2 = new Placement(new Cell(cell2));
            Placement placement3 = new Placement(new Cell(cell3));
            
            Assert.IsTrue(game.ExecutePlacement(placement1)); // A
            Assert.IsTrue(game.ExecutePlacement(new Placement(new Cell(2, 0)))); // B
            Assert.IsTrue(game.ExecutePlacement(placement2)); // A
            Assert.IsTrue(game.ExecutePlacement(new Placement(new Cell(2, 1)))); // B
            Assert.IsTrue(game.ExecutePlacement(placement3)); // A

            Assert.AreEqual(6, game.BoardHistory.Count, "Expected 6 board states: initial + 5 placements");

            bool ret = Rules.IsContiguousBlocks(placement3.Start, game.CurrentBoard, Side.Cross);
            Assert.IsTrue(ret, "Expected the last placement to create a winning chain.");

            for (int i = 0; i < 9; i++)
            {
                int row = i / Board.Size;
                int col = i % Board.Size;
                Debug.Log($"Checking cell ({row}, {col}) for {game.CurrentBoard[row, col].ToString()} occupation.");
            }
        }

        [Test]
        public void TestGame_State()
        {
            var game = new Game();

            Placement placement1 = new Placement(new Cell(0, 0));
            Placement placement2 = new Placement(new Cell(0, 1));
            Placement placement3 = new Placement(new Cell(0, 2));
            game.ExecutePlacement(placement1); // A
            game.ExecutePlacement(new Placement(new Cell(2, 0))); // B
            game.ExecutePlacement(placement2); // A
            game.ExecutePlacement(new Placement(new Cell(2, 1))); // B
            game.ExecutePlacement(placement3); // A

            Assert.AreEqual(6, game.BoardHistory.Count, "Expected 6 board states: initial + 5 placements");

            foreach (var state in game.GameStates)
            {
                Debug.Log($"GameState: Start: {state.Cell}, Block: {state.Block}, Placement: {state.Placement}");
                Debug.Log($"Win: {state.CausedWin}, Draw: {state.CausedDraw}");
            }
        }
    }
}
