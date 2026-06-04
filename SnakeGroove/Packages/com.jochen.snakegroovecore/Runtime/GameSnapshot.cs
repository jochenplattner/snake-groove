using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Неизменяемое представление текущего состояния игры для адаптеров и UI.
    /// </summary>
    public sealed class GameSnapshot
    {
        private readonly ReadOnlyCollection<GridPosition> _snakeSegments;

        /// <summary>
        /// Размер игрового поля.
        /// </summary>
        public GridSize GridSize { get; }

        /// <summary>
        /// Позиции змейки. Первый элемент — голова.
        /// </summary>
        public IReadOnlyList<GridPosition> SnakeSegments => _snakeSegments;

        /// <summary>
        /// Текущее направление движения.
        /// </summary>
        public Direction CurrentDirection { get; }

        /// <summary>
        /// Текущая еда. null, если уровень завершён.
        /// </summary>
        public Food Food { get; }

        /// <summary>
        /// Текущий счёт игрока.
        /// </summary>
        public int Score { get; }

        /// <summary>
        /// Текущее состояние жизненного цикла игры.
        /// </summary>
        public GameStatus Status { get; }

        /// <summary>
        /// Причина статуса проигрыша.
        /// </summary>
        public GameOverReason GameOverReason { get; }

        /// <summary>
        /// true, если игрок проиграл.
        /// </summary>
        public bool IsGameOver => Status == GameStatus.GameOver;

        /// <summary>
        /// true, если поле полностью заполнено.
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
