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

        private AIStrategies _currentStrategy;

        private int _searchDepth;

        public MinMaxPlayer(int boardSize, Enums.TileStateEnum playerColor, AIStrategies strategy, int searchDepth = 4)
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
            _currentStrategy = strategy;
        }

        public override PawnLightModel? FindNextMove(Tile[][] gameState)
        {
            for (var i = 0; i < _currentBoardState.Length; i++)
            {
                for (var j = 0; j < _currentBoardState[i].Length; j++)
                {
                    _currentBoardState[i][j] = gameState[i][j].State;
                }
            }

            var gameStateClone = DeepCloneGameState(_currentBoardState);

            var root = new Node<PotentialGameState>(null);

            MakeGameSubTree(root, _searchDepth, _playerColor, gameStateClone);

            PotentialGameState bestPgs = null;
            var nicestNiceness = double.NegativeInfinity;
            foreach (var child in root.Children)
            {
                var niceness = GetSubTreeNiceness(child, true, _currentStrategy);

                if (!(niceness > nicestNiceness)) continue;
                nicestNiceness = niceness;
                bestPgs = child.Value;
            }


            return bestPgs?.MoveMade;
        }

        private double GetSubTreeNiceness(Node<PotentialGameState> node, bool maximizingPlayer, AIStrategies currentStrategy)
        {
            if (node.Children == null || !node.Children.Any())
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
        /// <param name="gameState"></param>
        /// <returns></returns>
        private void MakeGameSubTree(Node<PotentialGameState> parent, int remainingDepth, TileStateEnum currentPlayerColor, TileStateEnum[][] gameState)
        {
            var possiblePawnPlacements = ReversiHelpers.GetPossiblePawnPlacements(gameState, currentPlayerColor);


            if (!possiblePawnPlacements.Any())
            {
                parent.Children = new Node<PotentialGameState>[0];
                return;
            }

            parent.Children = new Node<PotentialGameState>[possiblePawnPlacements.Length];

            remainingDepth--;
            var opponentPlayerColor = currentPlayerColor == TileStateEnum.Black ? TileStateEnum.White : TileStateEnum.Black;

            for (var i = 0; i < parent.Children.Length; i++)
            {
                var pawn = possiblePawnPlacements[i];
                pawn.State = currentPlayerColor;

                var gameStateClone = DeepCloneGameState(gameState);
                var capturedTiles = ReversiHelpers.PlacePawn(gameStateClone, pawn.State, pawn.X, pawn.Y);

                var pgs = new PotentialGameState(pawn)
                {
                    CapturedTiles = capturedTiles
                };

                var child = new Node<PotentialGameState>(pgs);
                parent.Children[i] = child;

                if (remainingDepth >= 0)
                {
                    MakeGameSubTree(child, remainingDepth, opponentPlayerColor, gameStateClone);
                }
            }
        }
    }
}
