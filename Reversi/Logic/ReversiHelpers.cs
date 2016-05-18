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
        private static IComparer<PawnLightModel> comparer = new PawnLightModelComparer(Configuration.TileWeights);
        //[ThreadStatic]
        //private static List<PawnLightModel> __possibleMovesCache;

        //static ReversiHelpers()
        //{
        //    __possibleMovesCache = new List<PawnLightModel>();
        //}

        public static PawnLightModel[] GetPossiblePawnPlacements(TileStateEnum[][] gameState, TileStateEnum playerColor)
        {
            var enemyPlayerColor = playerColor == TileStateEnum.Black ? TileStateEnum.White : TileStateEnum.Black;

            var currentPlayerTiles = new List<PawnLightModel>(gameState.Length * gameState.Length);
            for (var i = 0; i < gameState.Length; i++)
            {
                for (var j = 0; j < gameState[i].Length; j++)
                {
                    if (gameState[i][j] == playerColor)
                    {
                        currentPlayerTiles.Add(new PawnLightModel
                        {
                            State = gameState[i][j],
                            X = i,
                            Y = j
                        });
                    }
                }
            }

            var possibleMoves = new HashSet<PawnLightModel>();

            foreach (var tile in currentPlayerTiles)
            {
                //W
                if (tile.X - 2 >= 0)
                {
                    var curX = tile.X - 1;
                    var curY = tile.Y;
                    var currentTile = gameState[curX][curY];

                    var isTileInBetween = false;
                    while (curX >= 0)
                    {
                        if (!isTileInBetween && currentTile == Enums.TileStateEnum.Empty)
                            break;
                        if (!isTileInBetween && currentTile == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile == Enums.TileStateEnum.Empty)
                        {
                            possibleMoves.Add(new PawnLightModel
                            {
                                State = currentTile,
                                X = curX,
                                Y = curY
                            });
                            break;
                        }
                        if (isTileInBetween && currentTile == playerColor)
                        {
                            break;
                        }

                        curX--;
                        if (curX < 0) break;
                        currentTile = gameState[curX][curY];
                    }
                }
                //E
                if (tile.X + 2 < gameState.Length)
                {
                    var curX = tile.X + 1;
                    var curY = tile.Y;
                    var currentTile = gameState[curX][curY];

                    var isTileInBetween = false;
                    while (curX < gameState.Length)
                    {
                        if (!isTileInBetween && currentTile == Enums.TileStateEnum.Empty)
                            break;
                        if (!isTileInBetween && currentTile == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile == Enums.TileStateEnum.Empty)
                        {
                            possibleMoves.Add(new PawnLightModel
                            {
                                State = currentTile,
                                X = curX,
                                Y = curY
                            });
                            break;
                        }
                        if (isTileInBetween && currentTile == playerColor)
                        {
                            break;
                        }

                        curX++;
                        if (curX >= gameState.Length) break;
                        currentTile = gameState[curX][curY];
                    }
                }
                //N
                if (tile.Y - 2 >= 0)
                {
                    var curX = tile.X;
                    var curY = tile.Y - 1;
                    var currentTile = gameState[curX][curY];

                    var isTileInBetween = false;
                    while (curY >= 0)
                    {
                        if (!isTileInBetween && currentTile == Enums.TileStateEnum.Empty)
                            break;
                        if (!isTileInBetween && currentTile == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile == Enums.TileStateEnum.Empty)
                        {
                            possibleMoves.Add(new PawnLightModel
                            {
                                State = currentTile,
                                X = curX,
                                Y = curY
                            });
                            break;
                        }
                        if (isTileInBetween && currentTile == playerColor)
                        {
                            break;
                        }

                        curY--;
                        if (curY < 0) break;
                        currentTile = gameState[curX][curY];
                    }
                }
                //S
                if (tile.Y + 2 < gameState.Length)
                {
                    var curX = tile.X;
                    var curY = tile.Y + 1;
                    var currentTile = gameState[curX][curY];

                    var isTileInBetween = false;
                    while (curY < gameState.Length)
                    {
                        if (!isTileInBetween && currentTile == Enums.TileStateEnum.Empty)
                            break;
                        if (!isTileInBetween && currentTile == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile == Enums.TileStateEnum.Empty)
                        {
                            possibleMoves.Add(new PawnLightModel
                            {
                                State = currentTile,
                                X = curX,
                                Y = curY
                            });
                            break;
                        }
                        if (isTileInBetween && currentTile == playerColor)
                        {
                            break;
                        }

                        curY++;
                        if (curY >= gameState.Length) break;
                        currentTile = gameState[curX][curY];
                    }
                }
                //NW
                if (tile.X - 2 >= 0 && tile.Y - 2 >= 0)
                {
                    var curX = tile.X - 1;
                    var curY = tile.Y - 1;
                    var currentTile = gameState[curX][curY];

                    var isTileInBetween = false;
                    while (curX >= 0 && curY >= 0)
                    {
                        if (!isTileInBetween && currentTile == Enums.TileStateEnum.Empty)
                            break;
                        if (!isTileInBetween && currentTile == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile == Enums.TileStateEnum.Empty)
                        {
                            possibleMoves.Add(new PawnLightModel
                            {
                                State = currentTile,
                                X = curX,
                                Y = curY
                            });
                            break;
                        }
                        if (isTileInBetween && currentTile == playerColor)
                        {
                            break;
                        }

                        curX--;
                        curY--;
                        if (curX < 0 || curY < 0) break;
                        currentTile = gameState[curX][curY];
                    }
                }
                //NE
                if (tile.X + 2 < gameState.Length && tile.Y - 2 >= 0)
                {
                    var curX = tile.X + 1;
                    var curY = tile.Y - 1;
                    var currentTile = gameState[curX][curY];

                    var isTileInBetween = false;
                    while (curX < gameState.Length && curY >= 0)
                    {
                        if (!isTileInBetween && currentTile == Enums.TileStateEnum.Empty)
                            break;
                        if (!isTileInBetween && currentTile == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile == Enums.TileStateEnum.Empty)
                        {
                            possibleMoves.Add(new PawnLightModel
                            {
                                State = currentTile,
                                X = curX,
                                Y = curY
                            });
                            break;
                        }
                        if (isTileInBetween && currentTile == playerColor)
                        {
                            break;
                        }

                        curX++;
                        curY--;
                        if (curX >= gameState.Length || curY < 0) break;
                        currentTile = gameState[curX][curY];
                    }
                }
                //SE
                if (tile.X + 2 < gameState.Length && tile.Y + 2 < gameState.Length)
                {
                    var curX = tile.X + 1;
                    var curY = tile.Y + 1;
                    var currentTile = gameState[curX][curY];

                    var isTileInBetween = false;
                    while (curX < gameState.Length && curY < gameState.Length)
                    {
                        if (!isTileInBetween && currentTile == Enums.TileStateEnum.Empty)
                            break;
                        if (!isTileInBetween && currentTile == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile == Enums.TileStateEnum.Empty)
                        {
                            possibleMoves.Add(new PawnLightModel
                            {
                                State = currentTile,
                                X = curX,
                                Y = curY
                            });
                            break;
                        }
                        if (isTileInBetween && currentTile == playerColor)
                        {
                            break;
                        }

                        curX++;
                        curY++;
                        if (curX >= gameState.Length || curY >= gameState.Length) break;
                        currentTile = gameState[curX][curY];
                    }
                }
                //SW
                if (tile.X - 2 >= 0 && tile.Y + 2 < gameState.Length)
                {
                    var curX = tile.X - 1;
                    var curY = tile.Y + 1;
                    var currentTile = gameState[curX][curY];

                    var isTileInBetween = false;
                    while (curX >= 0 && curY < gameState.Length)
                    {
                        if (!isTileInBetween && currentTile == Enums.TileStateEnum.Empty)
                            break;
                        if (!isTileInBetween && currentTile == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile == Enums.TileStateEnum.Empty)
                        {
                            possibleMoves.Add(new PawnLightModel
                            {
                                State = currentTile,
                                X = curX,
                                Y = curY
                            });
                            break;
                        }
                        if (isTileInBetween && currentTile == playerColor)
                        {
                            break;
                        }

                        curX--;
                        curY++;
                        if (curX < 0 || curY >= gameState.Length) break;
                        currentTile = gameState[curX][curY];
                    }
                }
            }

            foreach (var move in possibleMoves)
            {
                move.State = playerColor;
            }

            return possibleMoves.ToArray();
        }

        public static int PlacePawn(TileStateEnum[][] board, TileStateEnum pawnColor, int x, int y)
        {
            board[x][y] = pawnColor;

            return FlipPawns(board, pawnColor, x, y);
        }

        private static int FlipPawns(TileStateEnum[][] board, TileStateEnum pawnColor, int pawnX, int pawnY)
        {

            var currentPlayerColor = pawnColor;
            var enemyPlayerColor = currentPlayerColor == TileStateEnum.Black ? Enums.TileStateEnum.White : Enums.TileStateEnum.Black;

            var capturedPawns = 0;

            //W
            if (pawnX - 2 >= 0)
            {
                var currentTile = board[pawnX - 1][pawnY];
                var curX = pawnX - 1;
                var curY = pawnY;

                var lastTileInSequence = currentTile;
                var lastX = pawnX;
                var lastY = pawnY;

                while (lastTileInSequence == enemyPlayerColor)
                {
                    if (lastX - 1 < 0) break;
                    lastX--;
                    lastTileInSequence = board[lastX][lastY];
                }

                if (lastTileInSequence == currentPlayerColor)
                    while (currentTile == enemyPlayerColor)
                    {
                        board[curX][curY] = currentTile == TileStateEnum.Black
                            ? TileStateEnum.White
                            : TileStateEnum.Black;
                        capturedPawns++;

                        if (curX - 1 < 0) break;
                        curX--;
                        currentTile = board[curX][curY];
                    }
            }
            //E
            if (pawnX + 2 < board.Length)
            {
                var currentTile = board[pawnX + 1][pawnY];
                var curX = pawnX + 1;
                var curY = pawnY;

                var lastTileInSequence = currentTile;
                var lastX = pawnX;
                var lastY = pawnY;
                while (lastTileInSequence == enemyPlayerColor)
                {
                    if (lastX + 1 >= board.Length) break;
                    lastX++;
                    lastTileInSequence = board[lastX][lastY];
                }

                if (lastTileInSequence == currentPlayerColor)
                    while (currentTile == enemyPlayerColor)
                    {
                        board[curX][curY] = currentTile == TileStateEnum.Black
                            ? TileStateEnum.White
                            : TileStateEnum.Black;
                        capturedPawns++;

                        if (curX + 1 >= board.Length) break;

                        curX++;
                        currentTile = board[curX][curY];
                    }
            }
            //N
            if (pawnY - 2 >= 0)
            {
                var currentTile = board[pawnX][pawnY - 1];
                var curX = pawnX;
                var curY = pawnY - 1;

                var lastTileInSequence = currentTile;
                var lastX = pawnX;
                var lastY = pawnY;
                while (lastTileInSequence == enemyPlayerColor)
                {
                    if (lastY - 1 < 0) break;
                    lastY--;
                    lastTileInSequence = board[lastX][lastY];
                }

                if (lastTileInSequence == currentPlayerColor)
                    while (currentTile == enemyPlayerColor)
                    {
                        board[curX][curY] = currentTile == TileStateEnum.Black
                            ? TileStateEnum.White
                            : TileStateEnum.Black;
                        capturedPawns++;

                        if (curY - 1 < 0) break;
                        curY--;
                        currentTile = board[curX][curY];
                    }
            }
            //S
            if (pawnY + 2 < board.Length)
            {
                var currentTile = board[pawnX][pawnY + 1];
                var curX = pawnX;
                var curY = pawnY + 1;

                var lastTileInSequence = currentTile;
                var lastX = pawnX;
                var lastY = pawnY;
                while (lastTileInSequence == enemyPlayerColor)
                {
                    if (lastY + 1 >= board.Length) break;
                    lastY++;
                    lastTileInSequence = board[lastX][lastY];
                }

                if (lastTileInSequence == currentPlayerColor)
                    while (currentTile == enemyPlayerColor)
                    {
                        board[curX][curY] = currentTile == TileStateEnum.Black
                            ? TileStateEnum.White
                            : TileStateEnum.Black;
                        capturedPawns++;

                        if (curY + 1 >= board.Length) break;

                        curY++;
                        currentTile = board[curX][curY];
                    }
            }
            //NW
            if (pawnX - 2 >= 0 && pawnY - 2 >= 0)
            {
                var currentTile = board[pawnX - 1][pawnY - 1];
                var curX = pawnX - 1;
                var curY = pawnY - 1;

                var lastTileInSequence = currentTile;
                var lastX = pawnX;
                var lastY = pawnY;
                while (lastTileInSequence == enemyPlayerColor)
                {
                    if (lastX - 1 < 0 || lastY - 1 < 0) break;
                    lastX--;
                    lastY--;
                    lastTileInSequence = board[lastX][lastY];
                }

                if (lastTileInSequence == currentPlayerColor)
                    while (currentTile == enemyPlayerColor)
                    {
                        board[curX][curY] = currentTile == TileStateEnum.Black
                            ? TileStateEnum.White
                            : TileStateEnum.Black;
                        capturedPawns++;

                        if (curX - 1 < 0 || curY - 1 < 0) break;

                        curX--;
                        curY--;
                        currentTile = board[curX][curY];
                    }
            }
            //NE
            if (pawnX + 2 < board.Length && pawnY - 2 >= 0)
            {
                var currentTile = board[pawnX + 1][pawnY - 1];
                var curX = pawnX + 1;
                var curY = pawnY - 1;

                var lastTileInSequence = currentTile;
                var lastX = pawnX;
                var lastY = pawnY;
                while (lastTileInSequence == enemyPlayerColor)
                {
                    if (lastX + 1 >= board.Length || lastY - 1 < 0) break;

                    lastX++;
                    lastY--;
                    lastTileInSequence = board[lastX][lastY];
                }

                if (lastTileInSequence == currentPlayerColor)
                    while (currentTile == enemyPlayerColor)
                    {
                        board[curX][curY] = currentTile == TileStateEnum.Black
                            ? TileStateEnum.White
                            : TileStateEnum.Black;
                        capturedPawns++;

                        if (curX + 1 >= board.Length || curY - 1 < 0) break;

                        curX++;
                        curY--;
                        currentTile = board[curX][curY];
                    }
            }
            //SE
            if (pawnX + 2 < board.Length && pawnY + 2 < board.Length)
            {
                var currentTile = board[pawnX + 1][pawnY + 1];
                var curX = pawnX + 1;
                var curY = pawnY + 1;

                var lastTileInSequence = currentTile;
                var lastX = pawnX;
                var lastY = pawnY;
                while (lastTileInSequence == enemyPlayerColor)
                {
                    if (lastX + 1 >= board.Length || lastY + 1 >= board.Length) break;

                    lastX++;
                    lastY++;
                    lastTileInSequence = board[lastX][lastY];
                }

                if (lastTileInSequence == currentPlayerColor)
                    while (currentTile == enemyPlayerColor)
                    {
                        board[curX][curY] = currentTile == TileStateEnum.Black
                            ? TileStateEnum.White
                            : TileStateEnum.Black;
                        capturedPawns++;

                        if (curX + 1 >= board.Length || curY + 1 >= board.Length) break;

                        curX++;
                        curY++;
                        currentTile = board[curX][curY];
                    }
            }
            //SW
            if (pawnX - 2 >= 0 && pawnY + 2 < board.Length)
            {
                var currentTile = board[pawnX - 1][pawnY + 1];
                var curX = pawnX - 1;
                var curY = pawnY + 1;

                var lastTileInSequence = currentTile;
                var lastX = pawnX;
                var lastY = pawnY;
                while (lastTileInSequence == enemyPlayerColor)
                {
                    if (lastX - 1 < 0 || lastY + 1 >= board.Length) break;

                    lastX--;
                    lastY++;
                    lastTileInSequence = board[lastX][lastY];
                }

                if (lastTileInSequence == currentPlayerColor)
                    while (currentTile == enemyPlayerColor)
                    {
                        board[curX][curY] = currentTile == TileStateEnum.Black
                            ? TileStateEnum.White
                            : TileStateEnum.Black;
                        capturedPawns++;

                        if (curX - 1 < 0 || curY + 1 >= board.Length) break;

                        curX--;
                        curY++
                            ;
                        currentTile = board[curX][curY];
                    }
            }

            return capturedPawns;
        }

        public static void OrderTreeTraversal(PawnLightModel[] possibleMoves)
        {
            Array.Sort(possibleMoves, comparer);
        }
    }
}
