namespace SnakeGroove.Core
{
    /// <summary>
    /// Current lifecycle state of a game session.
    /// </summary>
    public enum GameStatus
    {
        /// <summary>The game is still running.</summary>
        Running,

        /// <summary>The player lost.</summary>
        GameOver,

        /// <summary>The board was filled and the level is complete.</summary>
        LevelComplete
    }
}
