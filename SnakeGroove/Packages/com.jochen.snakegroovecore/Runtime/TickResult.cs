namespace SnakeGroove.Core
{
    /// <summary>
    /// High-level outcome of one game tick.
    /// </summary>
    public enum TickResult
    {
        /// <summary>The game continues without a special event.</summary>
        Continue,

        /// <summary>The snake ate food on this tick.</summary>
        AteFood,

        /// <summary>The snake lost on this tick.</summary>
        GameOver,

        /// <summary>The board is filled and the level is complete.</summary>
        LevelComplete
    }
}
