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
        /// </summary>
        /// <param name="inputDirection">Направление от игрока (null — без изменения направления).</param>
        /// <returns>Результат тика.</returns>
        public TickResult Tick(Direction? inputDirection = null)
        {
            // 1. Если игра окончена — сразу возвращаем GameOver
            if (_state.IsGameOver)
            {
                return TickResult.GameOver;
            }

            // 2. Обрабатываем ввод направления
            if (inputDirection.HasValue)
            {
                _state.Snake.ChangeDirection(inputDirection.Value);
            }

            // 3. Вычисляем следующую позицию головы
            var nextHead = _state.Snake.Head + _state.Snake.CurrentDirection.ToOffset();

            // 4. Проверяем выход за границы
            if (GameRules.IsOutsideBounds(nextHead, _state.GridSize))
            {
                _state.IsGameOver = true;
                _state.GameOverReason = GameOverReason.HitWall;
                return TickResult.GameOver;
            }

            // 5. Проверяем столкновение с собой
            // Если змейка НЕ растёт в этот тик, хвост освободится
            bool allowTailPass = _state.Snake.PendingGrowth == 0;
            if (GameRules.IsSelfCollision(nextHead, _state.Snake.Segments, allowTailPass))
            {
                _state.IsGameOver = true;
                _state.GameOverReason = GameOverReason.HitSelf;
                return TickResult.GameOver;
            }

            // 6. Выполняем движение
            _state.Snake.Move();

            // 7. Проверяем, съела ли змейка еду
            if (_state.Food.Position == _state.Snake.Head)
            {
                _state.Snake.Grow(1);
                _state.Food = _spawner.Spawn(_state.Snake.Segments);
                return TickResult.AteFood;
            }

            return TickResult.Continue;
        }

        // TODO: добавить подсчёт очков при поедании еды
        // TODO: добавить изменение скорости тика в зависимости от типа еды
        // TODO: добавить события для UI-слоя (OnFoodEaten, OnGameOver)
    }
}
