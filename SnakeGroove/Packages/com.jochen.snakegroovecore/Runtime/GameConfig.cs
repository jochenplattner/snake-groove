using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SnakeGroove.Core
{
    /// <summary>
    /// Immutable configuration used to create a valid game session.
    /// </summary>
    public sealed class GameConfig
    {
        private readonly ReadOnlyCollection<GridPosition> _initialSnakeSegments;

        /// <summary>
        /// Size of the game board.
        /// </summary>
        public GridSize GridSize { get; }

        /// <summary>
        /// Initial snake positions. The head is the first item.
        /// </summary>
        public IReadOnlyList<GridPosition> InitialSnakeSegments => _initialSnakeSegments;

        /// <summary>
        /// Initial movement direction.
        /// </summary>
        public Direction InitialDirection { get; }

        /// <summary>
        /// Domain tick speed hint for adapters.
        /// </summary>
        public int TicksPerSecond { get; }

        /// <summary>
        /// Initial player score.
        /// </summary>
        public int InitialScore { get; }

        /// <summary>
        /// Optional seed for deterministic food spawning.
        /// </summary>
        public int? RandomSeed { get; }

        /// <summary>
        /// Creates a configuration for a new Snake game.
        /// </summary>
        public GameConfig(
            GridSize gridSize,
            IEnumerable<GridPosition> initialSnakeSegments,
            Direction initialDirection,
            int ticksPerSecond = 8,
            int initialScore = 0,
            int? randomSeed = null)
        {
            if (initialSnakeSegments == null)
            {
                throw new ArgumentNullException(nameof(initialSnakeSegments));
            }

            if (!Enum.IsDefined(typeof(Direction), initialDirection))
            {
                throw new ArgumentOutOfRangeException(nameof(initialDirection), initialDirection, "Direction is not defined");
            }

            if (ticksPerSecond <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ticksPerSecond), ticksPerSecond, "Ticks per second must be > 0");
            }

            if (initialScore < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialScore), initialScore, "Initial score must be >= 0");
            }

            var segments = new List<GridPosition>(initialSnakeSegments);
            if (segments.Count == 0)
            {
                throw new ArgumentException("Initial snake must contain at least one segment", nameof(initialSnakeSegments));
            }

            if (segments.Count >= gridSize.TotalCells)
            {
                throw new ArgumentException("Initial snake must leave at least one free cell for food", nameof(initialSnakeSegments));
            }

            var unique = new HashSet<GridPosition>();
            for (int i = 0; i < segments.Count; i++)
            {
                var segment = segments[i];
                if (GameRules.IsOutsideBounds(segment, gridSize))
                {
                    throw new ArgumentException("Initial snake segment is outside the grid", nameof(initialSnakeSegments));
                }

                if (!unique.Add(segment))
                {
                    throw new ArgumentException("Initial snake segments must be unique", nameof(initialSnakeSegments));
                }
            }

            GridSize = gridSize;
            _initialSnakeSegments = new ReadOnlyCollection<GridPosition>(segments);
            InitialDirection = initialDirection;
            TicksPerSecond = ticksPerSecond;
            InitialScore = initialScore;
            RandomSeed = randomSeed;
        }

        /// <summary>
        /// Creates the default Phase 1 classic Snake configuration.
        /// </summary>
        public static GameConfig CreateClassicDefault(int? randomSeed = null)
        {
            var gridSize = new GridSize(20, 20);
            return new GameConfig(
                gridSize,
                new[]
                {
                    new GridPosition(10, 10),
                    new GridPosition(9, 10),
                    new GridPosition(8, 10)
                },
                Direction.Right,
                ticksPerSecond: 8,
                initialScore: 0,
                randomSeed: randomSeed);
        }
    }
}
