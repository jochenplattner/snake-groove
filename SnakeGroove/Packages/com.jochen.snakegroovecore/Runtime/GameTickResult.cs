namespace SnakeGroove.Core
{
    /// <summary>
    /// Подробный результат, который возвращается после одного доменного тика.
    /// </summary>
    public sealed class GameTickResult
    {
        /// <summary>
        /// Общее событие, произошедшее за тик.
        /// </summary>
        public TickResult Outcome { get; }

        /// <summary>
        /// Очки, добавленные за этот тик.
        /// </summary>
        public int ScoreDelta { get; }

        /// <summary>
        /// Еда, съеденная за этот тик, если она была.
        /// </summary>
        public Food EatenFood { get; }

        /// <summary>
        /// Еда, созданная после этого тика, если она была.
        /// </summary>
        public Food SpawnedFood { get; }

        /// <summary>
        /// Причина результата проигрыша.
        /// </summary>
        public GameOverReason GameOverReason { get; }

        /// <summary>
        /// Снимок состояния после применения тика.
        /// </summary>
        public GameSnapshot Snapshot { get; }

        /// <summary>
        /// Текущий статус после тика.
        /// </summary>
        public GameStatus Status => Snapshot.Status;

        /// <summary>
        /// true, если тик завершил игру поражением.
        /// </summary>
        public bool IsGameOver => Outcome == TickResult.GameOver;

        /// <summary>
        /// true, если тик завершил уровень.
        /// </summary>
        public bool IsLevelComplete => Outcome == TickResult.LevelComplete;

        private GameTickResult(
            TickResult outcome,
            int scoreDelta,
            Food eatenFood,
            Food spawnedFood,
            GameOverReason gameOverReason,
            GameSnapshot snapshot)
        {
            Outcome = outcome;
            ScoreDelta = scoreDelta;
            EatenFood = eatenFood;
            SpawnedFood = spawnedFood;
            GameOverReason = gameOverReason;
            Snapshot = snapshot;
        }

        internal static GameTickResult Continue(GameSnapshot snapshot)
        {
            return new GameTickResult(TickResult.Continue, 0, null, null, GameOverReason.None, snapshot);
        }

        internal static GameTickResult AteFood(Food eatenFood, Food spawnedFood, int scoreDelta, GameSnapshot snapshot)
        {
            return new GameTickResult(TickResult.AteFood, scoreDelta, eatenFood, spawnedFood, GameOverReason.None, snapshot);
        }

        internal static GameTickResult GameOver(GameOverReason reason, GameSnapshot snapshot)
        {
            return new GameTickResult(TickResult.GameOver, 0, null, null, reason, snapshot);
        }

        internal static GameTickResult LevelComplete(Food eatenFood, int scoreDelta, GameSnapshot snapshot)
        {
            return new GameTickResult(TickResult.LevelComplete, scoreDelta, eatenFood, null, GameOverReason.None, snapshot);
        }
    }
}
