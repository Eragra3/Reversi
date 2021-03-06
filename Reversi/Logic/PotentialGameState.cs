﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reversi.CustomControls;
using Reversi.Enums;

namespace Reversi.Logic
{
    public class PotentialGameState
    {
        public PawnLightModel MoveMade;

        public int CapturedTiles;

        public int PossibleMoves;

        /// <summary>
        /// Creates new game state and remembers which pawn was placed this turn
        /// gameState should have pawn already placed and game state should be valid
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="moveMade"></param>
        public PotentialGameState(PawnLightModel moveMade)
        {
            MoveMade = moveMade;
        }

        /*
         * this method cannot use GameState, it should be already disposed
         */
        public double GetNiceness(AIStrategies strategy)
        {
            switch (strategy)
            {
                case AIStrategies.MostCapturedTiles:
                    return CapturedTiles;
                case AIStrategies.Mobility:
                    return PossibleMoves;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }
        }

        public override string ToString()
        {
            return MoveMade.ToString();
        }
    }
}
