using NUnit.Framework;

namespace SnakeGroove.Core.Tests
{
    public sealed class GameSessionFactoryTests
    {
        [Test]
        public void CreateClassicDefault_ReturnsRunningSessionWithSnapshot()
        {
            var session = GameSessionFactory.CreateClassicDefault(randomSeed: 123);

            Assert.AreEqual(GameStatus.Running, session.Snapshot.Status);
            Assert.AreEqual(new GridSize(20, 20), session.Config.GridSize);
            Assert.AreEqual(3, session.Snapshot.SnakeSegments.Count);
            Assert.NotNull(session.Snapshot.Food);
        }

        [Test]
        public void GameConfig_RejectsOverlappingInitialSegments()
        {
            var grid = new GridSize(4, 4);

            Assert.Throws<System.ArgumentException>(() =>
                new GameConfig(
                    grid,
                    new[] { new GridPosition(1, 1), new GridPosition(1, 1) },
                    Direction.Right));
        }
    }
}
