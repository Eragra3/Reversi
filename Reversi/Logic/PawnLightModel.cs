using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reversi.Enums;

namespace Reversi.Logic
{
    public class PawnLightModel
    {
        public int X;
        public int Y;
        public Enums.TileStateEnum State;

        public void Flip()
        {
            switch (State)
            {
                case TileStateEnum.Black:
                    State = TileStateEnum.White;
                    break;
                case TileStateEnum.White:
                    State = TileStateEnum.Black;
                    break;
                case TileStateEnum.Empty:
                    Debug.WriteLine("Attempty to flip empty pawn");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public PawnLightModel Clone()
        {
            return new PawnLightModel
            {
                X = X,
                Y = Y,
                State = State
            };
        }
    }
}
