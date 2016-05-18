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
        public AIStrategies CurrentStrategy { get; set; }

        public abstract AiPlayerResult FindNextMove(Tile[][] gameState);

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

        //protected int GetStablePawnsCount(TileStateEnum[][] board, TileStateEnum currentPlayerColor)
        //{
        //    var enemyPlayerColor = currentPlayerColor == TileStateEnum.Black ? TileStateEnum.White : TileStateEnum.Black;

        //    var currentPlayerCapturedPawns = new HashSet<IntPair>();
        //    var enemyPawns = new List<IntPair>(board.Length * board.Length);
        //    var playerPawns = new List<IntPair>(board.Length * board.Length);

        //    for (int i = 0; i < board.Length; i++)
        //    {
        //        for (int j = 0; j < board[i].Length; j++)
        //        {
        //            if (board[i][j] == enemyPlayerColor)
        //            {
        //                enemyPawns.Add(new IntPair(i, j));
        //            }
        //            else if (board[i][j] == currentPlayerColor)
        //            {
        //                playerPawns.Add(new IntPair(i, j));
        //            }
        //        }
        //    }

        //    foreach (var enemyPawn in enemyPawns)
        //    {
        //        var pawnX = enemyPawn.X;
        //        var pawnY = enemyPawn.Y;

        //        //W
        //        if (pawnX - 2 >= 0)
        //        {
        //            var currentTile = board[pawnX - 1][pawnY];
        //            var curX = pawnX - 1;
        //            var curY = pawnY;

        //            var lastTileInSequence = currentTile;
        //            var lastX = pawnX;
        //            var lastY = pawnY;

        //            while (lastTileInSequence == enemyPlayerColor)
        //            {
        //                if (lastX - 1 < 0) break;
        //                lastX--;
        //                lastTileInSequence = board[lastX][lastY];
        //            }

        //            if (lastTileInSequence == currentPlayerColor)
        //                while (currentTile == enemyPlayerColor)
        //                {
        //                    currentPlayerCapturedPawns.Add(new IntPair(curX, curY));

        //                    if (curX - 1 < 0) break;
        //                    curX--;
        //                    currentTile = board[curX][curY];
        //                }
        //        }
        //        //E
        //        if (pawnX + 2 < board.Length)
        //        {
        //            var currentTile = board[pawnX + 1][pawnY];
        //            var curX = pawnX + 1;
        //            var curY = pawnY;

        //            var lastTileInSequence = currentTile;
        //            var lastX = pawnX;
        //            var lastY = pawnY;
        //            while (lastTileInSequence == enemyPlayerColor)
        //            {
        //                if (lastX + 1 >= board.Length) break;
        //                lastX++;
        //                lastTileInSequence = board[lastX][lastY];
        //            }

        //            if (lastTileInSequence == currentPlayerColor)
        //                while (currentTile == enemyPlayerColor)
        //                {
        //                    currentPlayerCapturedPawns.Add(new IntPair(curX, curY));

        //                    if (curX + 1 >= board.Length) break;

        //                    curX++;
        //                    currentTile = board[curX][curY];
        //                }
        //        }
        //        //N
        //        if (pawnY - 2 >= 0)
        //        {
        //            var currentTile = board[pawnX][pawnY - 1];
        //            var curX = pawnX;
        //            var curY = pawnY - 1;

        //            var lastTileInSequence = currentTile;
        //            var lastX = pawnX;
        //            var lastY = pawnY;
        //            while (lastTileInSequence == enemyPlayerColor)
        //            {
        //                if (lastY - 1 < 0) break;
        //                lastY--;
        //                lastTileInSequence = board[lastX][lastY];
        //            }

        //            if (lastTileInSequence == currentPlayerColor)
        //                while (currentTile == enemyPlayerColor)
        //                {
        //                    currentPlayerCapturedPawns.Add(new IntPair(curX, curY));

        //                    if (curY - 1 < 0) break;
        //                    curY--;
        //                    currentTile = board[curX][curY];
        //                }
        //        }
        //        //S
        //        if (pawnY + 2 < board.Length)
        //        {
        //            var currentTile = board[pawnX][pawnY + 1];
        //            var curX = pawnX;
        //            var curY = pawnY + 1;

        //            var lastTileInSequence = currentTile;
        //            var lastX = pawnX;
        //            var lastY = pawnY;
        //            while (lastTileInSequence == enemyPlayerColor)
        //            {
        //                if (lastY + 1 >= board.Length) break;
        //                lastY++;
        //                lastTileInSequence = board[lastX][lastY];
        //            }

        //            if (lastTileInSequence == currentPlayerColor)
        //                while (currentTile == enemyPlayerColor)
        //                {
        //                    currentPlayerCapturedPawns.Add(new IntPair(curX, curY));

        //                    if (curY + 1 >= board.Length) break;

        //                    curY++;
        //                    currentTile = board[curX][curY];
        //                }
        //        }
        //        //NW
        //        if (pawnX - 2 >= 0 && pawnY - 2 >= 0)
        //        {
        //            var currentTile = board[pawnX - 1][pawnY - 1];
        //            var curX = pawnX - 1;
        //            var curY = pawnY - 1;

        //            var lastTileInSequence = currentTile;
        //            var lastX = pawnX;
        //            var lastY = pawnY;
        //            while (lastTileInSequence == enemyPlayerColor)
        //            {
        //                if (lastX - 1 < 0 || lastY - 1 < 0) break;
        //                lastX--;
        //                lastY--;
        //                lastTileInSequence = board[lastX][lastY];
        //            }

        //            if (lastTileInSequence == currentPlayerColor)
        //                while (currentTile == enemyPlayerColor)
        //                {
        //                    currentPlayerCapturedPawns.Add(new IntPair(curX, curY));

        //                    if (curX - 1 < 0 || curY - 1 < 0) break;

        //                    curX--;
        //                    curY--;
        //                    currentTile = board[curX][curY];
        //                }
        //        }
        //        //NE
        //        if (pawnX + 2 < board.Length && pawnY - 2 >= 0)
        //        {
        //            var currentTile = board[pawnX + 1][pawnY - 1];
        //            var curX = pawnX + 1;
        //            var curY = pawnY - 1;

        //            var lastTileInSequence = currentTile;
        //            var lastX = pawnX;
        //            var lastY = pawnY;
        //            while (lastTileInSequence == enemyPlayerColor)
        //            {
        //                if (lastX + 1 >= board.Length || lastY - 1 < 0) break;

        //                lastX++;
        //                lastY--;
        //                lastTileInSequence = board[lastX][lastY];
        //            }

        //            if (lastTileInSequence == currentPlayerColor)
        //                while (currentTile == enemyPlayerColor)
        //                {
        //                    currentPlayerCapturedPawns.Add(new IntPair(curX, curY));

        //                    if (curX + 1 >= board.Length || curY - 1 < 0) break;

        //                    curX++;
        //                    curY--;
        //                    currentTile = board[curX][curY];
        //                }
        //        }
        //        //SE
        //        if (pawnX + 2 < board.Length && pawnY + 2 < board.Length)
        //        {
        //            var currentTile = board[pawnX + 1][pawnY + 1];
        //            var curX = pawnX + 1;
        //            var curY = pawnY + 1;

        //            var lastTileInSequence = currentTile;
        //            var lastX = pawnX;
        //            var lastY = pawnY;
        //            while (lastTileInSequence == enemyPlayerColor)
        //            {
        //                if (lastX + 1 >= board.Length || lastY + 1 >= board.Length) break;

        //                lastX++;
        //                lastY++;
        //                lastTileInSequence = board[lastX][lastY];
        //            }

        //            if (lastTileInSequence == currentPlayerColor)
        //                while (currentTile == enemyPlayerColor)
        //                {
        //                    currentPlayerCapturedPawns.Add(new IntPair(curX, curY));

        //                    if (curX + 1 >= board.Length || curY + 1 >= board.Length) break;

        //                    curX++;
        //                    curY++;
        //                    currentTile = board[curX][curY];
        //                }
        //        }
        //        //SW
        //        if (pawnX - 2 >= 0 && pawnY + 2 < board.Length)
        //        {
        //            var currentTile = board[pawnX - 1][pawnY + 1];
        //            var curX = pawnX - 1;
        //            var curY = pawnY + 1;

        //            var lastTileInSequence = currentTile;
        //            var lastX = pawnX;
        //            var lastY = pawnY;
        //            while (lastTileInSequence == enemyPlayerColor)
        //            {
        //                if (lastX - 1 < 0 || lastY + 1 >= board.Length) break;

        //                lastX--;
        //                lastY++;
        //                lastTileInSequence = board[lastX][lastY];
        //            }

        //            if (lastTileInSequence == currentPlayerColor)
        //                while (currentTile == enemyPlayerColor)
        //                {
        //                    currentPlayerCapturedPawns.Add(new IntPair(curX, curY));

        //                    if (curX - 1 < 0 || curY + 1 >= board.Length) break;

        //                    curX--;
        //                    curY++
        //                        ;
        //                    currentTile = board[curX][curY];
        //                }
        //        }
        //    }


        //    return playerPawns.Count - currentPlayerCapturedPawns.Count;
        //}
    }
}
