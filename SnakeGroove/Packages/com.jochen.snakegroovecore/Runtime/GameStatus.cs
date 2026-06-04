namespace SnakeGroove.Core
{
    /// <summary>
    /// Текущее состояние жизненного цикла игровой сессии.
    /// </summary>
    public enum GameStatus
    {
        /// <summary>Игра продолжается.</summary>
        Running,

        /// <summary>Игрок проиграл.</summary>
        GameOver,

        /// <summary>Поле заполнено, и уровень завершён.</summary>
        LevelComplete
    }
}
