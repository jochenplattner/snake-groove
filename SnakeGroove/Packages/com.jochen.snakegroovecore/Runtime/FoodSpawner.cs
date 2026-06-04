using System;
using System.Collections.Generic;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Создаёт еду в свободных клетках игровой сетки.
    /// </summary>
    public sealed class FoodSpawner
    {
        private readonly GridSize _gridSize;
        private readonly Random _random;

        /// <summary>
        /// Создаёт генератор еды, удобный для детерминированных тестов.
        /// </summary>
        public FoodSpawner(GridSize gridSize, Random random)
        {
            _gridSize = gridSize;
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        /// <summary>
        /// Создаёт еду в случайной свободной клетке.
        /// </summary>
        /// <exception cref="InvalidOperationException">Возникает, если свободных клеток больше нет.</exception>
        public Food Spawn(IReadOnlyCollection<GridPosition> occupiedPositions)
        {
            if (TrySpawn(occupiedPositions, out var food))
            {
                return food;
            }

            throw new InvalidOperationException("No free cells available for food spawn");
        }

        /// <summary>
        /// Пытается создать еду в случайной свободной клетке.
        /// </summary>
        public bool TrySpawn(IReadOnlyCollection<GridPosition> occupiedPositions, out Food food)
        {
            if (occupiedPositions == null)
            {
                throw new ArgumentNullException(nameof(occupiedPositions));
            }

            var occupied = new HashSet<GridPosition>();
            foreach (var position in occupiedPositions)
            {
                if (GameRules.IsOutsideBounds(position, _gridSize))
                {
                    throw new ArgumentException("Occupied position is outside the grid", nameof(occupiedPositions));
                }

                occupied.Add(position);
            }

            int freeCells = _gridSize.TotalCells - occupied.Count;
            if (freeCells <= 0)
            {
                food = null;
                return false;
            }

            int targetFreeIndex = _random.Next(freeCells);
            int currentFreeIndex = 0;

            for (int y = 0; y < _gridSize.Height; y++)
            {
                for (int x = 0; x < _gridSize.Width; x++)
                {
                    var position = new GridPosition(x, y);
                    if (occupied.Contains(position))
                    {
                        continue;
                    }

                    if (currentFreeIndex == targetFreeIndex)
                    {
                        food = new Apple(position);
                        return true;
                    }

                    currentFreeIndex++;
                }
            }

            food = null;
            return false;
        }
    }
}
