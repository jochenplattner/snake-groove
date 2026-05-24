using System;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Хранит состояние игры. Содержит ТОЛЬКО данные, без логики тика.
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
        /// Текущая еда на поле.
        /// </summary>
        public Food Food { get; set; }

        /// <summary>
        /// Признак окончания игры.
        /// </summary>
        public bool IsGameOver { get; set; }

        /// <summary>
        /// Причина окончания игры.
        /// </summary>
        public GameOverReason GameOverReason { get; set; }

        /// <summary>
        /// Счёт игрока.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Создаёт состояние игры с заданными параметрами.
        /// </summary>
        public GameState(GridSize gridSize, Snake snake, Food initialFood)
        {
            GridSize = gridSize;
            Snake = snake ?? throw new ArgumentNullException(nameof(snake));
            Food = initialFood ?? throw new ArgumentNullException(nameof(initialFood));
            IsGameOver = false;
            GameOverReason = GameOverReason.None;
            Score = 0;
        }

        // TODO: добавить GameConfig (скорость тика, стартовая длина, seed для Random)
    }
}
