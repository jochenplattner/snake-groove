using System;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Base class for food items that can be placed on the game grid.
    /// </summary>
    public abstract class Food : IEatable
    {
        /// <inheritdoc />
        public GridPosition Position { get; }

        /// <inheritdoc />
        public int ScoreValue { get; }

        /// <inheritdoc />
        public int GrowthAmount { get; }

        /// <summary>
        /// Creates a food item with domain effects attached.
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
