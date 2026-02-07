using System;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Направления движения по сетке.
    /// </summary>
    public enum Direction
    {
        /// <summary>Вверх по Y.</summary>
        Up,

        /// <summary>Вниз по Y.</summary>
        Down,

        /// <summary>Влево по X.</summary>
        Left,

        /// <summary>Вправо по X.</summary>
        Right
    }

    /// <summary>
    /// Расширения для работы с Direction.
    /// </summary>
    public static class DirectionExtensions
    {
        /// <summary>
        /// Получить смещение в координатах сетки для направления.
        /// </summary>
        public static GridPosition ToOffset(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new GridPosition(0, 1);
                case Direction.Down:
                    return new GridPosition(0, -1);
                case Direction.Left:
                    return new GridPosition(-1, 0);
                case Direction.Right:
                    return new GridPosition(1, 0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        /// <summary>
        /// Проверяет, является ли другое направление противоположным.
        /// </summary>
        public static bool IsOpposite(this Direction direction, Direction other)
        {
            return (direction == Direction.Up && other == Direction.Down)
                   || (direction == Direction.Down && other == Direction.Up)
                   || (direction == Direction.Left && other == Direction.Right)
                   || (direction == Direction.Right && other == Direction.Left);
        }
    }
}
