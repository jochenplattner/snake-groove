using System;

using NUnit.Framework;

namespace SnakeGroove.Core.Tests
{
    public sealed class GameLoopServiceTests
    {
        [Test]
        public void Tick_WhenNoSpecialEvent_ReturnsContinueWithSnapshot()
        {
            var grid = new GridSize(4, 4);
            var snake = new Snake(new[] { new GridPosition(1, 1) }, Direction.Right);
            var state = new GameState(grid, snake, new Apple(new GridPosition(3, 3)));
            var loop = new GameLoopService(state, new FoodSpawner(grid, new Random(0)));

            var result = loop.Tick();

            Assert.AreEqual(TickResult.Continue, result.Outcome);
            Assert.AreEqual(GameStatus.Running, result.Status);
            Assert.AreEqual(new GridPosition(2, 1), result.Snapshot.SnakeSegments[0]);
            Assert.AreEqual(0, result.ScoreDelta);
        }

        [Test]
        public void Tick_WhenInputIsOppositeDirection_DoesNotReverseSnake()
        {
            var grid = new GridSize(5, 5);
            var snake = new Snake(
                new[]
                {
                    new GridPosition(2, 2),
                    new GridPosition(1, 2),
                    new GridPosition(0, 2)
                },
                Direction.Right);
            var state = new GameState(grid, snake, new Apple(new GridPosition(4, 4)));
            var loop = new GameLoopService(state, new FoodSpawner(grid, new Random(0)));

            var result = loop.Tick(Direction.Left);

            Assert.AreEqual(TickResult.Continue, result.Outcome);
            Assert.AreEqual(Direction.Right, result.Snapshot.CurrentDirection);
            Assert.AreEqual(new GridPosition(3, 2), result.Snapshot.SnakeSegments[0]);
        }

        [Test]
        public void Tick_WhenFoodIsAhead_ReturnsFoodDetailsAndUpdatesScore()
        {
            var grid = new GridSize(4, 4);
            var snake = new Snake(new[] { new GridPosition(1, 1) }, Direction.Right);
            var state = new GameState(grid, snake, new Apple(new GridPosition(2, 1)));
            var loop = new GameLoopService(state, new FoodSpawner(grid, new Random(0)));

            var result = loop.Tick();

            Assert.AreEqual(TickResult.AteFood, result.Outcome);
            Assert.AreEqual(1, result.ScoreDelta);
            Assert.AreEqual(1, result.Snapshot.Score);
            Assert.AreEqual(2, result.Snapshot.SnakeSegments.Count);
            Assert.NotNull(result.EatenFood);
            Assert.NotNull(result.SpawnedFood);
        }

        [Test]
        public void Tick_WhenFoodFillsLastCell_CompletesLevelWithoutSpawnerException()
        {
            var grid = new GridSize(2, 1);
            var snake = new Snake(new[] { new GridPosition(0, 0) }, Direction.Right);
            var state = new GameState(grid, snake, new Apple(new GridPosition(1, 0)));
            var loop = new GameLoopService(state, new FoodSpawner(grid, new Random(0)));

            var result = loop.Tick();

            Assert.AreEqual(TickResult.LevelComplete, result.Outcome);
            Assert.AreEqual(GameStatus.LevelComplete, result.Status);
            Assert.IsTrue(result.IsLevelComplete);
            Assert.IsNull(result.Snapshot.Food);
            Assert.AreEqual(1, result.Snapshot.Score);
            Assert.AreEqual(2, result.Snapshot.SnakeSegments.Count);
        }

        [Test]
        public void Tick_WhenSnakeHitsWall_ReturnsGameOverReason()
        {
            var grid = new GridSize(2, 2);
            var snake = new Snake(new[] { new GridPosition(1, 1) }, Direction.Right);
            var state = new GameState(grid, snake, new Apple(new GridPosition(0, 0)));
            var loop = new GameLoopService(state, new FoodSpawner(grid, new Random(0)));

            var result = loop.Tick();

            Assert.AreEqual(TickResult.GameOver, result.Outcome);
            Assert.AreEqual(GameOverReason.HitWall, result.GameOverReason);
            Assert.AreEqual(GameStatus.GameOver, result.Status);
        }

        [Test]
        public void Tick_WhenSnakeHitsItself_ReturnsGameOverHitSelf()
        {
            var grid = new GridSize(5, 4);
            var snake = new Snake(
                new[]
                {
                    new GridPosition(2, 2),
                    new GridPosition(1, 2),
                    new GridPosition(1, 1),
                    new GridPosition(2, 1),
                    new GridPosition(3, 1),
                    new GridPosition(3, 2),
                    new GridPosition(4, 2)
                },
                Direction.Right);
            var state = new GameState(grid, snake, new Apple(new GridPosition(0, 0)));
            var loop = new GameLoopService(state, new FoodSpawner(grid, new Random(0)));

            var result = loop.Tick();

            Assert.AreEqual(TickResult.GameOver, result.Outcome);
            Assert.AreEqual(GameStatus.GameOver, result.Status);
            Assert.AreEqual(GameOverReason.HitSelf, result.GameOverReason);
            Assert.AreEqual(GameOverReason.HitSelf, result.Snapshot.GameOverReason);
        }
    }
}
