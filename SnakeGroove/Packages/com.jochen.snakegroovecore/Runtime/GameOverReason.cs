namespace SnakeGroove.Core
{
    /// <summary>
    /// Причина, по которой игрок проиграл.
    /// </summary>
    public enum GameOverReason
    {
        /// <summary>Игра ещё не завершена поражением.</summary>
        None,

        /// <summary>Змейка вышла за пределы поля.</summary>
        HitWall,

        /// <summary>Змейка столкнулась сама с собой.</summary>
        HitSelf
    }
}
