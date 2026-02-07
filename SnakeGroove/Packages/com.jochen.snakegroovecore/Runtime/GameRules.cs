using System.Collections.Generic;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Статический класс с чистыми проверками игровых правил (без состояния).
    /// </summary>
    public static class GameRules
    {
        /// <summary>
        /// Проверяет, находится ли позиция за пределами игровой сетки.
        /// </summary>
        /// <param name="pos">Позиция для проверки.</param>
        /// <param name="size">Размер игровой сетки.</param>
        /// <returns>True, если позиция за пределами сетки.</returns>
        public static bool IsOutsideBounds(GridPosition pos, GridSize size)
        {
            return pos.X < 0 || pos.Y < 0 || pos.X >= size.Width || pos.Y >= size.Height;
        }

        /// <summary>
        /// Проверяет столкновение головы с телом змейки.
        /// </summary>
        /// <param name="nextHead">Позиция, куда переместится голова.</param>
        /// <param name="segments">Текущие сегменты змейки.</param>
        /// <param name="allowTailPass">Если true — хвост (последний сегмент) не учитывается при проверке (он освободится).</param>
        /// <returns>True, если произойдёт столкновение.</returns>
        public static bool IsSelfCollision(GridPosition nextHead, IReadOnlyList<GridPosition> segments, bool allowTailPass)
        {
            if (segments == null || segments.Count == 0)
            {
                return false;
            }

            int segmentsToCheck = segments.Count;

            // Если хвост освободится — не проверяем его
            if (allowTailPass && segmentsToCheck > 1)
            {
                segmentsToCheck--;
            }

            for (int i = 0; i < segmentsToCheck; i++)
            {
                if (segments[i] == nextHead)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
