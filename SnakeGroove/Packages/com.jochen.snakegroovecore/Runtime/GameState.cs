using System;
using System.Collections.Generic;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Хранит изменяемое доменное состояние запущенной игры.
    /// </summary>
    public sealed class GameState
    {
        /// <summary>
        /// Размер игровой сетки.
        /// </summary>
        public GridSize GridSize { get; }

        /// <summary>
        /// Змейка в текущем состоянии игры.
        /// </summary>
        public Snake Snake { get; }

        /// <summary>
        /// Текущая еда на поле. null, если уровень завершён.
        /// </summary>
        public Food Food { get; private set; }

        /// <summary>
        /// Текущий статус жизненного цикла.
        /// </summary>
        public GameStatus Status { get; private set; }

        /// <summary>
        /// true, если игрок проиграл.
        /// </summary>
        public bool IsGameOver => Status == GameStatus.GameOver;

        /// <summary>
        /// true, если поле полностью заполнено.
        /// </summary>
        public bool IsLevelComplete => Status == GameStatus.LevelComplete;

        /// <summary>
        /// Причина статуса проигрыша.
        /// </summary>
        public GameOverReason GameOverReason { get; private set; }

        /// <summary>
        /// Счёт игрока.
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Создаёт состояние игры с проверенными стартовыми данными.
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
        /// Создаёт снимок только для чтения для адаптеров и UI.
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
