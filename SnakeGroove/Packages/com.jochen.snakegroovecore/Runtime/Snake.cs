using System;
using System.Collections.Generic;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Доменная сущность змейки. Хранит сегменты, направление и отложенный рост.
    /// </summary>
    public sealed class Snake
    {
        private readonly List<GridPosition> _segments;
        private Direction _currentDirection;
        private int _pendingGrowth;

        /// <summary>
        /// Создаёт новую змею с начальными позициями и направлением.
        /// </summary>
        /// <param name="initialPositions">Начальные позиции сегментов, голова первая.</param>
        /// <param name="initialDirection">Начальное направление движения.</param>
        public Snake(IEnumerable<GridPosition> initialPositions, Direction initialDirection)
        {
            if (initialPositions == null)
            {
                throw new ArgumentNullException(nameof(initialPositions));
            }

            _segments = new List<GridPosition>(initialPositions);
            if (_segments.Count == 0)
            {
                throw new ArgumentException("Змейка должна состоять как минимум из одного сегмента", nameof(initialPositions));
            }

            _currentDirection = initialDirection;
            _pendingGrowth = 0;
        }

        /// <summary>
        /// Список сегментов змейки (голова — индекс 0).
        /// </summary>
        public IReadOnlyList<GridPosition> Segments => _segments;

        /// <summary>
        /// Голова змейки.
        /// </summary>
        public GridPosition Head => _segments[0];

        /// <summary>
        /// Текущее направление.
        /// </summary>
        public Direction CurrentDirection => _currentDirection;

        /// <summary>
        /// Количество отложенных к добавлению сегментов (рост).
        /// </summary>
        public int PendingGrowth => _pendingGrowth;

        /// <summary>
        /// Выполнить шаг: добавить новую голову и удалить хвост если нет отложенного роста.
        /// </summary>
        public void Move()
        {
            var newHead = Head + _currentDirection.ToOffset();

            // вставляем новую голову в начале
            _segments.Insert(0, newHead);

            // если есть ожидаемый рост — уменьшаем счётчик, иначе удаляем хвост
            if (_pendingGrowth > 0)
            {
                _pendingGrowth--;
            }
            else
            {
                _segments.RemoveAt(_segments.Count - 1);
            }
        }

        /// <summary>
        /// Увеличить отложенный рост (будет применён при следующем Move).
        /// </summary>
        /// <param name="amount">Количество сегментов добавить.</param>
        public void Grow(int amount = 1)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Количество роста должно быть > 0");
            }

            _pendingGrowth += amount;
        }

        /// <summary>
        /// Изменить направление; игнорируем разворот на 180 градусов.
        /// </summary>
        /// <param name="newDirection">Новое направление.</param>
        public void ChangeDirection(Direction newDirection)
        {
            if (newDirection.IsOpposite(_currentDirection))
            {
                return; // запрещаем разворот на 180°
            }

            _currentDirection = newDirection;
        }
    }
}
