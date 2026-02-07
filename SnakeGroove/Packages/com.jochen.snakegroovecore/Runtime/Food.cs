namespace SnakeGroove.Core
{
    /// <summary>
    /// Базовый абстрактный класс еды.
    /// </summary>
    public abstract class Food : IEatable
    {
        /// <summary>
        /// Позиция еды на сетке.
        /// </summary>
        public GridPosition Position { get; }

        /// <summary>
        /// Создаёт еду в указанной позиции.
        /// </summary>
        /// <param name="position">Позиция еды на сетке.</param>
        protected Food(GridPosition position)
        {
            Position = position;
        }

        // TODO: добавить тип еды, эффект, количество очков в будущем
        // TODO: Lemon : Food — кислый, даёт меньше очков
        // TODO: Cherry : Food — даёт бонусные очки или ускорение
    }
}
