using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    public static class Configuration
    {
        public const int BOARD_SIZE = 8;

        private static int[][] _tileWeights;

        public static int[][] TileWeights
        {
            get
            {
                if (_tileWeights == null || _tileWeights.Length != BOARD_SIZE)
                {
                    _tileWeights = new int[BOARD_SIZE][];
                    for (int i = 0; i < _tileWeights.Length; i++)
                    {
                        _tileWeights[i] = new int[BOARD_SIZE];
                    }

                    //diagonall
                    for (int i = 0; i < _tileWeights.Length; i++)
                    {
                        _tileWeights[i][i] = 1;
                        _tileWeights[_tileWeights.Length - i - 1][i] = 1;
                    }

                    //walls inner neighbours
                    for (int n = 0; n < (_tileWeights.Length - 4) / 2; n++)
                    {
                        for (int i = 0; i < _tileWeights.Length; i++)
                        {
                            _tileWeights[i][0 + n] = -1;
                            _tileWeights[0 + n][i] = -1;
                            _tileWeights[i][_tileWeights.Length - 1 - n] = -1;
                            _tileWeights[_tileWeights.Length - 1 - n][i] = -1;
                        }
                    }

                    //walls
                    for (int i = 0; i < _tileWeights.Length; i++)
                    {
                        _tileWeights[i][0] = 2;
                        _tileWeights[0][i] = 2;
                        _tileWeights[i][_tileWeights.Length - 1] = 2;
                        _tileWeights[_tileWeights.Length - 1][i] = 2;
                    }

                    //corners
                    _tileWeights[0][0] = 4;
                    _tileWeights[_tileWeights.Length - 1][0] = 4;
                    _tileWeights[0][_tileWeights.Length - 1] = 4;
                    _tileWeights[_tileWeights.Length - 1][_tileWeights.Length - 1] = 4;

                    //corner neighbours
                    _tileWeights[1][0] = -3;
                    _tileWeights[0][1] = -3;
                    _tileWeights[1][1] = -4;

                    _tileWeights[_tileWeights.Length - 2][0] = -3;
                    _tileWeights[_tileWeights.Length - 1][1] = -3;
                    _tileWeights[_tileWeights.Length - 2][1] = -4;

                    _tileWeights[1][_tileWeights.Length - 1] = -3;
                    _tileWeights[0][_tileWeights.Length - 2] = -3;
                    _tileWeights[1][_tileWeights.Length - 2] = -4;

                    _tileWeights[_tileWeights.Length - 2][_tileWeights.Length - 1] = -3;
                    _tileWeights[_tileWeights.Length - 1][_tileWeights.Length - 2] = -3;
                    _tileWeights[_tileWeights.Length - 2][_tileWeights.Length - 2] = -4;


                    for (int i = 0; i < _tileWeights.Length; i++)
                    {
                        for (int j = 0; j < _tileWeights.Length; j++)
                        {
                            Console.Write($"{_tileWeights[j][i].ToString().PadLeft(2)} ");
                        }
                        Console.WriteLine();
                    }
                }

                return _tileWeights;
            }
        }
    }
}
