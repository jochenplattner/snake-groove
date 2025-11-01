using System;

namespace Snake.Core
{
    public class SnakeGameOverException : Exception
    {
        public SnakeGameOverException() { }

        public SnakeGameOverException(string message) : base(message) { }

        public SnakeGameOverException(string message, Exception inner) : base(message, inner) { }
    }
}
