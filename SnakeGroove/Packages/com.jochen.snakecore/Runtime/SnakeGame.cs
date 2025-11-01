using System;
using System.Collections.Generic;

namespace Snake.Core
{
    public class SnakeGame
    {
        public readonly int Width;
        public readonly int Height;

        public int Score => _snake.Count -1;
        //public event Action<int> OnScoreChanged;

        
        private readonly LinkedList<GridPos> _snake = new LinkedList<GridPos>();
        private readonly IRandom _random;
        
        public Direction Direction { get; private set; }
        public GridPos Food { get; private set; }
        public bool IsGameOver { get; private set; }
        
        public IEnumerable<GridPos> SnakePositions => _snake;
        
        public SnakeGame(int width, int height, IRandom random = null)
        {
            if (width <=0 || height <=0) throw new ArgumentException("Width and Height must be positive");
            Width = width;
            Height = height;
            _random = random ?? new DefaultRandom();
            Reset();
        }
        
        public void Reset()
        {
            _snake.Clear();
            var start = new GridPos(Width/2, Height/2);
            _snake.AddFirst(start);
            Direction = Direction.Right;
            IsGameOver = false;
            PlaceFood();
        }
        
        private void PlaceFood()
        {
            // naive placement: choose random free cell
            var max = Width * Height;
            var idx = _random.Next(0, max);
            for (int i =0; i < max; i++)
            {
                var linear = (idx + i) % max;
                var x = linear % Width;
                var y = linear / Width;
                var pos = new GridPos(x, y);
                if (!Contains(pos))
                {
                    Food = pos;
                    return;
                }
            }
        
            // no space left
            throw new SnakeGameOverException("No space to place food");
        }
        
        public void ChangeDirection(Direction dir)
        {
            // prevent reversing directly
            if ((_snake.Count >1) && IsOpposite(dir, Direction)) return;
            Direction = dir;
        }
        
        private static bool IsOpposite(Direction a, Direction b)
        {
            return (a == Direction.Up && b == Direction.Down) ||
            (a == Direction.Down && b == Direction.Up) ||
            (a == Direction.Left && b == Direction.Right) ||
            (a == Direction.Right && b == Direction.Left);
        }
        
        public void Tick()
        {
            // 1) Если игра уже окончена — прекращаем ход и сигнализируем об ошибке
            if (IsGameOver) throw new SnakeGameOverException("Game is over");

            // 2) Получаем текущую позицию головы змеи
            var head = _snake.First.Value;

            // 3) Вычисляем следующую позицию головы в направлении движения
            var next = head.Move(Direction);

            // 4) Проверка на столкновение со стеной (выход за границы поля)
            if (next.X <0 || next.Y <0 || next.X >= Width || next.Y >= Height)
            {
                IsGameOver = true;
                throw new SnakeGameOverException("Hit wall");
            }
            
            // 5) Проверка столкновения с собственным телом.
            //    Исключаем последний сегмент (хвост), потому что он может быть удалён в этом ходу —
            //    переход на клетку хвоста допустим, если змейка не растёт.
            bool willEat = (next.X == Food.X && next.Y == Food.Y);
            if (Contains(next) && !next.Equals(_snake.Last.Value) )
            {
                IsGameOver = true;
                throw new SnakeGameOverException("Hit self");
            }
            
            // 6) Добавляем новую голову в список позиций змеи
            _snake.AddFirst(next);

            // 7) Если на новой позиции была еда — змейка растёт (не удаляем хвост) и размещаем новую еду
            if (next.X == Food.X && next.Y == Food.Y)
            {
                // eat
                PlaceFood();
            }
            else
            {
                // 8) Если еды нет — сдвигаем змейку: удаляем последний сегмент (хвост)
                _snake.RemoveLast();
            }
        }
        
        private bool Contains(GridPos pos)
        {
            foreach (var p in _snake) if (p.X == pos.X && p.Y == pos.Y) return true;
            return false;
        }
    }
}
