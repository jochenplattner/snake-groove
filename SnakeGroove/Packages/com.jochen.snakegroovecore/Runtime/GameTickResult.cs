namespace SnakeGroove.Core
{
    /// <summary>
    /// Rich result object returned after one domain tick.
    /// </summary>
    public sealed class GameTickResult
    {
        /// <summary>
        /// High-level tick outcome.
        /// </summary>
        public TickResult Outcome { get; }

        /// <summary>
        /// Score added on this tick.
        /// </summary>
        public int ScoreDelta { get; }

        /// <summary>
        /// Food eaten on this tick, if any.
        /// </summary>
        public Food EatenFood { get; }

        /// <summary>
        /// Food spawned after this tick, if any.
        /// </summary>
        public Food SpawnedFood { get; }

        /// <summary>
        /// Reason for a GameOver outcome.
        /// </summary>
        public GameOverReason GameOverReason { get; }

        /// <summary>
        /// Snapshot after the tick was applied.
        /// </summary>
        public GameSnapshot Snapshot { get; }

        /// <summary>
        /// Current status after the tick.
        /// </summary>
        public GameStatus Status => Snapshot.Status;

        /// <summary>
        /// True when the tick ended the game with a loss.
        /// </summary>
        public bool IsGameOver => Outcome == TickResult.GameOver;

        /// <summary>
        /// True when the tick completed the level.
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
