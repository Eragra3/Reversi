using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reversi.CustomControls;
using Reversi.Enums;

namespace Reversi.Logic
{
    public class MinMaxPlayer
    {
        private PawnLightModel[][] _currentBoardState;

        private TileStateEnum _playerColor;

        private AIStrategies _currentStrategy;

        private int _searchDepth;

        public MinMaxPlayer(int boardSize, Enums.TileStateEnum playerColor, AIStrategies strategy, int searchDepth = 4)
        {
            _currentBoardState = new PawnLightModel[boardSize][];
            for (var i = 0; i < _currentBoardState.Length; i++)
            {
                _currentBoardState[i] = new PawnLightModel[boardSize];
                for (var j = 0; j < _currentBoardState[i].Length; j++)
                {
                    _currentBoardState[i][j] = new PawnLightModel();
                }
            }

            _playerColor = playerColor;
            _searchDepth = searchDepth;
            _currentStrategy = strategy;
        }

        public PawnLightModel FindNextMove(Tile[][] gameState)
        {
            for (var i = 0; i < _currentBoardState.Length; i++)
            {
                for (var j = 0; j < _currentBoardState[i].Length; j++)
                {
                    _currentBoardState[i][j].X = gameState[i][j].X;
                    _currentBoardState[i][j].Y = gameState[i][j].Y;
                    _currentBoardState[i][j].State = gameState[i][j].State;
                }
            }

            var gameStateClone = DeepCloneGameState(_currentBoardState);
            var pgs = new PotentialGameState(gameStateClone, null);

            var root = new Node<PotentialGameState>(null, pgs);

            MakeGameSubTree(root, _searchDepth, _playerColor);

            PotentialGameState bestPGS = null;
            double nicestNiceness = double.NegativeInfinity;
            foreach (var child in root.Children)
            {
                var niceness = GetSubTreeNiceness(child, true, _currentStrategy);

                if (niceness > nicestNiceness)
                {
                    nicestNiceness = niceness;
                    bestPGS = child.Value;
                }
            }


            return bestPGS?.MoveMade;
        }

        private double GetSubTreeNiceness(Node<PotentialGameState> node, bool maximizingPlayer, AIStrategies currentStrategy)
        {
            if (node.Children == null)
            {
                return node.Value.GetNiceness(currentStrategy);
            }

            double nicestNiceness;
            if (maximizingPlayer)
            {
                nicestNiceness = double.NegativeInfinity;
                foreach (var child in node.Children)
                {
                    var subtreeNiceness = GetSubTreeNiceness(child, false, currentStrategy);
                    if (subtreeNiceness > nicestNiceness)
                    {
                        nicestNiceness = subtreeNiceness;
                    }
                }
            }
            else
            {
                nicestNiceness = double.PositiveInfinity;
                foreach (var child in node.Children)
                {
                    var subtreeNiceness = GetSubTreeNiceness(child, true, currentStrategy);
                    if (subtreeNiceness < nicestNiceness)
                    {
                        nicestNiceness = subtreeNiceness;
                    }
                }
            }

            return nicestNiceness;
        }

        /// <summary>
        /// Recursive
        /// </summary>
        /// <param name="parent">Parent node</param>
        /// <param name="remainingDepth">Nodes to be created beneath this node</param>
        /// <param name="currentPlayerColor"></param>
        /// <returns></returns>
        private void MakeGameSubTree(Node<PotentialGameState> parent, int remainingDepth, TileStateEnum currentPlayerColor)
        {
            var possiblePawnPlacements = ReversiHelpers.GetPossiblePawnPlacements(parent.Value.GameState, currentPlayerColor);

            if (possiblePawnPlacements == null || !possiblePawnPlacements.Any()) return;

            parent.Children = new Node<PotentialGameState>[possiblePawnPlacements.Length];

            remainingDepth--;
            var opponentPlayerColor = currentPlayerColor == TileStateEnum.Black ? Enums.TileStateEnum.White : Enums.TileStateEnum.Black;

            for (var i = 0; i < parent.Children.Length; i++)
            {
                var pawn = possiblePawnPlacements[i];
                pawn.State = currentPlayerColor;

                var gameStateClone = DeepCloneGameState(parent.Value.GameState);
                var capturedTiles = ReversiHelpers.PlacePawn(gameStateClone, pawn);

                var pgs = new PotentialGameState(gameStateClone, pawn)
                {
                    CapturedTiles = capturedTiles
                };

                var child = new Node<PotentialGameState>(parent, pgs);
                parent.Children[i] = child;

                if (remainingDepth >= 0)
                {
                    MakeGameSubTree(child, remainingDepth, opponentPlayerColor);
                }
            }
        }

        private PawnLightModel[][] DeepCloneGameState(PawnLightModel[][] gameState)
        {
            var gameStateCopy = new PawnLightModel[gameState.Length][];
            for (var i = 0; i < gameStateCopy.Length; i++)
            {
                gameStateCopy[i] = new PawnLightModel[gameStateCopy.Length];
                for (var j = 0; j < gameStateCopy[i].Length; j++)
                {
                    gameStateCopy[i][j] = gameState[i][j].Clone();
                }
            }

            return gameStateCopy;
        }
    }
}
