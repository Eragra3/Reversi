using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reversi.CustomControls;
using Reversi.Enums;

namespace Reversi.Logic
{
    public abstract class ReversiPlayer
    {
        public abstract PawnLightModel FindNextMove(Tile[][] gameState);

        protected TileStateEnum[][] DeepCloneGameState(TileStateEnum[][] gameState)
        {

            return CopyArrayBuiltIn(gameState);

            //var gameStateCopy = new TileStateEnum[gameState.Length][];
            //for (var i = 0; i < gameStateCopy.Length; i++)
            //{
            //    gameStateCopy[i] = new TileStateEnum[gameStateCopy.Length];
            //    for (var j = 0; j < gameStateCopy[i].Length; j++)
            //    {
            //        gameStateCopy[i][j] = gameState[i][j];
            //    }
            //}

            //return gameStateCopy;
        }

        static TileStateEnum[][] CopyArrayBuiltIn(TileStateEnum[][] source)
        {
            var len = source.Length;
            var dest = new TileStateEnum[len][];

            for (var x = 0; x < len; x++)
            {
                var inner = source[x];
                var ilen = inner.Length;
                var newer = new TileStateEnum[ilen];
                Array.Copy(inner, newer, ilen);
                dest[x] = newer;
            }

            return dest;
        }
    }
}
