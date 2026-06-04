using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Immutable view of the current game state for adapters and UI.
    /// </summary>
    public sealed class GameSnapshot
    {
        private readonly ReadOnlyCollection<GridPosition> _snakeSegments;

        /// <summary>
        /// Size of the game board.
        /// </summary>
        public GridSize GridSize { get; }

        /// <summary>
        /// Snake positions. The head is the first item.
        /// </summary>
        public IReadOnlyList<GridPosition> SnakeSegments => _snakeSegments;

        /// <summary>
        /// Current movement direction.
        /// </summary>
        public Direction CurrentDirection { get; }

        /// <summary>
        /// Current food. Null when the level is complete.
        /// </summary>
        public Food Food { get; }

        /// <summary>
        /// Current player score.
        /// </summary>
        public int Score { get; }

        /// <summary>
        /// Current game lifecycle state.
        /// </summary>
        public GameStatus Status { get; }

        /// <summary>
        /// Reason for a GameOver status.
        /// </summary>
        public GameOverReason GameOverReason { get; }

        /// <summary>
        /// True when the player lost.
        /// </summary>
        public bool IsGameOver => Status == GameStatus.GameOver;

        /// <summary>
        /// True when the board is filled.
        /// </summary>
        public bool IsLevelComplete => Status == GameStatus.LevelComplete;

        internal GameSnapshot(GameState state)
        {
            GridSize = state.GridSize;
            _snakeSegments = new ReadOnlyCollection<GridPosition>(new List<GridPosition>(state.Snake.Segments));
            CurrentDirection = state.Snake.CurrentDirection;
            Food = state.Food;
            Score = state.Score;
            Status = state.Status;
            GameOverReason = state.GameOverReason;
        }
    }
}
