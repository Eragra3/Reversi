using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reversi.Enums;

namespace Reversi.Logic
{
    public class PawnLightModel : IEquatable<PawnLightModel>
    {
        public int X;
        public int Y;
        public TileStateEnum State;

        public bool Equals(PawnLightModel other)
        {
            return X == other.X && Y == other.Y && State == other.State;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PawnLightModel)) return base.Equals(obj);

            var other = (PawnLightModel)obj;
            return X == other.X && Y == other.Y && State == other.State;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = (hash * 23) + X;
                hash = (hash * 23) + Y;
                hash = (hash * 23) + (int)State;
                return hash * 13;
            }
        }

        public override string ToString()
        {
            return $"X:{X},Y:{Y},{State}";
        }
    }
}
