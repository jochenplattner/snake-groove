namespace SnakeGroove.Core
{
    /// <summary>
    /// Результат выполнения одного тика игры.
    /// </summary>
    public enum TickResult
    {
        /// <summary>Игра продолжается без особых событий.</summary>
        Continue,

        /// <summary>Змейка съела еду.</summary>
        AteFood,

        /// <summary>Игра окончена.</summary>
        GameOver
    }
}
