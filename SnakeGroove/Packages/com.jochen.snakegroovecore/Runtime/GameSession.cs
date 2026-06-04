namespace SnakeGroove.Core
{
    /// <summary>
    /// Public facade for a running game session.
    /// </summary>
    public sealed class GameSession
    {
        private readonly GameLoopService _loop;

        /// <summary>
        /// Configuration used to create the session.
        /// </summary>
        public GameConfig Config { get; }

        internal GameState State { get; }

        /// <summary>
        /// Current read-only snapshot.
        /// </summary>
        public GameSnapshot Snapshot => State.CreateSnapshot();

        internal GameSession(GameConfig config, GameState state, GameLoopService loop)
        {
            Config = config;
            State = state;
            _loop = loop;
        }

        /// <summary>
        /// Advances the session by one domain tick.
        /// </summary>
        public GameTickResult Tick(Direction? inputDirection = null)
        {
            return _loop.Tick(inputDirection);
        }
    }
}
