using System;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Представляет позицию в сетке с целочисленными координатами.
    /// </summary>
    public readonly struct GridPosition : IEquatable<GridPosition>
    {
        /// <summary>
        /// Координата X позиции в сетке.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Координата Y позиции в сетке.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Создаёт новую позицию в сетке.
        /// </summary>
        /// <param name="x">Координата X.</param>
        /// <param name="y">Координата Y.</param>
        public GridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Позиция (0, 0).
        /// </summary>
        public static GridPosition Zero { get; } = new GridPosition(0, 0);

        /// <summary>
        /// Оператор равенства для двух позиций.
        /// </summary>
        public static bool operator ==(GridPosition left, GridPosition right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Оператор неравенства для двух позиций.
        /// </summary>
        public static bool operator !=(GridPosition left, GridPosition right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Оператор сложения двух позиций (сложение координат).
        /// </summary>
        public static GridPosition operator +(GridPosition a, GridPosition b)
        {
            return new GridPosition(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Сравнение с другой позицией.
        /// </summary>
        public bool Equals(GridPosition other)
        {
            return X == other.X && Y == other.Y;
        }

        /// <summary>
        /// Переопределение Equals для общего объекта.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is GridPosition other && Equals(other);
        }

        /// <summary>
        /// Вычисляет хеш-код для позиции.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        /// <summary>
        /// Строковое представление позиции в виде "(X,Y)".
        /// </summary>
        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}
