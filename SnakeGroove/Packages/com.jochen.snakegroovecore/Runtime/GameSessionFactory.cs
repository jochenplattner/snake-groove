using System;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Creates fully wired game sessions from validated configuration.
    /// </summary>
    public static class GameSessionFactory
    {
        /// <summary>
        /// Creates a classic default Snake session.
        /// </summary>
        public static GameSession CreateClassicDefault(int? randomSeed = null)
        {
            return Create(GameConfig.CreateClassicDefault(randomSeed));
        }

        /// <summary>
        /// Creates a session with the provided configuration.
        /// </summary>
        public static GameSession Create(GameConfig config, Random random = null)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var effectiveRandom = random ?? (config.RandomSeed.HasValue
                ? new Random(config.RandomSeed.Value)
                : new Random());

            var snake = new Snake(config.InitialSnakeSegments, config.InitialDirection);
            var spawner = new FoodSpawner(config.GridSize, effectiveRandom);

            if (!spawner.TrySpawn(snake.Segments, out var initialFood))
            {
                throw new InvalidOperationException("Cannot create a game session without a free food cell");
            }

            var state = new GameState(config.GridSize, snake, initialFood, config.InitialScore);
            var loop = new GameLoopService(state, spawner);
            return new GameSession(config, state, loop);
        }
    }
}
