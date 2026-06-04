namespace SnakeGroove.Core
{
    /// <summary>
    /// Общее событие одного игрового тика.
    /// </summary>
    public enum TickResult
    {
        /// <summary>Игра продолжается без особого события.</summary>
        Continue,

        /// <summary>Змейка съела еду за этот тик.</summary>
        AteFood,

        /// <summary>Змейка проиграла за этот тик.</summary>
        GameOver,

        /// <summary>Поле заполнено, и уровень завершён.</summary>
        LevelComplete
    }
}
