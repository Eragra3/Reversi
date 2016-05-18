using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reversi.CustomControls;
using Reversi.Enums;

namespace Reversi.Logic
{
    public class MinMaxPlayer : ReversiPlayer
    {
        private TileStateEnum[][] _currentBoardState;

        private TileStateEnum _playerColor;

        private int _searchDepth;


        public int VisitedNodesCount;

        public MinMaxPlayer(int boardSize, TileStateEnum playerColor, AIStrategies strategy, int searchDepth = 4)
        {
            _currentBoardState = new TileStateEnum[boardSize][];
            for (var i = 0; i < _currentBoardState.Length; i++)
            {
                _currentBoardState[i] = new TileStateEnum[boardSize];
                for (var j = 0; j < _currentBoardState[i].Length; j++)
                {
                    _currentBoardState[i][j] = TileStateEnum.Empty;
                }
            }

            _playerColor = playerColor;
            _searchDepth = searchDepth;
            CurrentStrategy = strategy;
        }

        public override AiPlayerResult FindNextMove(Tile[][] gameState)
        {
            VisitedNodesCount = 0;

            for (var i = 0; i < _currentBoardState.Length; i++)
            {
                for (var j = 0; j < _currentBoardState[i].Length; j++)
                {
                    _currentBoardState[i][j] = gameState[i][j].State;
                }
            }

            var gameStateClone = DeepCloneGameState(_currentBoardState);

            var possiblePawnPlacements = ReversiHelpers.GetPossiblePawnPlacements(gameStateClone, _playerColor);

            PawnLightModel bestMove = null;
            var nicestNiceness = double.NegativeInfinity;
            foreach (var possibleMove in possiblePawnPlacements)
            {
                var moveNiceness = MinMax(gameStateClone, possibleMove, _searchDepth, true);

                if (!(moveNiceness > nicestNiceness)) continue;
                nicestNiceness = moveNiceness;
                bestMove = possibleMove;
            }

            var result = new AiPlayerResult()
            {
                Move = bestMove,
                SearchedNodes = VisitedNodesCount
            };

            return result;
        }

        private double MinMax(TileStateEnum[][] parentGameState, PawnLightModel move, int depth, bool maximizingPlayer)
        {
            VisitedNodesCount++;

            var gameStateClone = DeepCloneGameState(parentGameState);

            var capturedTiles = ReversiHelpers.PlacePawn(gameStateClone, move.State, move.X, move.Y);

            var nextMovePlayer = move.State == TileStateEnum.Black ? TileStateEnum.White : TileStateEnum.Black;
            var possiblePawnPlacements = ReversiHelpers.GetPossiblePawnPlacements(gameStateClone, nextMovePlayer);

            var pgs = new PotentialGameState(move)
            {
                CapturedTiles = capturedTiles,
                PossibleMoves = possiblePawnPlacements.Length
            };

            if (depth == 0)
            {
                return pgs.GetNiceness(CurrentStrategy);
            }

            if (possiblePawnPlacements.Length == 0)
            {
                return pgs.GetNiceness(CurrentStrategy);
            }

            double bestMove;
            if (maximizingPlayer)
            {
                bestMove = double.NegativeInfinity;
                foreach (var possibleMove in possiblePawnPlacements)
                {
                    var moveResult = MinMax(gameStateClone, possibleMove, depth - 1, false);
                    if (moveResult > bestMove)
                    {
                        bestMove = moveResult;
                    }
                }
            }
            else
            {
                bestMove = double.PositiveInfinity;
                foreach (var possibleMove in possiblePawnPlacements)
                {
                    var moveResult = MinMax(gameStateClone, possibleMove, depth - 1, true);
                    if (moveResult < bestMove)
                    {
                        bestMove = moveResult;
                    }
                }
            }

            return bestMove;
        }
    }
}
