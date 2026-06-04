namespace SnakeGroove.Core
{
    /// <summary>
    /// Reason why the player lost.
    /// </summary>
    public enum GameOverReason
    {
        /// <summary>The game is not over.</summary>
        None,

        /// <summary>The snake moved outside the board.</summary>
        HitWall,

        /// <summary>The snake collided with itself.</summary>
        HitSelf
    }
}
