namespace Snake.Core
{
    public readonly struct GridPos
    {
        public int X { get; }
        public int Y { get; }

        public GridPos(int x, int y)
        {
            X = x;
            Y = y;
        }

        public GridPos Move(Direction direction)
        {
            return direction switch
            {
                Direction.Up => new GridPos(X, Y + 1),
                Direction.Down => new GridPos(X, Y - 1),
                Direction.Left => new GridPos(X - 1, Y),
                Direction.Right => new GridPos(X + 1, Y),
                _ => this
            };
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}
