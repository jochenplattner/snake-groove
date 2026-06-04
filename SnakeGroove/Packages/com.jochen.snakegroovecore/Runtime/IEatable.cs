namespace SnakeGroove.Core
{
    /// <summary>
    /// Represents a grid item that can be eaten by the snake.
    /// </summary>
    public interface IEatable
    {
        /// <summary>
        /// Position of the item on the grid.
        /// </summary>
        GridPosition Position { get; }

        /// <summary>
        /// Score added when the item is eaten.
        /// </summary>
        int ScoreValue { get; }

        /// <summary>
        /// Number of snake segments added when the item is eaten.
        /// </summary>
        int GrowthAmount { get; }
    }
}
