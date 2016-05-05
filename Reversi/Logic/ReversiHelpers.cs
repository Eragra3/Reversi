using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reversi.Enums;

namespace Reversi.Logic
{
    public static class ReversiHelpers
    {
        //[ThreadStatic]
        //private static List<PawnLightModel> __possibleMovesCache;

        //static ReversiHelpers()
        //{
        //    __possibleMovesCache = new List<PawnLightModel>();
        //}

        public static PawnLightModel[] GetPossiblePawnPlacements(PawnLightModel[][] gameState, TileStateEnum playerColor)
        {
            var enemyPlayerColor = playerColor == TileStateEnum.Black ? TileStateEnum.White : TileStateEnum.Black;

            var currentPlayerTiles = new List<PawnLightModel>(gameState.Length * gameState.Length);
            for (var i = 0; i < gameState.Length; i++)
            {
                for (var j = 0; j < gameState[i].Length; j++)
                {
                    if (gameState[i][j].State == playerColor)
                    {
                        currentPlayerTiles.Add(gameState[i][j]);
                    }
                }
            }

            var possibleMoves = new HashSet<PawnLightModel>();

            foreach (var tile in currentPlayerTiles)
            {
                //W
                if (tile.X - 2 >= 0)
                {
                    var currentTile = gameState[tile.X - 1][tile.Y];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            possibleMoves.Add(currentTile);
                            break;
                        }
                        if (isTileInBetween && currentTile.State == playerColor)
                        {
                            break;
                        }

                        if (currentTile.X - 1 < 0) break;
                        currentTile = gameState[currentTile.X - 1][currentTile.Y];
                    }
                }
                //E
                if (tile.X + 2 < gameState.Length)
                {
                    var currentTile = gameState[tile.X + 1][tile.Y];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            possibleMoves.Add(currentTile);
                            break;
                        }
                        if (isTileInBetween && currentTile.State == playerColor)
                        {
                            break;
                        }

                        if (currentTile.X + 1 >= gameState.Length) break;
                        currentTile = gameState[currentTile.X + 1][currentTile.Y];
                    }
                }
                //N
                if (tile.Y - 2 >= 0)
                {
                    var currentTile = gameState[tile.X][tile.Y - 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            possibleMoves.Add(currentTile);
                            break;
                        }
                        if (isTileInBetween && currentTile.State == playerColor)
                        {
                            break;
                        }

                        if (currentTile.Y - 1 < 0) break;
                        currentTile = gameState[currentTile.X][currentTile.Y - 1];
                    }
                }
                //S
                if (tile.Y + 2 < gameState.Length)
                {
                    var currentTile = gameState[tile.X][tile.Y + 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            possibleMoves.Add(currentTile);
                            break;
                        }
                        if (isTileInBetween && currentTile.State == playerColor)
                        {
                            break;
                        }

                        if (currentTile.Y + 1 >= gameState.Length) break;
                        currentTile = gameState[currentTile.X][currentTile.Y + 1];
                    }
                }
                //NW
                if (tile.X - 2 >= 0 && tile.Y - 2 >= 0)
                {
                    var currentTile = gameState[tile.X - 1][tile.Y - 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            possibleMoves.Add(currentTile);
                            break;
                        }
                        if (isTileInBetween && currentTile.State == playerColor)
                        {
                            break;
                        }

                        if (currentTile.X - 1 < 0 || currentTile.Y - 1 < 0) break;
                        currentTile = gameState[currentTile.X - 1][currentTile.Y - 1];
                    }
                }
                //NE
                if (tile.X + 2 < gameState.Length && tile.Y - 2 >= 0)
                {
                    var currentTile = gameState[tile.X + 1][tile.Y - 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            possibleMoves.Add(currentTile);
                            break;
                        }
                        if (isTileInBetween && currentTile.State == playerColor)
                        {
                            break;
                        }

                        if (currentTile.X + 1 >= gameState.Length || currentTile.Y - 1 < 0) break;
                        currentTile = gameState[currentTile.X + 1][currentTile.Y - 1];
                    }
                }
                //SE
                if (tile.X + 2 < gameState.Length && tile.Y + 2 < gameState.Length)
                {
                    var currentTile = gameState[tile.X + 1][tile.Y + 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            possibleMoves.Add(currentTile);
                            break;
                        }
                        if (isTileInBetween && currentTile.State == playerColor)
                        {
                            break;
                        }

                        if (currentTile.X + 1 >= gameState.Length || currentTile.Y + 1 >= gameState.Length) break;
                        currentTile = gameState[currentTile.X + 1][currentTile.Y + 1];
                    }
                }
                //SW
                if (tile.X - 2 >= 0 && tile.Y + 2 < gameState.Length)
                {
                    var currentTile = gameState[tile.X - 1][tile.Y + 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            possibleMoves.Add(currentTile);
                            break;
                        }
                        if (isTileInBetween && currentTile.State == playerColor)
                        {
                            break;
                        }

                        if (currentTile.X - 1 < 0 || currentTile.Y + 1 >= gameState.Length) break;
                        currentTile = gameState[currentTile.X - 1][currentTile.Y + 1];
                    }
                }
            }
            return possibleMoves.ToArray();
        }

        public static int PlacePawn(PawnLightModel[][] board, PawnLightModel pawnToPlace)
        {
            board[pawnToPlace.X][pawnToPlace.Y] = pawnToPlace;

            return FlipPawns(board, pawnToPlace);
        }

        private static int FlipPawns(PawnLightModel[][] board, PawnLightModel pawn)
        {

            var currentPlayerColor = pawn.State;
            var enemyPlayerColor = currentPlayerColor == TileStateEnum.Black ? Enums.TileStateEnum.White : Enums.TileStateEnum.Black;

            var capturedPawns = 0;

            //W
            if (pawn.X - 2 >= 0)
            {
                var currentTile = board[pawn.X - 1][pawn.Y];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X - 1 < 0) break;
                    lastTileInSeqence = board[lastTileInSeqence.X - 1][lastTileInSeqence.Y];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();
                        capturedPawns++;

                        if (currentTile.X - 1 < 0) break;
                        currentTile = board[currentTile.X - 1][currentTile.Y];
                    }
            }
            //E
            if (pawn.X + 2 < board.Length)
            {
                var currentTile = board[pawn.X + 1][pawn.Y];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X + 1 >= board.Length) break;
                    lastTileInSeqence = board[lastTileInSeqence.X + 1][lastTileInSeqence.Y];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();
                        capturedPawns++;

                        if (currentTile.X + 1 >= board.Length) break;
                        currentTile = board[currentTile.X + 1][currentTile.Y];
                    }
            }
            //N
            if (pawn.Y - 2 >= 0)
            {
                var currentTile = board[pawn.X][pawn.Y - 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.Y - 1 < 0) break;
                    lastTileInSeqence = board[lastTileInSeqence.X][lastTileInSeqence.Y - 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();
                        capturedPawns++;

                        if (currentTile.Y - 1 < 0) break;
                        currentTile = board[currentTile.X][currentTile.Y - 1];
                    }
            }
            //S
            if (pawn.Y + 2 < board.Length)
            {
                var currentTile = board[pawn.X][pawn.Y + 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.Y + 1 >= board.Length) break;
                    lastTileInSeqence = board[lastTileInSeqence.X][lastTileInSeqence.Y + 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();
                        capturedPawns++;

                        if (currentTile.Y + 1 >= board.Length) break;
                        currentTile = board[currentTile.X][currentTile.Y + 1];
                    }
            }
            //NW
            if (pawn.X - 2 >= 0 && pawn.Y - 2 >= 0)
            {
                var currentTile = board[pawn.X - 1][pawn.Y - 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X - 1 < 0 || lastTileInSeqence.Y - 1 < 0) break;
                    lastTileInSeqence = board[lastTileInSeqence.X - 1][lastTileInSeqence.Y - 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();
                        capturedPawns++;

                        if (currentTile.X - 1 < 0 || currentTile.Y - 1 < 0) break;
                        currentTile = board[currentTile.X - 1][currentTile.Y - 1];
                    }
            }
            //NE
            if (pawn.X + 2 < board.Length && pawn.Y - 2 >= 0)
            {
                var currentTile = board[pawn.X + 1][pawn.Y - 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X + 1 >= board.Length || lastTileInSeqence.Y - 1 < 0) break;
                    lastTileInSeqence = board[lastTileInSeqence.X + 1][lastTileInSeqence.Y - 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();
                        capturedPawns++;

                        if (currentTile.X + 1 >= board.Length || currentTile.Y - 1 < 0) break;
                        currentTile = board[currentTile.X + 1][currentTile.Y - 1];
                    }
            }
            //SE
            if (pawn.X + 2 < board.Length && pawn.Y + 2 < board.Length)
            {
                var currentTile = board[pawn.X + 1][pawn.Y + 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X + 1 >= board.Length || lastTileInSeqence.Y + 1 >= board.Length) break;
                    lastTileInSeqence = board[lastTileInSeqence.X + 1][lastTileInSeqence.Y + 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();
                        capturedPawns++;

                        if (currentTile.X + 1 >= board.Length || currentTile.Y + 1 >= board.Length) break;
                        currentTile = board[currentTile.X + 1][currentTile.Y + 1];
                    }
            }
            //SW
            if (pawn.X - 2 >= 0 && pawn.Y + 2 < board.Length)
            {
                var currentTile = board[pawn.X - 1][pawn.Y + 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X - 1 < 0 || lastTileInSeqence.Y + 1 >= board.Length) break;
                    lastTileInSeqence = board[lastTileInSeqence.X - 1][lastTileInSeqence.Y + 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();
                        capturedPawns++;

                        if (currentTile.X - 1 < 0 || currentTile.Y + 1 >= board.Length) break;
                        currentTile = board[currentTile.X - 1][currentTile.Y + 1];
                    }
            }

            return capturedPawns;
        }
    }
}
