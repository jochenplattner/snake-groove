using System;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Базовый класс еды, которую можно разместить на игровой сетке.
    /// </summary>
    public abstract class Food : IEatable
    {
        /// Позиция еды на сетке.
        public GridPosition Position { get; }

        /// Количество очков, которое добавляется при поедании.
        public int ScoreValue { get; }

        /// Количество сегментов, на которое вырастет змейка.
        public int GrowthAmount { get; }

        /// <summary>
        /// Создаёт еду с привязанными доменными эффектами.
        /// </summary>
        protected Food(GridPosition position, int scoreValue, int growthAmount)
        {
            if (scoreValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scoreValue), scoreValue, "Score value must be >= 0");
            }

            if (growthAmount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(growthAmount), growthAmount, "Growth amount must be >= 0");
            }

            Position = position;
            ScoreValue = scoreValue;
            GrowthAmount = growthAmount;
        }
    }
}
