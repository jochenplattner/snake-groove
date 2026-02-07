namespace SnakeGroove.Core
{
    /// <summary>
    /// Яблоко — стандартная еда для змейки.
    /// </summary>
    public sealed class Apple : Food
    {
        /// <summary>
        /// Создаёт яблоко в указанной позиции.
        /// </summary>
        /// <param name="position">Позиция яблока на сетке.</param>
        public Apple(GridPosition position) : base(position)
        {
        }

        // TODO: добавить количество очков за яблоко
        // TODO: возможно эффект роста (сколько сегментов добавлять)
    }
}
