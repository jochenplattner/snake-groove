using System;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Представляет размер игровой сетки (ширина и высота).
    /// </summary>
    public readonly struct GridSize : IEquatable<GridSize>
    {
        /// <summary>
        /// Ширина сетки (количество клеток по горизонтали).
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Высота сетки (количество клеток по вертикали).
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Создаёт размер сетки с указанными шириной и высотой.
        /// </summary>
        /// <param name="width">Ширина сетки (должна быть больше 0).</param>
        /// <param name="height">Высота сетки (должна быть больше 0).</param>
        /// <exception cref="ArgumentOutOfRangeException">Если ширина или высота меньше или равна 0.</exception>
        public GridSize(int width, int height)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), width, "Ширина должна быть больше 0");
            }

            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height), height, "Высота должна быть больше 0");
            }

            Width = width;
            Height = height;
        }

        /// <summary>
        /// Общее количество клеток в сетке.
        /// </summary>
        public int TotalCells => Width * Height;

        /// <summary>
        /// Сравнение с другим размером.
        /// </summary>
        public bool Equals(GridSize other)
        {
            return Width == other.Width && Height == other.Height;
        }

        /// <summary>
        /// Переопределение Equals для общего объекта.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is GridSize other && Equals(other);
        }

        /// <summary>
        /// Вычисляет хеш-код для размера.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Width * 397) ^ Height;
            }
        }

        /// <summary>
        /// Оператор равенства для двух размеров.
        /// </summary>
        public static bool operator ==(GridSize left, GridSize right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Оператор неравенства для двух размеров.
        /// </summary>
        public static bool operator !=(GridSize left, GridSize right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Строковое представление размера.
        /// </summary>
        public override string ToString()
        {
            return $"{Width}x{Height}";
        }
    }
}
