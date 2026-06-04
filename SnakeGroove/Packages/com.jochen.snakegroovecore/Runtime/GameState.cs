using System;
using System.Collections.Generic;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Holds the mutable domain state of a running game.
    /// </summary>
    public sealed class GameState
    {
        /// <summary>
        /// Size of the game grid.
        /// </summary>
        public GridSize GridSize { get; }

        /// <summary>
        /// Snake in the current game state.
        /// </summary>
        public Snake Snake { get; }

        /// <summary>
        /// Current food on the board. Null when the level is complete.
        /// </summary>
        public Food Food { get; private set; }

        /// <summary>
        /// Current lifecycle status.
        /// </summary>
        public GameStatus Status { get; private set; }

        /// <summary>
        /// True when the player lost.
        /// </summary>
        public bool IsGameOver => Status == GameStatus.GameOver;

        /// <summary>
        /// True when the board is filled.
        /// </summary>
        public bool IsLevelComplete => Status == GameStatus.LevelComplete;

        /// <summary>
        /// Reason for a GameOver status.
        /// </summary>
        public GameOverReason GameOverReason { get; private set; }

        /// <summary>
        /// Player score.
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Creates game state with validated starting data.
        /// </summary>
        public GameState(GridSize gridSize, Snake snake, Food initialFood, int initialScore = 0)
        {
            if (initialScore < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialScore), initialScore, "Initial score must be >= 0");
            }

            if (initialFood == null)
            {
                throw new ArgumentNullException(nameof(initialFood));
            }

            GridSize = gridSize;
            Snake = snake ?? throw new ArgumentNullException(nameof(snake));

            if (GameRules.IsOutsideBounds(initialFood.Position, gridSize))
            {
                throw new ArgumentException("Initial food is outside the grid", nameof(initialFood));
            }

            var occupied = new HashSet<GridPosition>();
            for (int i = 0; i < Snake.Segments.Count; i++)
            {
                var segment = Snake.Segments[i];
                if (GameRules.IsOutsideBounds(segment, gridSize))
                {
                    throw new ArgumentException("Initial snake segment is outside the grid", nameof(snake));
                }

                if (!occupied.Add(segment))
                {
                    throw new ArgumentException("Initial snake segments must be unique", nameof(snake));
                }

                if (segment == initialFood.Position)
                {
                    throw new ArgumentException("Initial food cannot overlap the snake", nameof(initialFood));
                }
            }

            Food = initialFood;
            Score = initialScore;
            Status = GameStatus.Running;
            GameOverReason = GameOverReason.None;
        }

        /// <summary>
        /// Creates a read-only snapshot for adapters and UI.
        /// </summary>
        public GameSnapshot CreateSnapshot()
        {
            return new GameSnapshot(this);
        }

        internal void AddScore(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Score amount must be >= 0");
            }

            Score += amount;
        }

        internal void SetFood(Food food)
        {
            if (food == null)
            {
                throw new ArgumentNullException(nameof(food));
            }

            if (GameRules.IsOutsideBounds(food.Position, GridSize))
            {
                throw new ArgumentException("Food is outside the grid", nameof(food));
            }

            Food = food;
        }

        internal void ClearFood()
        {
            Food = null;
        }

        internal void MarkGameOver(GameOverReason reason)
        {
            if (reason == GameOverReason.None)
            {
                throw new ArgumentException("Game over requires a concrete reason", nameof(reason));
            }

            Status = GameStatus.GameOver;
            GameOverReason = reason;
        }

        internal void CompleteLevel()
        {
            Status = GameStatus.LevelComplete;
            GameOverReason = GameOverReason.None;
        }
    }
}
