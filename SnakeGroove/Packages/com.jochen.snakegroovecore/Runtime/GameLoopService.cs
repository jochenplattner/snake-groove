using System;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Сервис игрового цикла. Содержит логику одного тика игры.
    /// </summary>
    public sealed class GameLoopService
    {
        private readonly GameState _state;
        private readonly FoodSpawner _spawner;

        /// <summary>
        /// Создаёт сервис игрового цикла.
        /// </summary>
        /// <param name="state">Состояние игры.</param>
        /// <param name="spawner">Спавнер еды.</param>
        /// <exception cref="ArgumentNullException">Если state или spawner не заданы.</exception>
        public GameLoopService(GameState state, FoodSpawner spawner)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
            _spawner = spawner ?? throw new ArgumentNullException(nameof(spawner));
        }

        /// <summary>
        /// Выполняет один тик игры.
        /// Правила:
        /// - вычислить nextHead;
        /// - проверить выход за границы;
        /// - определить, съест ли змейка еду на этом тике;
        /// - вычислить allowTailPass (учитывая предстоящий рост);
        /// - проверить self-collision через GameRules;
        /// - если будет еда — вызвать Grow(1) ДО Move();
        /// - выполнить Move();
        /// - если была еда — увеличить Score и заспавнить новую еду.
        /// </summary>
        public TickResult Tick(Direction? inputDirection = null)
        {
            // 1) Если игра уже окончена — сразу вернуть GameOver.
            if (_state.IsGameOver) return TickResult.GameOver;

            // 2) Применить ввод пользователя: изменить направление змейки, если задано.
            if (inputDirection.HasValue) _state.Snake.ChangeDirection(inputDirection.Value);

            // 3) Вычислить следующую позицию головы (текущая голова + смещение по направлению).
            var nextHead = _state.Snake.Head + _state.Snake.CurrentDirection.ToOffset();

            // 4) Проверить выход за границы игрового поля — если да, установить GameOver с причиной HitWall.
            if (GameRules.IsOutsideBounds(nextHead, _state.GridSize))
            {
                _state.IsGameOver = true;
                _state.GameOverReason = GameOverReason.HitWall;
                return TickResult.GameOver;
            }

            // 5) Определить, будет ли съедена еда на этой позиции.
            bool willEat = _state.Food != null && nextHead == _state.Food.Position;
            // 6) Разрешить проход через хвост только если не будет еды и нет ожидаемого роста.
            bool allowTailPass = !willEat && _state.Snake.PendingGrowth == 0;

            // 7) Проверить самопересечение с учётом allowTailPass — в случае коллизии завершить игру.
            if (GameRules.IsSelfCollision(nextHead, _state.Snake.Segments, allowTailPass))
            {
                _state.IsGameOver = true;
                _state.GameOverReason = GameOverReason.HitSelf;
                return TickResult.GameOver;
            }

            // 8) Если будет еда — подготовить рост змейки (Grow) ДО перемещения, чтобы голова заняла клетку с едой.
            if (willEat) _state.Snake.Grow(1);

            // 9) Выполнить само перемещение (переместить голову, возможно убрать хвост).
            _state.Snake.Move();

            // 10) Если еда была съедена — увеличить счёт, заспавнить новую еду и вернуть AteFood.
            if (willEat)
            {
                _state.Score++;
                _state.Food = _spawner.Spawn(_state.Snake.Segments);
                return TickResult.AteFood;
            }

            // 11) В остальных случаях продолжать игру.
            return TickResult.Continue;
        }
    }
}
