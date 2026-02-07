namespace SnakeGroove.Core
{
    /// <summary>
    /// Интерфейс для объектов, которые можно съесть.
    /// </summary>
    public interface IEatable
    {
        /// <summary>
        /// Позиция съедобного объекта на сетке.
        /// </summary>
        GridPosition Position { get; }

        // TODO: добавить свойства для эффектов (очки, ускорение, замедление) в будущем
    }
}
