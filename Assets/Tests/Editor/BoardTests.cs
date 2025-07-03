using NUnit.Framework;
using UnityEngine;
using TicTacToe;

namespace Tests
{
    public class BoardTests
    {
        private Board board;
        private TicTacToe.Block cross;

        [SetUp]
        public void Setup()
        {
            cross = new TicTacToeBlock(Side.Cross);

            (Cell, TicTacToe.Block)[] blocks =
            {
                (new Cell(0, 0), new TicTacToeBlock()),
                (new Cell(0, 1), new TicTacToeBlock()),
                (new Cell(0, 2), new TicTacToeBlock()),
                (new Cell(1, 0), new TicTacToeBlock()),
                (new Cell(1, 1), new TicTacToeBlock()),
                (new Cell(1, 2), new TicTacToeBlock()),
                (new Cell(2, 0), new TicTacToeBlock()),
                (new Cell(2, 1), new TicTacToeBlock()),
                (new Cell(2, 2), new TicTacToeBlock()),
            };
            board = new Board(blocks);
        }

        [Test]
        public void Board_Initialization()
        {
            var board = new Board();
            Assert.AreEqual(9, board.GetBlocks().Count);
            Assert.IsFalse(board[0, 0].IsOccupied);
            Assert.IsFalse(board[0, 1].IsOccupied);
            Assert.IsFalse(board[0, 2].IsOccupied);
            Assert.IsFalse(board[1, 0].IsOccupied);
            Assert.IsFalse(board[1, 1].IsOccupied);
            Assert.IsFalse(board[1, 2].IsOccupied);
            Assert.IsFalse(board[2, 0].IsOccupied);
            Assert.IsFalse(board[2, 1].IsOccupied);
            Assert.IsFalse(board[2, 2].IsOccupied);
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(0, 2)]
        [TestCase(1, 0)]
        [TestCase(1, 2)] // passing
        [TestCase(2, 1)]
        [TestCase(2, 2)]
        public void Placement_Block_Normal(int row, int col)
        {
            Assert.IsTrue(board.IsOccupiedBySide(row, col, Side.None));

            // place a cross block at (row, col)
            Placement placement = new Placement(new Cell(row, col), new TicTacToeBlock(Side.Cross));
            board.PlaceBlock(GameContext.DefaultContext, placement);

            Assert.IsTrue(board.IsOccupiedBySide(row, col, Side.Cross));
            Assert.IsFalse(board.IsOccupiedBySide(row, col, Side.Circle));
            Assert.IsFalse(board.IsOccupiedBySide(row, col, Side.None));
        }

        [Test]
        [TestCase(99, 99)] // Invalid case
        public void TextInvalideBlock_WithExceptioin(int row, int col)
        {
            var board = new Board();

            Assert.IsFalse(board.IsValidPosition(row, col));

            var invalidPlacement = new Placement(new Cell(row, col));
            var what = Assert.Throws<System.ArgumentOutOfRangeException>(() =>
            {
                board.PlaceBlock(GameContext.DefaultContext, invalidPlacement);
            });
            Assert.That(what.Message, Does.Contain("Cell is out of bounds."));
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        public void TestNoOccupiedBySide(int row, int col)
        {
            var board = new Board();
            board[new Cell(row, col)] = new TicTacToeBlock(Side.Cross);
            Assert.IsTrue(board.IsOccupiedBySide(row, col, Side.Cross));

            // replace the block at (row, co) with an empty block
            board[row, col] = new TicTacToeBlock();
            Assert.IsTrue(board.IsOccupiedBySide(row, col, Side.None));
            Assert.IsFalse(board.IsOccupiedBySide(row, col, Side.Cross));
            Assert.IsFalse(board.IsOccupiedBySide(row, col, Side.Circle));
        }

        [Test]
        public void TestIsAllCellsOccupied()
        {
            for (int i = 0; i < Board.Size; i++)
            {
                for (int j = 0; j < Board.Size; j++)
                {
                    board[i, j] = new TicTacToeBlock(Side.Circle);
                }
            }
            Assert.IsTrue(board.IsAllCellsOccupied());

            for (int i = 0; i < Board.Size; i++)
            {
                for (int j = 0; j < Board.Size; j++)
                {
                    board.PlaceBlock(GameContext.DefaultContext, new Placement(new Cell(i, j)));
                }
            }

            Assert.IsTrue(board.IsAllCellsOccupied());
        }

        [Test]
        public void TestClearBoard()
        {
            board.Clear();
            Assert.AreEqual(Board.Size * Board.Size, board.GetBlocks().Count);

            for (int i = 0; i < Board.Size; i++)
            {
                for (int j = 0; j < Board.Size; j++)
                {
                    Assert.IsNull(board[i, j]);
                }
            }

            var placement = new Placement(new Cell(1,2));
            var what = Assert.Throws<System.InvalidOperationException>(() =>
            {
                board.PlaceBlock(GameContext.DefaultContext, placement);
            });
            Assert.That(what.Message, Does.Contain("not initialized"));
        }
    }
}