using System;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Applies one domain tick to a game state.
    /// </summary>
    public sealed class GameLoopService
    {
        private readonly GameState _state;
        private readonly FoodSpawner _spawner;

        /// <summary>
        /// Creates a game loop service.
        /// </summary>
        public GameLoopService(GameState state, FoodSpawner spawner)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
            _spawner = spawner ?? throw new ArgumentNullException(nameof(spawner));
        }

        /// <summary>
        /// Advances the game by one tick.
        /// </summary>
        public GameTickResult Tick(Direction? inputDirection = null)
        {
            if (_state.IsGameOver)
            {
                return GameTickResult.GameOver(_state.GameOverReason, _state.CreateSnapshot());
            }

            if (_state.IsLevelComplete)
            {
                return GameTickResult.LevelComplete(null, 0, _state.CreateSnapshot());
            }

            if (inputDirection.HasValue)
            {
                _state.Snake.ChangeDirection(inputDirection.Value);
            }

            var nextHead = _state.Snake.Head + _state.Snake.CurrentDirection.ToOffset();
            if (GameRules.IsOutsideBounds(nextHead, _state.GridSize))
            {
                _state.MarkGameOver(GameOverReason.HitWall);
                return GameTickResult.GameOver(_state.GameOverReason, _state.CreateSnapshot());
            }

            bool willEat = _state.Food != null && nextHead == _state.Food.Position;
            int growthAmount = willEat ? _state.Food.GrowthAmount : 0;
            bool allowTailPass = growthAmount == 0 && _state.Snake.PendingGrowth == 0;

            if (GameRules.IsSelfCollision(nextHead, _state.Snake.Segments, allowTailPass))
            {
                _state.MarkGameOver(GameOverReason.HitSelf);
                return GameTickResult.GameOver(_state.GameOverReason, _state.CreateSnapshot());
            }

            Food eatenFood = null;
            if (willEat)
            {
                eatenFood = _state.Food;
                if (growthAmount > 0)
                {
                    _state.Snake.Grow(growthAmount);
                }
            }

            _state.Snake.Move();

            if (!willEat)
            {
                return GameTickResult.Continue(_state.CreateSnapshot());
            }

            int scoreDelta = eatenFood.ScoreValue;
            _state.AddScore(scoreDelta);

            if (_spawner.TrySpawn(_state.Snake.Segments, out var spawnedFood))
            {
                _state.SetFood(spawnedFood);
                return GameTickResult.AteFood(eatenFood, spawnedFood, scoreDelta, _state.CreateSnapshot());
            }

            _state.ClearFood();
            _state.CompleteLevel();
            return GameTickResult.LevelComplete(eatenFood, scoreDelta, _state.CreateSnapshot());
        }
    }
}
