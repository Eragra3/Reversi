using Reversi.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Reversi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int BoardSize { get; set; }

        private Tile[][] _board;

        private bool _blackTurn;
        private bool BlackTurn
        {
            get
            {
                return _blackTurn;
            }
            set
            {
                _blackTurn = value;
                if (_blackTurn)
                {
                    PlayerTurnIndicatorLabel.Background = Brushes.Black;
                    PlayerTurnIndicatorLabel.Foreground = Brushes.White;
                    PlayerTurnIndicatorLabel.Content = "Black";
                }
                else
                {
                    PlayerTurnIndicatorLabel.Background = Brushes.White;
                    PlayerTurnIndicatorLabel.Foreground = Brushes.Black;
                    PlayerTurnIndicatorLabel.Content = "White";
                }
            }
        }

        private HashSet<Tile> _possibleMoves;

        public MainWindow()
        {
            InitializeComponent();

            BoardSize = Configuration.BOARD_SIZE;
            _possibleMoves = new HashSet<Tile>();

            DrawBoard();

            BlackTurn = true;
            PlaceInitialPawns();

            //todo
            ShowMoveHelpers();
        }

        private void DrawBoard()
        {
            if (_board == null || _board.Length != BoardSize)
            {
                _board = new Tile[BoardSize][];

                for (var i = 0; i < _board.Length; i++)
                {
                    _board[i] = new Tile[BoardSize];
                }
            }

            for (var i = 0; i < BoardSize; i++)
            {
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                BoardGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                for (var j = 0; j < BoardSize; j++)
                {
                    var tile = new Tile();
                    tile.SetValue(Grid.RowProperty, i);
                    tile.SetValue(Grid.ColumnProperty, j);
                    tile.X = j;
                    tile.Y = i;

                    tile.MouseUp += (sender, args) => PlaceStoneAt((Tile)sender);

                    _board[j][i] = tile;
                    BoardGrid.Children.Add(tile);
                }
            }
        }

        private void PlaceInitialPawns()
        {
            var rightX = BoardSize / 2;
            var bottomY = BoardSize / 2;

            _board[rightX][bottomY].PutWhite();
            _board[rightX - 1][bottomY].PutBlack();
            _board[rightX - 1][bottomY - 1].PutWhite();
            _board[rightX][bottomY - 1].PutBlack();

            UpdatePossibleMoves();
        }

        private void PlaceStoneAt(Tile tile)
        {
            if (!(tile.State == Enums.TileStateEnum.Empty))
            {
                ShowInvalidMoveLabel("Must place pawn on empty tile");
                return;
            }
            if (!(_possibleMoves.Contains(tile)))
            {
                ShowInvalidMoveLabel("This move is illegal. You can use 'show hints' to see possible moves");
                return;
            }


            if (BlackTurn)
            {
                tile.PutBlack();
            }
            else
            {
                tile.PutWhite();
            }
            FlipPawns(tile);

            BlackTurn = !BlackTurn;

            UpdatePossibleMoves();

            HideMoveHelpers();
            ShowMoveHelpers();
        }

        #region helpers
        private void UpdatePossibleMoves()
        {
            _possibleMoves.Clear();

            var currentPlayerTiles = new List<Tile>(BoardSize * BoardSize);

            for (int i = 0; i < _board.Length; i++)
            {
                for (int j = 0; j < _board[i].Length; j++)
                {
                    if (BlackTurn)
                    {
                        if (_board[i][j].State == Enums.TileStateEnum.Black)
                        {
                            currentPlayerTiles.Add(_board[i][j]);
                        }
                    }
                    else
                    {
                        if (_board[i][j].State == Enums.TileStateEnum.White)
                        {
                            currentPlayerTiles.Add(_board[i][j]);
                        }
                    }
                }
            }

            var currentPlayerColor = BlackTurn ? Enums.TileStateEnum.Black : Enums.TileStateEnum.White;
            var enemyPlayerColor = BlackTurn ? Enums.TileStateEnum.White : Enums.TileStateEnum.Black;

            foreach (var tile in currentPlayerTiles)
            {
                //W
                if (tile.X - 2 >= 0)
                {
                    var currentTile = _board[tile.X - 1][tile.Y];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        else if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            _possibleMoves.Add(currentTile);
                            break;
                        }
                        else if (isTileInBetween && currentTile.State == currentPlayerColor)
                        {
                            break;
                        }

                        if (currentTile.X - 1 < 0) break;
                        currentTile = _board[currentTile.X - 1][currentTile.Y];
                    }
                }
                //E
                if (tile.X + 2 < _board.Length)
                {
                    var currentTile = _board[tile.X + 1][tile.Y];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        else if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            _possibleMoves.Add(currentTile);
                            break;
                        }
                        else if (isTileInBetween && currentTile.State == currentPlayerColor)
                        {
                            break;
                        }

                        if (currentTile.X + 1 >= _board.Length) break;
                        currentTile = _board[currentTile.X + 1][currentTile.Y];
                    }
                }
                //N
                if (tile.Y - 2 >= 0)
                {
                    var currentTile = _board[tile.X][tile.Y - 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        else if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            _possibleMoves.Add(currentTile);
                            break;
                        }
                        else if (isTileInBetween && currentTile.State == currentPlayerColor)
                        {
                            break;
                        }

                        if (currentTile.Y - 1 < 0) break;
                        currentTile = _board[currentTile.X][currentTile.Y - 1];
                    }
                }
                //S
                if (tile.Y + 2 < _board.Length)
                {
                    var currentTile = _board[tile.X][tile.Y + 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        else if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            _possibleMoves.Add(currentTile);
                            break;
                        }
                        else if (isTileInBetween && currentTile.State == currentPlayerColor)
                        {
                            break;
                        }

                        if (currentTile.Y + 1 >= _board.Length) break;
                        currentTile = _board[currentTile.X][currentTile.Y + 1];
                    }
                }
                //NW
                if (tile.X - 2 >= 0 && tile.Y - 2 >= 0)
                {
                    var currentTile = _board[tile.X - 1][tile.Y - 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        else if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            _possibleMoves.Add(currentTile);
                            break;
                        }
                        else if (isTileInBetween && currentTile.State == currentPlayerColor)
                        {
                            break;
                        }

                        if (currentTile.X - 1 < 0 || currentTile.Y - 1 < 0) break;
                        currentTile = _board[currentTile.X - 1][currentTile.Y - 1];
                    }
                }
                //NE
                if (tile.X + 2 < _board.Length && tile.Y - 2 >= 0)
                {
                    var currentTile = _board[tile.X + 1][tile.Y - 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        else if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            _possibleMoves.Add(currentTile);
                            break;
                        }
                        else if (isTileInBetween && currentTile.State == currentPlayerColor)
                        {
                            break;
                        }

                        if (currentTile.X + 1 >= _board.Length || currentTile.Y - 1 < 0) break;
                        currentTile = _board[currentTile.X + 1][currentTile.Y - 1];
                    }
                }
                //SE
                if (tile.X + 2 < _board.Length && tile.Y + 2 < _board.Length)
                {
                    var currentTile = _board[tile.X + 1][tile.Y + 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        else if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            _possibleMoves.Add(currentTile);
                            break;
                        }
                        else if (isTileInBetween && currentTile.State == currentPlayerColor)
                        {
                            break;
                        }

                        if (currentTile.X + 1 >= _board.Length || currentTile.Y + 1 >= _board.Length) break;
                        currentTile = _board[currentTile.X + 1][currentTile.Y + 1];
                    }
                }
                //SW
                if (tile.X - 2 >= 0 && tile.Y + 2 < _board.Length)
                {
                    var currentTile = _board[tile.X - 1][tile.Y + 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        else if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            _possibleMoves.Add(currentTile);
                            break;
                        }
                        else if (isTileInBetween && currentTile.State == currentPlayerColor)
                        {
                            break;
                        }

                        if (currentTile.X - 1 < 0 || currentTile.Y + 1 >= _board.Length) break;
                        currentTile = _board[currentTile.X - 1][currentTile.Y + 1];
                    }
                }
            }
        }
        private void FlipPawns(Tile tile)
        {
            var currentPlayerColor = BlackTurn ? Enums.TileStateEnum.Black : Enums.TileStateEnum.White;
            var enemyPlayerColor = BlackTurn ? Enums.TileStateEnum.White : Enums.TileStateEnum.Black;

            //W
            if (tile.X - 2 >= 0)
            {
                var currentTile = _board[tile.X - 1][tile.Y];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X - 1 < 0) break;
                    lastTileInSeqence = _board[lastTileInSeqence.X - 1][lastTileInSeqence.Y];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();

                        if (currentTile.X - 1 < 0) break;
                        currentTile = _board[currentTile.X - 1][currentTile.Y];
                    }
            }
            //E
            if (tile.X + 2 < _board.Length)
            {
                var currentTile = _board[tile.X + 1][tile.Y];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X + 1 >= _board.Length) break;
                    lastTileInSeqence = _board[lastTileInSeqence.X + 1][lastTileInSeqence.Y];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();

                        if (currentTile.X + 1 >= _board.Length) break;
                        currentTile = _board[currentTile.X + 1][currentTile.Y];
                    }
            }
            //N
            if (tile.Y - 2 >= 0)
            {
                var currentTile = _board[tile.X][tile.Y - 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.Y - 1 < 0) break;
                    lastTileInSeqence = _board[lastTileInSeqence.X][lastTileInSeqence.Y - 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();

                        if (currentTile.Y - 1 < 0) break;
                        currentTile = _board[currentTile.X][currentTile.Y - 1];
                    }
            }
            //S
            if (tile.Y + 2 < _board.Length)
            {
                var currentTile = _board[tile.X][tile.Y + 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.Y + 1 >= _board.Length) break;
                    lastTileInSeqence = _board[lastTileInSeqence.X][lastTileInSeqence.Y + 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();

                        if (currentTile.Y + 1 >= _board.Length) break;
                        currentTile = _board[currentTile.X][currentTile.Y + 1];
                    }
            }
            //NW
            if (tile.X - 2 >= 0 && tile.Y - 2 >= 0)
            {
                var currentTile = _board[tile.X - 1][tile.Y - 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X - 1 < 0 || lastTileInSeqence.Y - 1 < 0) break;
                    lastTileInSeqence = _board[lastTileInSeqence.X - 1][lastTileInSeqence.Y - 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();

                        if (currentTile.X - 1 < 0 || currentTile.Y - 1 < 0) break;
                        currentTile = _board[currentTile.X - 1][currentTile.Y - 1];
                    }
            }
            //NE
            if (tile.X + 2 < _board.Length && tile.Y - 2 >= 0)
            {
                var currentTile = _board[tile.X + 1][tile.Y - 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X + 1 >= _board.Length || lastTileInSeqence.Y - 1 < 0) break;
                    lastTileInSeqence = _board[lastTileInSeqence.X + 1][lastTileInSeqence.Y - 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();

                        if (currentTile.X + 1 >= _board.Length || currentTile.Y - 1 < 0) break;
                        currentTile = _board[currentTile.X + 1][currentTile.Y - 1];
                    }
            }
            //SE
            if (tile.X + 2 < _board.Length && tile.Y + 2 < _board.Length)
            {
                var currentTile = _board[tile.X + 1][tile.Y + 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X + 1 >= _board.Length || lastTileInSeqence.Y + 1 >= _board.Length) break;
                    lastTileInSeqence = _board[lastTileInSeqence.X + 1][lastTileInSeqence.Y + 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();

                        if (currentTile.X + 1 >= _board.Length || currentTile.Y + 1 >= _board.Length) break;
                        currentTile = _board[currentTile.X + 1][currentTile.Y + 1];
                    }
            }
            //SW
            if (tile.X - 2 >= 0 && tile.Y + 2 < _board.Length)
            {
                var currentTile = _board[tile.X - 1][tile.Y + 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X - 1 < 0 || lastTileInSeqence.Y + 1 >= _board.Length) break;
                    lastTileInSeqence = _board[lastTileInSeqence.X - 1][lastTileInSeqence.Y + 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();

                        if (currentTile.X - 1 < 0 || currentTile.Y + 1 >= _board.Length) break;
                        currentTile = _board[currentTile.X - 1][currentTile.Y + 1];
                    }
            }
        }
        #endregion

        #region  UI
        private int invalidMoveSemaphore = 0;
        private async void ShowInvalidMoveLabel(string message = "")
        {
            InvalidMoveLabel.Visibility = Visibility.Visible;
            InvalidMoveLabel.Content = message;

            invalidMoveSemaphore++;

            await Task.Delay(3000);

            invalidMoveSemaphore--;

            if (invalidMoveSemaphore == 0)
            {
                InvalidMoveLabel.Visibility = Visibility.Hidden;
            }
        }
        private void ShowMoveHelpers()
        {
            foreach (var tile in _possibleMoves)
            {
                tile.Background = Brushes.LightPink;
            }
        }
        private void HideMoveHelpers()
        {
            foreach (var row in _board)
            {
                foreach (var tile in row)
                {
                    tile.Background = Brushes.DarkGreen;
                }
            }
        }
        #endregion
    }
}
