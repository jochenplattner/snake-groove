namespace SnakeGroove.Core
{
    /// <summary>
    /// Представляет объект на сетке, который змейка может съесть.
    /// </summary>
    public interface IEatable
    {
        /// <summary>
        /// Позиция объекта на сетке.
        /// </summary>
        GridPosition Position { get; }

        /// <summary>
        /// Очки, которые добавляются при поедании объекта.
        /// </summary>
        int ScoreValue { get; }

        /// <summary>
        /// Количество сегментов, которое добавляется змейке при поедании объекта.
        /// </summary>
        int GrowthAmount { get; }
    }
}
