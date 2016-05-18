using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reversi.CustomControls;
using Reversi.Enums;

namespace Reversi.Logic
{
    public class AlfaBetaPlayer : ReversiPlayer
    {
        private TileStateEnum[][] _currentBoardState;

        private TileStateEnum _playerColor;

        private int _searchDepth;

        private int _visitedNodesCount;

        public AlfaBetaPlayer(int boardSize, TileStateEnum playerColor, AIStrategies strategy, int searchDepth = 4)
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
            _visitedNodesCount = 0;

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
                var moveNiceness = AlfaBeta(gameStateClone, possibleMove, double.NegativeInfinity, double.PositiveInfinity, _searchDepth, true);

                if (!(moveNiceness > nicestNiceness)) continue;
                nicestNiceness = moveNiceness;
                bestMove = possibleMove;
            }

            var result = new AiPlayerResult()
            {
                Move = bestMove,
                SearchedNodes = _visitedNodesCount
            };

            return result;
        }

        private double AlfaBeta(TileStateEnum[][] parentGameState, PawnLightModel move, double alfa, double beta, int depth, bool maximizingPlayer)
        {
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

            //order nodes
            //ReversiHelpers.OrderTreeTraversal(possiblePawnPlacements);

            double bestMove;
            if (maximizingPlayer)
            {
                bestMove = double.NegativeInfinity;
                foreach (var possibleMove in possiblePawnPlacements)
                {
                    _visitedNodesCount++;

                    var moveResult = AlfaBeta(gameStateClone, possibleMove, alfa, beta, depth - 1, false);
                    if (moveResult > bestMove)
                    {
                        bestMove = moveResult;
                    }

                    alfa = Math.Max(alfa, bestMove);
                    if (beta <= alfa)
                    {
                        break;
                    }
                }
            }
            else
            {
                bestMove = double.PositiveInfinity;
                foreach (var possibleMove in possiblePawnPlacements)
                {
                    _visitedNodesCount++;

                    var moveResult = AlfaBeta(gameStateClone, possibleMove, alfa, beta, depth - 1, true);
                    if (moveResult < bestMove)
                    {
                        bestMove = moveResult;
                    }

                    beta = Math.Min(alfa, bestMove);
                    if (beta <= alfa)
                    {
                        break;
                    }
                }
            }

            return bestMove;
        }
    }
}
