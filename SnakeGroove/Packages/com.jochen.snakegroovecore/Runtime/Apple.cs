namespace SnakeGroove.Core
{
    /// <summary>
    /// Еда по умолчанию для классической версии змейки.
    /// </summary>
    public sealed class Apple : Food
    {
        /// <summary>
        /// Создаёт яблоко в указанной позиции сетки.
        /// </summary>
        public Apple(GridPosition position)
            : base(position, scoreValue: 1, growthAmount: 1)
        {
        }
    }
}
