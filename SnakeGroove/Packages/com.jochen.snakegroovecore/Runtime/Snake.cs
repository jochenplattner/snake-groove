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
        /// Создаёт новую змейку с начальными позициями и направлением.
        /// </summary>
        /// <param name="initialPositions">Начальные позиции сегментов, голова первой.</param>
        /// <param name="initialDirection">Начальное направление движения.</param>
        /// <exception cref="ArgumentNullException">Если позиции не заданы.</exception>
        /// <exception cref="ArgumentException">Если список позиций пуст.</exception>
        public Snake(IEnumerable<GridPosition> initialPositions, Direction initialDirection)
        {
            if (initialPositions == null)
            {
                throw new ArgumentNullException(nameof(initialPositions));
            }

            _segments = new List<GridPosition>(initialPositions);
            if (_segments.Count == 0)
            {
                throw new ArgumentException("Змейка должна содержать как минимум один сегмент", nameof(initialPositions));
            }

            _currentDirection = initialDirection;
            _pendingGrowth = 0;
        }

        /// <summary>
        /// Список позиций сегментов змейки (голова — индекс 0).
        /// </summary>
        public IReadOnlyList<GridPosition> Segments => _segments.AsReadOnly();

        /// <summary>
        /// Позиция головы змейки.
        /// </summary>
        public GridPosition Head => _segments[0];

        /// <summary>
        /// Текущее направление движения змейки.
        /// </summary>
        public Direction CurrentDirection => _currentDirection;

        /// <summary>
        /// Количество отложенного роста (сегменты, которые будут добавлены при следующих ходах).
        /// </summary>
        public int PendingGrowth => _pendingGrowth;

        /// <summary>
        /// Двигает змейку на один шаг в текущем направлении.
        /// Если есть отложенный рост — хвост не удаляется.
        /// </summary>
        public void Move()
        {
            var newHead = Head + _currentDirection.ToOffset();

            // Вставляем новую голову в начало
            _segments.Insert(0, newHead);

            // Если есть отложенный рост — не удаляем хвост
            if (_pendingGrowth > 0)
            {
                _pendingGrowth--;
            }
            else
            {
                // Удаляем хвост
                _segments.RemoveAt(_segments.Count - 1);
            }
        }

        /// <summary>
        /// Добавляет отложенный рост (змейка вырастет на указанное количество сегментов).
        /// </summary>
        /// <param name="amount">Количество сегментов для роста (должно быть больше 0).</param>
        /// <exception cref="ArgumentOutOfRangeException">Если amount меньше или равен 0.</exception>
        public void Grow(int amount = 1)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Количество роста должно быть больше 0");
            }

            _pendingGrowth += amount;
        }

        /// <summary>
        /// Изменяет направление движения. Запрещает разворот на 180 градусов.
        /// </summary>
        /// <param name="newDirection">Новое направление.</param>
        public void ChangeDirection(Direction newDirection)
        {
            if (newDirection.IsOpposite(_currentDirection))
            {
                return; // игнорируем разворот на 180 градусов
            }

            _currentDirection = newDirection;
        }

        /// <summary>
        /// Проверяет, столкнётся ли змейка сама с собой, если голова переместится в указанную позицию.
        /// Учитывает, что если нет отложенного роста, хвост освободится при движении.
        /// </summary>
        /// <param name="nextHead">Позиция, куда переместится голова.</param>
        /// <returns>True, если произойдёт столкновение с телом.</returns>
        public bool WouldCollideWithSelf(GridPosition nextHead)
        {
            // Проверяем столкновение со всеми сегментами, кроме хвоста (если хвост освободится)
            int segmentsToCheck = _segments.Count;

            // Если нет отложенного роста, хвост освободится — не проверяем его
            if (_pendingGrowth == 0 && segmentsToCheck > 1)
            {
                segmentsToCheck--;
            }

            for (int i = 0; i < segmentsToCheck; i++)
            {
                if (_segments[i] == nextHead)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
