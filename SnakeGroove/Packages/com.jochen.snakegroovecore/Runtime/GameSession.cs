namespace SnakeGroove.Core
{
    /// <summary>
    /// Публичный фасад запущенной игровой сессии.
    /// </summary>
    public sealed class GameSession
    {
        private readonly GameLoopService _loop;

        /// <summary>
        /// Конфигурация, с которой была создана сессия.
        /// </summary>
        public GameConfig Config { get; }

        /// <summary>
        /// Внутреннее изменяемое состояние текущей игровой сессии.
        /// </summary>
        internal GameState State { get; }

        /// <summary>
        /// Текущий снимок состояния только для чтения.
        /// </summary>
        public GameSnapshot Snapshot => State.CreateSnapshot();

        internal GameSession(GameConfig config, GameState state, GameLoopService loop)
        {
            Config = config;
            State = state;
            _loop = loop;
        }

        /// <summary>
        /// Продвигает сессию на один доменный тик.
        /// </summary>
        public GameTickResult Tick(Direction? inputDirection = null)
        {
            return _loop.Tick(inputDirection);
        }
    }
}
