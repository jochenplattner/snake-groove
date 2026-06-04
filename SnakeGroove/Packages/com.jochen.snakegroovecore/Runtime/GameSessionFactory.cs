using System;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Создаёт полностью собранные игровые сессии из проверенной конфигурации.
    /// </summary>
    public static class GameSessionFactory
    {
        /// <summary>
        /// Создаёт стандартную классическую сессию змейки.
        /// </summary>
        /// <param name="randomSeed">Необязательное зерно случайности для повторяемого создания еды.</param>
        public static GameSession CreateClassicDefault(int? randomSeed = null)
        {
            return Create(GameConfig.CreateClassicDefault(randomSeed));
        }

        /// <summary>
        /// Создаёт сессию с переданной конфигурацией.
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
