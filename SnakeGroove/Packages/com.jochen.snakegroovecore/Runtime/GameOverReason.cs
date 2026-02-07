namespace SnakeGroove.Core
{
    /// <summary>
    /// Причина окончания игры.
    /// </summary>
    public enum GameOverReason
    {
        /// <summary>Игра не окончена.</summary>
        None,

        /// <summary>Змейка врезалась в стену (вышла за границы поля).</summary>
        HitWall,

        /// <summary>Змейка врезалась сама в себя.</summary>
        HitSelf
    }
}
