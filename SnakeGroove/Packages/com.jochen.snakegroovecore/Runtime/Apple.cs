namespace SnakeGroove.Core
{
    /// <summary>
    /// Default food for the classic Snake MVP.
    /// </summary>
    public sealed class Apple : Food
    {
        /// <summary>
        /// Creates an apple at the given grid position.
        /// </summary>
        public Apple(GridPosition position)
            : base(position, scoreValue: 1, growthAmount: 1)
        {
        }
    }
}
