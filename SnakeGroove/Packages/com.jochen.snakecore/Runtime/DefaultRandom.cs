using System;

namespace Snake.Core
{
    public class DefaultRandom : IRandom
    {
        private readonly Random _rnd = new Random();

        public int Next(int minInclusive, int maxExclusive)
        {
            return _rnd.Next(minInclusive, maxExclusive);
        }
    }
}
