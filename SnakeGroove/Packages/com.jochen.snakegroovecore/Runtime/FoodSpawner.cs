using System;
using System.Collections.Generic;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Отвечает за создание еды в свободных клетках сетки.
    /// </summary>
    public sealed class FoodSpawner
    {
        private readonly GridSize _gridSize;
        private readonly Random _random;

        /// <summary>
        /// Создаёт спавнер еды.
        /// </summary>
        /// <param name="gridSize">Размер игровой сетки.</param>
        /// <param name="random">Генератор случайных чисел (для детерминизма можно передать с фиксированным seed).</param>
        public FoodSpawner(GridSize gridSize, Random random)
        {
            _gridSize = gridSize;
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        /// <summary>
        /// Создаёт еду в случайной свободной клетке.
        /// </summary>
        /// <param name="occupiedPositions">Занятые позиции (например, сегменты змейки).</param>
        /// <returns>Новая еда в свободной позиции.</returns>
        /// <exception cref="InvalidOperationException">Если свободных клеток нет.</exception>
        public Food Spawn(IReadOnlyCollection<GridPosition> occupiedPositions)
        {
            if (occupiedPositions == null)
            {
                throw new ArgumentNullException(nameof(occupiedPositions));
            }

            // Собираем занятые позиции в HashSet для быстрого поиска
            var occupied = new HashSet<GridPosition>(occupiedPositions);

            // Подсчитываем количество свободных клеток
            int totalCells = _gridSize.TotalCells;
            int freeCells = totalCells - occupied.Count;

            if (freeCells <= 0)
            {
                throw new InvalidOperationException("Нет свободных клеток для спавна еды");
            }

            // Выбираем случайную свободную клетку
            int targetFreeIndex = _random.Next(freeCells);
            int currentFreeIndex = 0;

            for (int y = 0; y < _gridSize.Height; y++)
            {
                for (int x = 0; x < _gridSize.Width; x++)
                {
                    var pos = new GridPosition(x, y);
                    if (!occupied.Contains(pos))
                    {
                        if (currentFreeIndex == targetFreeIndex)
                        {
                            // TODO: поддержка разных типов еды (Apple/Lemon) и весов спавна
                            return new Apple(pos);
                        }
                        currentFreeIndex++;
                    }
                }
            }

            // Не должно произойти при корректных данных
            throw new InvalidOperationException("Не удалось найти свободную клетку для спавна еды");
        }
    }
}
