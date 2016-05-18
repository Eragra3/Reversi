using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.Logic
{
    struct IntPair : IEquatable<IntPair>
    {
        public int X;
        public int Y;

        public IntPair(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(IntPair other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = (hash * 23) + X;
                hash = (hash * 23) + Y;
                return hash * 7;
            }
        }
    }
}
